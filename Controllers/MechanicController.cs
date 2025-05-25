using GaragePRO.Data;
using GaragePRO.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GaragePRO.Controllers;

public class MechanicController : Controller
{
    private readonly ApplicationDbContext _context;

    public MechanicController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var mechanic = await _context.Mechanics.ToListAsync();
        return View(mechanic);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return BadRequest();
        var mechanic = await _context.Mechanics
            .Include(m => m.WorkOrders)
            .ThenInclude(wo => wo.Vehicle)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (mechanic == null) return NotFound();
        return View(mechanic);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [Bind("FullName,AssignedVehicleBrand,Seniority,EmploymentStartYear,Status,Phone")]
        Mechanic mechanic)
    {
        if (!ModelState.IsValid) return View(mechanic);

        _context.Mechanics.Add(mechanic);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

[HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return BadRequest();
        var mechanic = await _context.Mechanics.FindAsync(id);
        if (mechanic == null) return NotFound();
        return View(mechanic);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id,
        [Bind("Id,FullName,AssignedVehicleBrand,Seniority,EmploymentStartYear,Status,Phone")]
        Mechanic mechanic)
    {
        if (id != mechanic.Id) return BadRequest();
        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(mechanic);
                await _context.SaveChangesAsync();
            }

            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Mechanics.Any(e => e.Id == mechanic.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(mechanic);
    }
}