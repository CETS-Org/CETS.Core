using Application.Interfaces.COM;
using AutoMapper;
using Domain.Entities.MongoDB;
using Domain.Interfaces.COM;
using DTOs.COM.COM_Notification.Requests;
using DTOs.COM.COM_Notification.Responses;

namespace Application.Implementations.COM;

public class COM_NotificationService : ICOM_NotificationService
{
    private readonly ICOM_NotificationRepository _repository;
    private readonly IMapper _mapper;
    private readonly INotificationEventPublisher _eventPublisher;

    public COM_NotificationService(ICOM_NotificationRepository repository, IMapper mapper, INotificationEventPublisher eventPublisher)
    {
        _repository = repository;
        _mapper = mapper;
        _eventPublisher = eventPublisher;
    }

    public async Task<IReadOnlyList<NotificationResponse>> GetAllAsync()
    {
        var documents = await _repository.GetAllAsync();
        return _mapper.Map<List<NotificationResponse>>(documents);
    }

    public async Task<IReadOnlyList<NotificationResponse>> GetByUserAsync(string userId)
    {
        var documents = await _repository.GetByUserAsync(userId);
        return _mapper.Map<List<NotificationResponse>>(documents);
    }

    public async Task<NotificationResponse?> GetByIdAsync(string id)
    {
        var document = await _repository.GetByIdAsync(id);
        return document == null ? null : _mapper.Map<NotificationResponse>(document);
    }

    public async Task<NotificationResponse> CreateAsync(CreateNotificationRequest request)
    {
        var document = _mapper.Map<COM_Notification>(request);

        var created = await _repository.CreateAsync(document);
        var response = _mapper.Map<NotificationResponse>(created);
        await _eventPublisher.PublishNotificationAsync(response);
        return response;
    }

    public async Task<NotificationResponse?> UpdateAsync(string id, UpdateNotificationRequest request)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null)
        {
            return null;
        }

        _mapper.Map(request, existing);

        var updated = await _repository.UpdateAsync(existing);
        return updated ? _mapper.Map<NotificationResponse>(existing) : null;
    }

    public Task DeleteAsync(string id) => _repository.DeleteAsync(id);

    public async Task<NotificationResponse?> MarkAsReadAsync(string id)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null)
        {
            return null;
        }

        if (!existing.IsRead)
        {
            existing.IsRead = true;
            var updated = await _repository.UpdateAsync(existing);
            if (!updated)
            {
                return null;
            }
        }

        return _mapper.Map<NotificationResponse>(existing);
    }

    public async Task<int> MarkAllAsReadAsync(string userId)
    {
        var notifications = await _repository.GetByUserAsync(userId);
        if (notifications == null || notifications.Count == 0)
        {
            return 0;
        }

        var updatedCount = 0;
        foreach (var notification in notifications)
        {
            if (!notification.IsRead)
            {
                notification.IsRead = true;
                var updated = await _repository.UpdateAsync(notification);
                if (updated)
                {
                    updatedCount++;
                }
            }
        }

        return updatedCount;
    }

    public async Task<IReadOnlyList<NotificationResponse>> CreateManyAsync(IEnumerable<CreateNotificationRequest> requests)
    {
        var responses = new List<NotificationResponse>();

        foreach (var request in requests)
        {
            var document = _mapper.Map<COM_Notification>(request);
            var created = await _repository.CreateAsync(document);
            responses.Add(_mapper.Map<NotificationResponse>(created));
        }

        await _eventPublisher.PublishNotificationsAsync(responses);
        return responses;
    }
}
