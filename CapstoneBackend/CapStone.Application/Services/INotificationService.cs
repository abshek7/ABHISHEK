using CapStone.Application.DTOs.Customer;

namespace CapStone.Application.Services
{
    public interface INotificationService
    {
        Task<IReadOnlyList<NotificationDto>> GetNotificationsAsync(Guid customerId, CancellationToken cancellationToken = default);
    }
}
