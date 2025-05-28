using GaragePRO.Data;
using GaragePRO.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GaragePRO.Controllers;

public class PartUsedController : Controller
{
    private readonly ApplicationDbContext _context;

    public PartUsedController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Create(int workOrderId)
    {
        ViewBag.PartCatalogs = new SelectList(_context.PartCatalogs, "Id", "PartName");
        ViewBag.WorkOrderId = workOrderId;
        return View(new PartUsed { WorkOrderId = workOrderId });
    }

    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("WorkOrderId,PartCatalogId,Quantity,CreatedAt")]
        PartUsed partUsed)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.PartCatalogs = new SelectList(_context.PartCatalogs, "Id", "PartName", partUsed.PartCatalogId);
            return View(partUsed);
        }

        partUsed.CreatedAt = DateTime.Now;
        _context.PartUsed.Add(partUsed);
        await _context.SaveChangesAsync();
        return RedirectToAction("Details", "WorkOrder", new { id = partUsed.WorkOrderId });
    }


    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var partUsed = await _context.PartUsed.FindAsync(id);
        if (partUsed == null) return NotFound();

        ViewBag.PartCatalogs = new SelectList(_context.PartCatalogs, "Id", "PartName", partUsed.PartCatalogId);
        return View(partUsed);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id,
        [Bind("Id,WorkOrderId,PartCatalogId,Quantity,CreatedAt")]
        PartUsed partUsed)
    {
        ViewBag.PartCatalogs = new SelectList(_context.PartCatalogs, "Id", "PartName", partUsed.PartCatalogId);
        if (id != partUsed.Id) return BadRequest();
        if (ModelState.IsValid)
        {
            try
            {
                var existing = await _context.PartUsed.FindAsync(id);
                if (existing == null) return NotFound();
                
                existing.PartCatalogId = partUsed.PartCatalogId;
                existing.Quantity = partUsed.Quantity;
                await _context.SaveChangesAsync();
            }

            catch (DbUpdateConcurrencyException)
            {
                if (!_context.PartUsed.Any(e => e.Id == partUsed.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction("Details", "WorkOrder", new { id = partUsed.WorkOrderId });
        }

        return View(partUsed);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var partUsed = await _context.PartUsed.FindAsync(id);
        if (partUsed == null) return NotFound();
        _context.PartUsed.Remove(partUsed);
        await _context.SaveChangesAsync();
        return RedirectToAction("Details", "WorkOrder", new { id = partUsed.WorkOrderId });
    }
}