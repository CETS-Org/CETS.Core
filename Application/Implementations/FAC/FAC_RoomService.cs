using Application.Interfaces.FAC;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.ACAD;
using Domain.Interfaces.CORE;
using Domain.Interfaces.FAC;
using DTOs.FAC.FAC_Room.Requests;
using DTOs.FAC.FAC_Room.Responses;

namespace Application.Implementations.FAC
{
	public class FAC_RoomService : BaseService<FAC_Room, RoomResponse, UpdateRoomRequest, CreateRoomRequest>, IFAC_RoomService
	{
		private readonly IFAC_RoomRepository _roomRepository;
        private readonly IACAD_ClassMeetingRepository _classMeetingRepository;
        private readonly ICORE_LookUpRepository _lookUpRepository;
        private readonly IACAD_CourseTeacherAssignmentRepository _courseTeacherAssignmentRepo;
        private readonly IACAD_SyllabusItemRepository _syllabusItemRepository;


        public FAC_RoomService(
            IFAC_RoomRepository repository, 
            IACAD_ClassMeetingRepository classMeetingRepository, 
            IACAD_CourseTeacherAssignmentRepository courseTeacherAssignmentRepository,
            IACAD_SyllabusItemRepository syllabusItemRepository,
            ICORE_LookUpRepository lookUpRepository, 
            IUnitOfWork unitOfWork, 
            IMapper mapper)
			: base(repository, unitOfWork, mapper)
		{
			_roomRepository = repository;
            _classMeetingRepository = classMeetingRepository;
            _courseTeacherAssignmentRepo = courseTeacherAssignmentRepository;
            _syllabusItemRepository = syllabusItemRepository;
            _lookUpRepository = lookUpRepository;
        }

		public async Task<IReadOnlyList<RoomResponse>> GetByTypeAsync(Guid roomTypeId)
		{
			var items = await _roomRepository.GetAllAsync();
			var filtered = items
				.Where(r => r.RoomTypeId == roomTypeId)
				.ToList();
			return _mapper.Map<IReadOnlyList<RoomResponse>>(filtered);
		}

        public async Task<RoomResponse> UpdateRoomStatusAsync(Guid id, Guid statusId)
        {
            var room = await _repository.GetByIdAsync(id);
            if (room == null)
                throw new Exception("Room not found");

            room.RoomStatusId = statusId;
            room.UpdatedAt = DateTime.Now;

            _repository.Update(room);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<RoomResponse>(room);
        }

        public async Task<SlotAvailabilityDto> CheckSlotAvailabilityAsync(Guid roomId, DateTime date, int slotNumber)
        {
            var room = await _repository.GetByIdAsync(roomId);

            if (room == null)
                return new SlotAvailabilityDto { Available = false, Reason = "Room not found" };

            // Check room status
            if (room.RoomStatus.Code != "Available")
                return new SlotAvailabilityDto
                {
                    Available = false,
                    Reason = "Room is not available for booking"
                };

            var dateOnly = DateOnly.FromDateTime(date);

            // find slot lookup based on "Slot 1", "Slot 2"...
            var slotLookup = await _lookUpRepository.FindFirstAsync(x =>
                x.Code == $"Slot {slotNumber}");

            if (slotLookup == null)
                return new SlotAvailabilityDto { Available = false, Reason = "Invalid slot number" };

            // Check conflict
            var conflict = await _classMeetingRepository.FindFirstAsync(x =>
                x.RoomID == roomId &&
                x.Date == dateOnly &&
                x.SlotID == slotLookup.Id);

            if (conflict != null)
            {
                return new SlotAvailabilityDto
                {
                    Available = false,
                    Reason = "Slot already booked",
                    ConflictBookingId = conflict.Id,
                    ConflictClassName = conflict.Class.ClassName
                };
            }

            return new SlotAvailabilityDto
            {
                Available = true
            };
        }

        public async Task<IEnumerable<RoomResponse>> GetAvailableRoomsForSlotAsync(DateTime date, Guid slotId)
        {
            var dateOnly = DateOnly.FromDateTime(date);
            
            // Get all active rooms with "Available" status
            var allRooms = await _repository.GetAllAsync();
            var availableStatusRooms = allRooms
                .Where(r => r.IsActive && r.RoomStatus?.Code == "Available")
                .ToList();

            // Get all class meetings for the specified date and slot
            var bookedMeetings = await _classMeetingRepository.FindAsync(x =>
                x.Date == dateOnly &&
                x.SlotID == slotId &&
                !x.IsDeleted);

            // Get the list of booked room IDs
            var bookedRoomIds = bookedMeetings.Select(m => m.RoomID).ToHashSet();

            // Filter out rooms that are already booked
            var availableRooms = availableStatusRooms
                .Where(r => !bookedRoomIds.Contains(r.Id))
                .ToList();

            return _mapper.Map<IEnumerable<RoomResponse>>(availableRooms);
        }


        public async Task<IEnumerable<RoomWeeklyScheduleDto>> GetWeeklyScheduleAsync(DateTime weekStart, DateTime weekEnd)
        {
            var rooms = await _repository.GetAllAsync();

            var start = DateOnly.FromDateTime(weekStart);
            var end = DateOnly.FromDateTime(weekEnd);

            // gọi repository đã include đầy đủ
            var meetings = await _roomRepository.GetMeetingsWithNavigationAsync(start, end);

            var result = new List<RoomWeeklyScheduleDto>();

            foreach (var room in rooms)
            {
                var dto = new RoomWeeklyScheduleDto
                {
                    RoomId = room.Id,
                    RoomCode = room.RoomCode,
                    RoomStatus = room.RoomStatus?.Code ?? "Unknown",
                    RoomTypeName = room.RoomType?.Name ?? "Unknown"
                };

                // Init days × slots
                foreach (var dayIndex in Enumerable.Range(0, 6))
                {
                    var date = start.AddDays(dayIndex);
                    var dayName = date.ToString("dddd");

                    dto.Days[dayName] = Enumerable.Range(1, 5)
                        .Select(slot => new SlotScheduleDto
                        {
                            SlotNumber = slot,
                            IsBooked = false
                        }).ToList();
                }

                var roomMeetings = meetings.Where(x => x.RoomID == room.Id);

                foreach (var m in roomMeetings)
                {
                    var dayName = m.Date.ToString("dddd");
                    if (!dto.Days.ContainsKey(dayName))
                        continue;

                    int slotNumber = 0;
                    if (m.Slot?.Code != null && m.Slot.Code.StartsWith("Slot"))
                        int.TryParse(m.Slot.Code.Replace("Slot", ""), out slotNumber);

                    if (slotNumber == 0)
                        continue;

                    var slot = dto.Days[dayName].FirstOrDefault(s => s.SlotNumber == slotNumber);
                    if (slot == null)
                        continue;

                    slot.IsBooked = true;
                    slot.BookingId = m.Id;
                    slot.ClassName = m.Class?.ClassName ?? "N/A";
                    slot.CourseName = m.TeacherAssignment?.Course?.CourseName ?? "N/A";
                    slot.TeacherName = m.TeacherAssignment?.Teacher?.Account?.FullName ?? "N/A";
                }

                result.Add(dto);
            }

            return result;
        }



        public async Task<RoomStatisticsResponse> GetStatisticsAsync()
        {
            var rooms = await _repository.GetAllAsync();

            // Total active rooms in system (not soft deleted)
            var total = rooms.Count(r => r.IsActive);

            // Active = Available + In Use + Reserved
            var active = rooms.Count(r =>
                r.IsActive &&
                (r.RoomStatus.Code == "Available" ||
                 r.RoomStatus.Code == "In Use" ||
                 r.RoomStatus.Code == "Reserved")
            );

            var maintenance = rooms.Count(r =>
                r.IsActive &&
                r.RoomStatus.Code == "Maintenance"
            );

            var unavailable = rooms.Count(r =>
                r.IsActive &&
                r.RoomStatus.Code == "Unavailable"
            );

            return new RoomStatisticsResponse
            {
                TotalRooms = total,
                ActiveRooms = active,
                MaintenanceRooms = maintenance,
                UnavailableRooms = unavailable
            };
        }
        public async Task<List<RoomTypeResponse>> GetRoomTypesAsync()
        {
            var types = await _roomRepository.GetRoomTypesAsync();

            return types.Select(t => new RoomTypeResponse
            {
                Id = t.Id,
                Code = t.Code,
                Name = t.Name
            }).ToList();
        }

        public async Task<List<RoomStatusResponse>> GetRoomStatusesAsync()
        {
            var statuses = await _roomRepository.GetRoomStatusesAsync();

            return statuses.Select(s => new RoomStatusResponse
            {
                Id = s.Id,
                Code = s.Code,
                Name = s.Name
            }).ToList();
        }

        public async Task<RoomSlotInfoResponse> GetSlotInfoAsync(Guid roomId, DateOnly date, int slotNumber)
        {
            var room = await _roomRepository.GetByIdAsync(roomId);
            var slotLookup = await _lookUpRepository.FindFirstAsync(x =>
                x.LookUpType.Code == "TimeSlot" && x.Code == $"Slot{slotNumber}");

            var meeting = await _classMeetingRepository.GetMeetingDetailAsync(roomId, date, slotLookup.Id);

            return new RoomSlotInfoResponse
            {
                Room = new RoomSlotInfoResponse.RoomInfo
                {
                    RoomId = room.Id,
                    RoomCode = room.RoomCode,
                    RoomType = room.RoomType?.Name ?? "Unknown",
                    Status = room.RoomStatus?.Name ?? "Unknown",
                    Capacity = room.Capacity
                },
                Slot = new RoomSlotInfoResponse.SlotInfo
                {
                    SlotNumber = slotNumber,
                    Start = slotLookup.Name,
                    End = TimeSpan.Parse(slotLookup.Name).Add(TimeSpan.FromMinutes(90)).ToString(@"hh\:mm"),
                    Date = date,
                    DayOfWeek = date.ToString("dddd")
                },
                IsBooked = meeting != null,
                CurrentClass = meeting == null ? null : new RoomSlotInfoResponse.ClassInfo
                {
                    MeetingId = meeting.Id,
                    ClassName = meeting.Class?.ClassName ?? "N/A",
                    CourseName = meeting.TeacherAssignment?.Course?.CourseName ?? "N/A",
                    TeacherName = meeting.TeacherAssignment?.Teacher?.Account?.FullName ?? "N/A"
                }
            };
        }

        public async Task<Guid> BookSlotAsync(BookRoomSlotRequest request)
        {
            // Slot lookup
            var slotLookup = await _lookUpRepository.FindFirstAsync(x =>
                x.LookUpType.Code == "TimeSlot" &&
                x.Code == $"Slot{request.SlotNumber}");

            if (slotLookup == null)
                throw new Exception("Invalid slot");

            // Check conflict
            var conflict = await _classMeetingRepository.FindFirstAsync(x =>
                x.RoomID == request.RoomId &&
                x.Date == request.Date &&
                x.SlotID == slotLookup.Id);

            if (conflict != null)
                throw new Exception("Slot already booked");

            // Find teacher assignment
            var teacherAssignment = await _courseTeacherAssignmentRepo.FindFirstAsync(
                x => x.CourseID == request.CourseId && x.TeacherID == request.TeacherId);

            if (teacherAssignment == null)
                throw new Exception("Teacher is not assigned to this course");

            // Get default CoveredTopic
            var firstTopic = await _syllabusItemRepository.FindFirstAsync(
                x => x.Syllabus.CourseID == request.CourseId && !x.IsDeleted);

            if (firstTopic == null)
                throw new Exception("No syllabus items found for this course.");

            var meeting = new ACAD_ClassMeeting
            {
                Id = Guid.NewGuid(),
                RoomID = request.RoomId,
                ClassID = request.ClassId,
                TeacherAssignmentID = teacherAssignment.Id,
                SlotID = slotLookup.Id,
                Date = request.Date,
                CoveredTopicID = firstTopic.Id,   // FIX HERE
                CreatedAt = DateTime.Now,
                IsActive = true,
                IsDeleted = false
            };

            _classMeetingRepository.Add(meeting);

            var inUseStatus = await _lookUpRepository.FindFirstAsync(x =>
                x.LookUpType.Code == "RoomStatus" && x.Code == "In Use");

            if (inUseStatus != null)
            {
                var room = await _repository.GetByIdAsync(request.RoomId);
                room.RoomStatusId = inUseStatus.Id;
                room.UpdatedAt = DateTime.Now;
                _repository.Update(room);
            }

            await _unitOfWork.SaveChangesAsync();

            return meeting.Id;
        }


        public async Task CancelSlotBookingAsync(Guid meetingId)
        {
            var meeting = await _classMeetingRepository.GetByIdAsync(meetingId)
                          ?? throw new Exception("Booking not found");

            _classMeetingRepository.Remove(meeting);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<RoomResponse> PatchAsync(Guid id, UpdateRoomRequest request)
        {
            var room = await _repository.GetByIdAsync(id);
            if (room == null)
                throw new Exception("Room not found");

            // Update từng field nếu có gửi lên
            if (request.RoomCode != null)
                room.RoomCode = request.RoomCode;

            if (request.Capacity.HasValue)
                room.Capacity = request.Capacity.Value;

            if (request.RoomTypeId.HasValue)
                room.RoomTypeId = request.RoomTypeId.Value;

            if (request.RoomStatusId.HasValue)
                room.RoomStatusId = request.RoomStatusId.Value;

            if (request.OnlineMeetingUrl != null)
                room.OnlineMeetingUrl = request.OnlineMeetingUrl;

            if (request.IsActive.HasValue)
                room.IsActive = request.IsActive.Value;

             _repository.Update(room);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<RoomResponse>(room);
        }


        public async Task<IReadOnlyList<RoomOptionDto>> GetAvailableRoomsAsync(GetAvailableRoomsRequest request)
        {
            if (request.EndDate < request.StartDate)
                throw new ArgumentException("EndDate must be greater than or equal to StartDate.");

            // Không có schedule => trả về tất cả phòng active
            if (request.Schedules == null || request.Schedules.Count == 0)
            {
                var allActiveRooms = await _roomRepository.GetActiveRoomsAsync();
                return allActiveRooms
                    .Select(r => new RoomOptionDto
                    {
                        Id = r.Id,
                        RoomCode = r.RoomCode,
                        Capacity = r.Capacity,
                        IsActive = r.IsActive
                    })
                    .OrderBy(r => r.RoomCode)
                    .ToList();
            }

            // 1. Map DayOfWeek -> list TimeSlotID (Guid)
            var scheduleLookup = request.Schedules
                .GroupBy(s => s.DayOfWeek)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(x => x.TimeSlotID).Distinct().ToList()
                );

            // 2. Generate (Date, TimeSlotID) cho tất cả buổi học
            var requestedPairs = new List<(DateOnly Date, Guid TimeSlotID)>();
            var cursor = request.StartDate;

            while (cursor <= request.EndDate)
            {
                var dow = cursor.DayOfWeek;

                if (scheduleLookup.TryGetValue(dow, out var slotList))
                {
                    foreach (var slotId in slotList)
                    {
                        requestedPairs.Add((cursor, slotId));
                    }
                }

                cursor = cursor.AddDays(1);
            }

            // Nếu không có cặp nào => return full room active
            if (requestedPairs.Count == 0)
            {
                var allActiveRooms = await _roomRepository.GetActiveRoomsAsync();
                return allActiveRooms
                    .Select(r => new RoomOptionDto
                    {
                        Id = r.Id,
                        RoomCode = r.RoomCode,
                        Capacity = r.Capacity,
                        IsActive = r.IsActive
                    })
                    .OrderBy(r => r.RoomCode)
                    .ToList();
            }

            // 3. SlotIDs cần quan tâm
            var requestedSlotIds = request.Schedules
                .Select(s => s.TimeSlotID)
                .Distinct()
                .ToList();

            // 4. Lấy các ClassMeeting trong range để check trùng
            var candidateMeetings = await _classMeetingRepository
                .GetMeetingsForScheduleOverlapAsync(
                    request.StartDate,
                    request.EndDate,
                    requestedSlotIds);

            // 5. HashSet "yyyy-MM-dd|slotGuid" cho lịch lớp cần tạo
            var requestedKeySet = requestedPairs
                .Select(p => $"{p.Date:yyyy-MM-dd}|{p.TimeSlotID}")
                .ToHashSet();

            // 6. Room nào có (Date, SlotID) trùng => bị chiếm
            var occupiedRoomIds = candidateMeetings
                .Where(m => m.RoomID.HasValue &&
                            requestedKeySet.Contains($"{m.Date:yyyy-MM-dd}|{m.SlotID}"))
                .Select(m => m.RoomID!.Value)
                .Distinct()
                .ToHashSet();

            // 7. Lấy phòng active chưa bị chiếm
            var allRooms = await _roomRepository.GetActiveRoomsAsync();
            var availableRooms = allRooms
                .Where(r => !occupiedRoomIds.Contains(r.Id))
                .OrderBy(r => r.RoomCode)
                .Select(r => new RoomOptionDto
                {
                    Id = r.Id,
                    RoomCode = r.RoomCode,
                    Capacity = r.Capacity,
                    IsActive = r.IsActive
                })
                .ToList();

            return availableRooms;
        }



    }
}



