using System.Data;
using Microsoft.EntityFrameworkCore;
using Dapper;
using wbs_api.Data;
using wbs_api.DTOs;
using wbs_api.Models;
using wbs_api.Repositories.Interfaces;

namespace wbs_api.Repositories
{
    public class WorkstationRepository : IWorkstationRepository
    {
        private readonly ApplicationDbContext _context;

        public WorkstationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<WorkstationStatusDto>> GetWorkstationStatusesByDateAsync(DateTime date, string name)
        {
            const string sql = @"
                SELECT
                    WS.id AS Id,
                    BK.id AS BookingId,
                    BK.user_id AS UserId,
                    WS.code AS Code,
                    WS.wing AS Wing,
                    WS.x_position AS XPosition,
                    WS.y_position AS YPosition,
                    WS.width AS Width,
                    WS.height AS Height,
                    BK.user_name AS UserName,
                    CASE
                        WHEN BK.is_permanent = TRUE THEN 'Booked'
                        WHEN BK.id IS NOT NULL THEN 'Reserved'
                        ELSE 'Available'
                    END AS Status
                FROM Workstations WS
                LEFT JOIN Booking BK
                    ON WS.id = BK.workstation_id
                    AND (
                        BK.booking_date = @BookingDate
                        OR BK.is_permanent = TRUE
                    )
                WHERE WS.wing LIKE @Name;";

            // Obtain the underlying DbConnection from DbContext
            var connection = _context.Database.GetDbConnection();

            return await connection.QueryAsync<WorkstationStatusDto>(
                sql,
                new { BookingDate = date.Date, Name = $"%{name}%" }
            );
        }

        public Task<Workstation> CreateAsync(Workstation workstation)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Workstation workstation)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Workstation>> GetAllAsync()
        {
            return await _context.Workstations.ToListAsync();
        }

        public Task<Workstation?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Workstation>> GetByName(string name)
        {
            return await _context.Workstations.Where(w => w.Wing.Contains(name)).ToListAsync();
        }

        public Task UpdateAsync(Workstation workstation)
        {
            throw new NotImplementedException();
        }
    }
}