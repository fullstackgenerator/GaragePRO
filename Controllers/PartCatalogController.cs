using GaragePRO.Data;
using GaragePRO.Models;
using Microsoft.AspNetCore.Mvc;

namespace GaragePRO.Controllers;

public class PartCatalogController : Controller
{
    private readonly ApplicationDbContext _context;

    public PartCatalogController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PartCatalog partcatalog)
    {
        if (ModelState.IsValid)
        {
            _context.Add(partcatalog);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "WorkOrder");
        }

        return View(partcatalog);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var partcatalog = await _context.PartCatalogs.FindAsync(id);
        if (partcatalog == null) return NotFound();

        return View(partcatalog);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, PartCatalog partcatalog)
    {
        if (id != partcatalog.Id) return NotFound();

        if (ModelState.IsValid)
        {
            _context.Update(partcatalog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(partcatalog);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var partcatalog = await _context.PartCatalogs.FindAsync(id);
        if (partcatalog == null) return NotFound();
        _context.PartCatalogs.Remove(partcatalog);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}