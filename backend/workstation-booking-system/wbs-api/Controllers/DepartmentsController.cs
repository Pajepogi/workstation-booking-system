using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using wbs_api.Models;
using wbs_api.Repositories.Interfaces;

namespace wbs_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentsController : ControllerBase
{
    private readonly IDepartmentRepository _departmentRepository;

    public DepartmentsController(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var departments = await _departmentRepository.GetAllAsync();
        return Ok(departments);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var department = await _departmentRepository.GetByIdAsync(id);

        if (department == null)
        {
            return NotFound(new { Message = $"Department with ID {id} not found." });
        }

        return Ok(department);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Department department)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        department.CreatedDate = DateTime.UtcNow;
        department.UpdatedDate = DateTime.UtcNow;

        var createdDepartment = await _departmentRepository.CreateAsync(department);

        return CreatedAtAction(nameof(GetById), new { id = createdDepartment.Id }, createdDepartment);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Department request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var department = await _departmentRepository.GetByIdAsync(id);

        if (department == null)
        {
            return NotFound(new { Message = $"Department with ID {id} not found." });
        }

        department.Name = request.Name;
        department.UpdatedDate = DateTime.UtcNow;

        await _departmentRepository.UpdateAsync(department);

        return Ok(new { Message = "Department updated successfully." });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var department = await _departmentRepository.GetByIdAsync(id);

        if (department == null)
        {
            return NotFound(new { Message = $"Department with ID {id} not found." });
        }

        await _departmentRepository.DeleteAsync(department);

        return Ok(new { Message = "Department deleted successfully." });
    }
}
