using GaragePRO.Data;
using GaragePRO.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace GaragePRO.Controllers;

public class WorkOrderController : Controller
{
    private readonly ApplicationDbContext _context;

    public WorkOrderController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var workOrders = await _context.WorkOrders
            .Include(w => w.Mechanic)
            .Include(w => w.Vehicle)
            .ToListAsync();

        return View(workOrders);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return BadRequest();

        var workOrder = await _context.WorkOrders
            .Include(w => w.Mechanic)
            .Include(w => w.Vehicle)
            .Include(w => w.ServiceDetails)
            .Include(w => w.PartsUsed)
                .ThenInclude(p => p.PartCatalog)
            .FirstOrDefaultAsync(w => w.Id == id);

        if (workOrder == null) return NotFound();

        return View(workOrder);
    }

    public IActionResult Create()
    {
        ViewBag.Mechanics = _context.Mechanics.ToList();
        ViewBag.Vehicles = _context.Vehicles.ToList();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(WorkOrder workOrder)
    {
        if (ModelState.IsValid)
        {
            workOrder.CreatedAt = DateTime.Now;
            _context.WorkOrders.Add(workOrder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Mechanics = _context.Mechanics.ToList();
        ViewBag.Vehicles = _context.Vehicles.ToList();
        return View(workOrder);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var workOrder = await _context.WorkOrders.FindAsync(id);
        if (workOrder == null) return NotFound();

        ViewBag.Mechanics = _context.Mechanics.ToList();
        ViewBag.Vehicles = _context.Vehicles.ToList();
        return View(workOrder);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, WorkOrder workOrder)
    {
        if (id != workOrder.Id) return BadRequest();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(workOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.WorkOrders.Any(e => e.Id == workOrder.Id)) return NotFound();
                throw;
            }
        }

        ViewBag.Mechanics = _context.Mechanics.ToList();
        ViewBag.Vehicles = _context.Vehicles.ToList();
        return View(workOrder);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var workOrder = await _context.WorkOrders.FindAsync(id);
        if (workOrder == null) return NotFound();

        _context.WorkOrders.Remove(workOrder);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
