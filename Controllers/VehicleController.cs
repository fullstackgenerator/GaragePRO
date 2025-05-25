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

    // CREATE GET
    public IActionResult Create(int customerId)
    {
        var customer = _context.Customers.Find(customerId);
        if (customer == null)
        {
            return NotFound();
        }

        var vehicle = new Vehicle { CustomerId = customerId };
        ViewBag.CustomerId = customerId;
        return View(vehicle);
    }

    // CREATE POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Make,Model,Year,VIN,Mileage,CustomerId")] Vehicle vehicle)
    {
        ViewBag.CustomerId = vehicle.CustomerId;

        if (ModelState.IsValid)
        {
            if (!await _context.Customers.AnyAsync(c => c.Id == vehicle.CustomerId))
            {
                ModelState.AddModelError("", "The associated customer was not found.");
                return View(vehicle);
            }

            try
            {
                _context.Vehicles.Add(vehicle);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Customer", new { id = vehicle.CustomerId });
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("UNIQUE constraint failed: Vehicles.VIN") == true)
                {
                    ModelState.AddModelError("VIN", "A vehicle with this VIN already exists.");
                }
                else
                {
                    ModelState.AddModelError("", "An error occurred while saving the vehicle.");
                }
            }
        }

        return View(vehicle);
    }

    // EDIT GET
    public async Task<IActionResult> Edit(int id)
    {
        var vehicle = await _context.Vehicles.FindAsync(id);
        if (vehicle == null) return NotFound();
        return View(vehicle);
    }

    // EDIT POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Make,Model,Year,VIN,Mileage,CustomerId")] Vehicle vehicle)
    {
        if (id != vehicle.Id) return BadRequest();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(vehicle);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Customer", new { id = vehicle.CustomerId });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehicleExists(vehicle.Id)) return NotFound();
                throw;
            }
        }

        return View(vehicle);
    }

    private bool VehicleExists(int id)
    {
        return _context.Vehicles.Any(v => v.Id == id);
    }
}
