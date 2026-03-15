using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeVault.Constants;
using SafeVault.Data;
using SafeVault.Models;

namespace SafeVault.Controllers;

public class DepartmentController : ControllerBase
{
    private AppDbContext _context;

    public DepartmentController(AppDbContext context) : base()
    {
        this._context = context;
    }

    [HttpGet]
    [Authorize(policy: AuthConstants.ManagerPolicy)]
    [Authorize(policy: AuthConstants.AdminPolicy)]
    public IActionResult Index(int limit = 3, int offset = 0)
    {
        List<Department> departments = _context.Departments.Take(limit).Skip(offset).ToList();
        return Ok(departments);
    }

    [HttpGet]
    [Authorize(policy: AuthConstants.ManagerPolicy)]
    [Authorize(policy: AuthConstants.AdminPolicy)]
    public IActionResult Details(int id)
    {
        Department? department = _context.Departments.Find(task => task.Id == id);
        if (department == null)
        {
            return NotFound("Department not found");
        }

        return Ok(department);
    }

    [Authorize(policy: AuthConstants.AdminPolicy)]
    public IActionResult Create([FromBody] Department dto)
    {
        if (!ModelState.IsValid)
        {
            return UnprocessableEntity("Invalid department data");
        }

        this._context.Departments.Add(dto);
        this._context.SaveChanges();
        return Created();
    }

    [HttpPut]
    [Authorize(policy: AuthConstants.AdminPolicy)]
    public IActionResult Update(int id, Department dto)
    {
        Department? existingDepartment = _context.Departments.Find(d => d.Id == id);
        if (existingDepartment == null)
        {
            return NotFound($"Department [{id}] not found");
        }

        if (!ModelState.IsValid)
        {
            return UnprocessableEntity("Invalid department data");
        }

        existingDepartment.Name = dto.Name;
        this._context.SaveChanges();
        return Ok(existingDepartment);
    }

    [HttpDelete]
    [Authorize(policy: AuthConstants.AdminPolicy)]
    public IActionResult Delete(int id)
    {
        Department? department = _context.Departments.Find(d => d.Id == id);
        if (department == null)
        {
            return NotFound($"Department [{id}] not found");
        }

        this._context.Departments.Remove(department);
        this._context.SaveChanges();
        return NoContent();
    }
}