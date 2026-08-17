using Microsoft.AspNetCore.Mvc;
using wbs_department_api.Data;
using wbs_department_api.Models;
using wbs_department_api.Repositories.Interfaces;

namespace wbs_department_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentsController : ControllerBase
{
    private readonly IDepartmentRepository _departmentRepository;

    public DepartmentsController(
        IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    /// <summary>
    /// Get all departments
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var departments =
            await _departmentRepository.GetAllAsync();

        return Ok(departments);
    }

    /// <summary>
    /// Get department by id
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var department =
            await _departmentRepository.GetByIdAsync(id);

        if (department == null)
        {
            return NotFound(new
            {
                Message = $"Department with ID {id} not found."
            });
        }

        return Ok(department);
    }

    /// <summary>
    /// Create department
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] Department department)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        department.CreatedDate = DateTime.UtcNow;
        department.UpdatedDate = DateTime.UtcNow;

        var createdDepartment =
            await _departmentRepository.CreateAsync(department);

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdDepartment.Id },
            createdDepartment);
    }

    /// <summary>
    /// Update department
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] Department request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var department =
            await _departmentRepository.GetByIdAsync(id);

        if (department == null)
        {
            return NotFound(new
            {
                Message = $"Department with ID {id} not found."
            });
        }

        department.Name = request.Name;
        department.UpdatedDate = DateTime.UtcNow;

        await _departmentRepository.UpdateAsync(department);

        return Ok(new
        {
            Message = "Department updated successfully."
        });
    }

    /// <summary>
    /// Delete department
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var department =
            await _departmentRepository.GetByIdAsync(id);

        if (department == null)
        {
            return NotFound(new
            {
                Message = $"Department with ID {id} not found."
            });
        }

        await _departmentRepository.DeleteAsync(department);

        return Ok(new
        {
            Message = "Department deleted successfully."
        });
    }
}