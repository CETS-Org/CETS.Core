using Application.Implementations.IDN;
using Application.Interfaces.COM;
using Application.Interfaces.IDN;
using AutoMapper;
using Domain.Entities.MongoDB;
using Domain.Interfaces.COM;
using DTOs.COM.COM_Chat.Requests;
using DTOs.COM.COM_Chat.Responses;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Implementations.COM
{
    public class COM_ChatService : ICOM_ChatService
    {
        private readonly ICOM_ChatRepository _repository;
        private readonly IIDN_AccountService _accountService;
        private readonly IMapper _mapper;
        private readonly IChatEventPublisher _eventPublisher;
        private readonly ILogger<COM_ChatService> _logger;

        public COM_ChatService(
            IIDN_AccountService accountService,
            ICOM_ChatRepository repository,
            IMapper mapper,
            IChatEventPublisher eventPublisher,
            ILogger<COM_ChatService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _eventPublisher = eventPublisher;
            _logger = logger;
            _accountService = accountService;
        }

        public async Task<ChatRoomResponse> CreateRoomAsync(CreateChatRoomRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (request.MemberIds == null || !request.MemberIds.Any())
                throw new ArgumentException("Members cannot be empty.");

            // Normalize member IDs to uppercase for consistency (matching notification system)
            var normalizedMemberIds = request.MemberIds
                .Select(id => string.IsNullOrWhiteSpace(id) ? id : id.ToUpperInvariant())
                .Where(id => !string.IsNullOrWhiteSpace(id))
                .Distinct()
                .ToList();

            // Logic cho phòng 1v1: Kiểm tra đã tồn tại chưa
            if (request.Type.ToLower() == "private" && normalizedMemberIds.Count == 2)
            {
                var existingRoom = await _repository.GetPrivateRoomByMembersAsync(normalizedMemberIds[0], normalizedMemberIds[1]);
                if (existingRoom != null)
                {
                    return _mapper.Map<ChatRoomResponse>(existingRoom);
                }
            }

            var roomEntity = _mapper.Map<COM_ChatRoom>(request);
            roomEntity.CreatedAt = DateTime.UtcNow;
            roomEntity.LastMessageAt = DateTime.UtcNow; // Set default để sort không bị null

            // Xử lý chuẩn hóa MemberIds (Unique và uppercase)
            roomEntity.MemberIds = normalizedMemberIds;

            var createdRoom = await _repository.CreateRoomAsync(roomEntity);
            return _mapper.Map<ChatRoomResponse>(createdRoom);
        }

        public async Task<ChatRoomResponse?> GetRoomByIdAsync(string roomId)
        {
            if (string.IsNullOrWhiteSpace(roomId)) return null;
            var room = await _repository.GetRoomByIdAsync(roomId);
            return room == null ? null : _mapper.Map<ChatRoomResponse>(room);
        }

        public async Task<List<ChatRoomResponse>> GetUserRoomsAsync(string userId)
        {
            // Normalize userId to uppercase for consistency
            var normalizedUserId = string.IsNullOrWhiteSpace(userId) ? userId : userId.ToUpperInvariant();
            
            // 1. Lấy danh sách phòng từ MongoDB
            var rooms = await _repository.GetRoomsByUserIdAsync(normalizedUserId);
            var responseList = _mapper.Map<List<ChatRoomResponse>>(rooms);

            // 2. Lấy danh sách ID thành viên (đang là String) và lọc trùng
            var allMemberStringIds = responseList
                .SelectMany(r => r.MemberIds)
                .Distinct()
                .ToList();

            // 3. Chuyển đổi String ID sang Guid ID (để gọi sang Identity Service)
            var allMemberGuids = allMemberStringIds
                .Select(id => Guid.TryParse(id, out var g) ? g : Guid.Empty)
                .Where(g => g != Guid.Empty)
                .ToList();

            // 4. Gọi Service lấy thông tin User (Hàm GetUsersByIdsAsync bạn vừa viết)
            // Lưu ý: Đảm bảo bạn đã inject IIDN_AccountService vào constructor và đặt tên biến là _accountService (hoặc _userService)
            var usersInfo = await _accountService.GetUsersByIdsAsync(allMemberGuids);

            // 5. Map thông tin User vào từng phòng
            foreach (var room in responseList)
            {
                room.Members = room.MemberIds.Select(idStr =>
                {
                    // Tìm user trong danh sách đã tải về. 
                    // So sánh Guid (từ SQL) với String (từ Mongo)
                    var user = usersInfo.FirstOrDefault(u => u.Id.ToString().Equals(idStr, StringComparison.OrdinalIgnoreCase));

                    return new ChatMemberDetail
                    {
                        Id = idStr,
                        FullName = user?.FullName ?? "Unknown User", // Fallback nếu không tìm thấy
                        AvatarUrl = user?.AvatarUrl
                    };
                }).ToList();
            }

            return responseList;
        }

        public async Task<ChatMessageResponse> SendMessageAsync(SendMessageRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            // Validate Room Exists
            var room = await _repository.GetRoomByIdAsync(request.RoomId);
            if (room == null)
            {
                throw new KeyNotFoundException($"Room with Id {request.RoomId} not found.");
            }

            // Normalize sender ID to uppercase for comparison
            var normalizedSenderId = string.IsNullOrWhiteSpace(request.SenderId) 
                ? request.SenderId 
                : request.SenderId.ToUpperInvariant();
            
            // Validate User is in Room (case-insensitive comparison)
            if (!room.MemberIds.Any(id => id.Equals(normalizedSenderId, StringComparison.OrdinalIgnoreCase)))
            {
                throw new UnauthorizedAccessException("User is not a member of this room.");
            }
            
            // Update sender ID to normalized version
            request.SenderId = normalizedSenderId;

            // Validate Assignment Link Type
            if (request.Type == "assignment_link")
            {
                if (request.Metadata == null ||
                    !request.Metadata.ContainsKey("assignmentId") ||
                    !request.Metadata.ContainsKey("redirectUrl"))
                {
                    throw new ArgumentException("Assignment Link messages must include 'assignmentId' and 'redirectUrl' in metadata.");
                }
            }

            var messageEntity = _mapper.Map<COM_ChatMessage>(request);
            messageEntity.CreatedAt = DateTime.UtcNow;

            try
            {
                var createdMessage = await _repository.CreateMessageAsync(messageEntity);
                var response = _mapper.Map<ChatMessageResponse>(createdMessage);

                // Publish to Redis for Realtime
                await _eventPublisher.PublishMessageAsync(response);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating chat message for Room {RoomId}", request.RoomId);
                throw;
            }
        }

        public async Task<List<ChatMessageResponse>> GetMessagesAsync(string roomId, int limit = 50, int skip = 0)
        {
            if (string.IsNullOrWhiteSpace(roomId)) return new List<ChatMessageResponse>();

            var messages = await _repository.GetMessagesByRoomIdAsync(roomId, limit, skip);

            // Đảo ngược lại danh sách để trả về client hiển thị đúng thứ tự (từ cũ đến mới) nếu cần
            // Hoặc để nguyên nếu client tự sort. Ở đây trả về đúng thứ tự DB trả ra (Mới nhất -> Cũ nhất)
            return _mapper.Map<List<ChatMessageResponse>>(messages);
        }
    }
}
