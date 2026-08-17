using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using wbs_api.Repositories.Interfaces;

namespace wbs_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkstationController : ControllerBase
    {
        private readonly IWorkstationRepository _workstationRepository;

        public WorkstationController(IWorkstationRepository workstationRepository)
        {
            _workstationRepository = workstationRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var workstations = await _workstationRepository.GetAllAsync();
            return Ok(workstations);
        }

        [HttpGet("search")]
        public async Task<IActionResult> GetByName([FromQuery] string name)
        {
            var workstations = await _workstationRepository.GetByName(name);
            return Ok(workstations);
        }

        [HttpGet("status")]
        public async Task<IActionResult> GetStatusesByDate([FromQuery] DateTime? date, string name)
        {
            // Default to today's date if no query parameter is supplied
            var targetDate = date?.Date ?? DateTime.Today;

            var result = await _workstationRepository.GetWorkstationStatusesByDateAsync(targetDate, name);
            return Ok(result);
        }

    }
}
