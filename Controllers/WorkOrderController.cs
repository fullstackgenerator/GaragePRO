using GaragePRO.Data;
using GaragePRO.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GaragePRO.Controllers;

public class WorkOrderController : Controller
{
    private readonly ApplicationDbContext _context;

    public WorkOrderController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string searchString, DateTime? fromDate, DateTime? toDate, bool showArchived = false) // Add showArchived parameter
    {
        var workOrdersQuery = _context.WorkOrders
            .Include(w => w.Mechanic)
            .Include(w => w.Vehicle)
            .ThenInclude(v => v.Customer)
            .AsQueryable();
        
        if (showArchived)
        {
            workOrdersQuery = workOrdersQuery.Where(w => w.Status == WorkOrderStatus.Archived);
        }
        else
        {
            //default: show work orders that are NOT Archived
            workOrdersQuery = workOrdersQuery.Where(w => w.Status != WorkOrderStatus.Archived);
        }

        if (!string.IsNullOrEmpty(searchString))
        {
            searchString = searchString.ToLower();

            workOrdersQuery = workOrdersQuery.Where(w =>
                w.Id.ToString().Contains(searchString) ||
                (w.Vehicle != null && w.Vehicle.Customer != null &&
                 w.Vehicle.Customer.FullName.ToLower().Contains(searchString)) ||
                (w.Vehicle != null && w.Vehicle.Make.ToLower().Contains(searchString)) ||
                (w.Vehicle != null && w.Vehicle.Model.ToLower().Contains(searchString)) ||
                (w.Vehicle != null &&
                 w.Vehicle.VIN.ToLower().Contains(searchString)) ||
                (w.Mechanic != null &&
                 w.Mechanic.FullName.ToLower().Contains(searchString))
            );
        }

        if (fromDate.HasValue)
        {
            workOrdersQuery = workOrdersQuery.Where(w => w.DateIn.Date >= fromDate.Value.Date);
        }

        if (toDate.HasValue)
        {
            workOrdersQuery = workOrdersQuery.Where(w => w.DateIn.Date <= toDate.Value.Date);
        }

        var workOrders = await workOrdersQuery.ToListAsync();

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
    public async Task<IActionResult> Create(
        [Bind("VehicleId,MechanicId,DateIn,DateOut,Notes,Status")]
        WorkOrder workOrder)
    {
        if (ModelState.IsValid)
        {
            workOrder.CreatedAt = DateTime.Now;
            _context.WorkOrders.Add(workOrder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Mechanics = new SelectList(_context.Mechanics, "Id", "FullName");
        ViewBag.Vehicles = new SelectList(_context.Vehicles, "Id", "VIN");

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
    public async Task<IActionResult> Edit(int id,
        [Bind("Id,VehicleId,MechanicId,DateIn,DateOut,Notes,Status,CreatedAt")]
        WorkOrder workOrder)
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

    public async Task<IActionResult> SearchMechanics(string term)
    {
        var results = await _context.Mechanics
            .Where(m => m.FullName.Contains(term) || m.AssignedVehicleBrand.Contains(term))
            .Select(m => new { id = m.Id, text = m.FullName + " (" + m.AssignedVehicleBrand + ")" })
            .ToListAsync();

        return Json(results);
    }

    public async Task<IActionResult> SearchVehicles(string term)
    {
        var results = await _context.Vehicles
            .Where(v => v.VIN.Contains(term) || v.Make.Contains(term) || v.Model.Contains(term))
            .Select(v => new { id = v.Id, text = v.VIN + " - " + v.Make + " " + v.Model })
            .ToListAsync();
        return Json(results);
    }
}