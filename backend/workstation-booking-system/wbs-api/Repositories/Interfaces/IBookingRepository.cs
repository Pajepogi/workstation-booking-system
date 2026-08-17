using wbs_api.Models;

namespace wbs_api.Repositories.Interfaces
{
    public interface IBookingRepository
    {
        Task<IEnumerable<Booking>> GetAllAsync();
        Task<Booking?> GetByIdAsync(int id);
        Task<Booking> CreateAsync(Booking booking);
        Task UpdateAsync(Booking booking);
        Task DeleteAsync(Booking booking);
        Task<Booking?> GetActiveBookingAsync(int workstationId, DateTime bookingDate);
        Task<Booking?> GetActiveBookingPerDateAsync(DateTime bookingDate, string userId);
    }
}
