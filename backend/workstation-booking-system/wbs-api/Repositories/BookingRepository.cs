using Microsoft.EntityFrameworkCore;
using wbs_api.Data;
using wbs_api.Models;
using wbs_api.Repositories.Interfaces;

namespace wbs_api.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _context;

        public BookingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Booking>> GetAllAsync()
        {
            return await _context.Bookings
                .OrderBy(x => x.BookingDate)
                .ToListAsync();
        }

        public async Task<Booking?> GetByIdAsync(int id)
        {
            return await _context.Bookings
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Booking> CreateAsync(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        public async Task UpdateAsync(Booking booking)
        {
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Booking booking)
        {
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
        }

        public async Task<Booking?> GetActiveBookingAsync(
            int workstationId,
            DateTime bookingDate)
        {
            // Extract the date part outside the query so EF Core can parametrize it cleanly
            DateTime targetDate = bookingDate.Date;

            return await _context.Bookings
                .FirstOrDefaultAsync(b =>
                    b.WorkstationId == workstationId &&
                    (
                        b.IsPermanent ||
                        (
                            b.BookingDate.HasValue &&
                            b.BookingDate == targetDate
                        )
                    ));
        }

        public async Task<Booking?> GetActiveBookingPerDateAsync(DateTime bookingDate, string userId)
        {
            // Extract the date part outside the query so EF Core can parametrize it cleanly
            DateTime targetDate = bookingDate.Date;

            return await _context.Bookings
                .FirstOrDefaultAsync(b =>
                    b.UserId == userId && b.BookingDate == targetDate);
        }
    }
}
