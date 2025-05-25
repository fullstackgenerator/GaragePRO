using GaragePRO.Data;
using GaragePRO.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GaragePRO.Controllers;

public class VehicleController : Controller
{
    private readonly ApplicationDbContext _context;

    public VehicleController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Create(int customerId)
    {
        var vehicle = new Vehicle { customerId = customerId };
        return View(vehicle);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,make,model,year,VIN,mileage,customerId")] Vehicle vehicle)
    {
        if (ModelState.IsValid)
        {
            _context.Add(vehicle);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Customer", new { id = vehicle.customerId });
        }
        return View(vehicle);
    }

    public async Task<IActionResult> Edit(int id)
    {
        if (id == null) return BadRequest();
        var vehicle = await _context.Vehicles.FindAsync(id);
        if (vehicle == null) return NotFound();
        return View(vehicle);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,make,model,year,VIN,mileage,customerId")] Vehicle vehicle)
    {
        if (id != vehicle.Id) return BadRequest();
        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(vehicle);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (vehicle == null) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(vehicle);
    }
}