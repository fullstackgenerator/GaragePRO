using GaragePRO.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GaragePRO.Models;

namespace GaragePRO.Controllers;

public class InvoiceController : Controller
{
    private readonly ApplicationDbContext _context;

    public InvoiceController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var invoice = await _context.Invoices.ToListAsync();
        return View(invoice);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return BadRequest();
        var invoice = await _context.Invoices
                .Include(i => i.WorkOrder)
                .ThenInclude(w => w.Vehicle)
                .ThenInclude(v => v.Customer)
                .Include(i => i.WorkOrder)
                .ThenInclude(w => w.ServiceDetails)
                .Include(i => i.WorkOrder)
                .ThenInclude(w => w.PartsUsed)
                .ThenInclude(p => p.PartCatalog)
                .FirstOrDefaultAsync(i => i.Id == id);

        if (invoice == null) return NotFound();

        return View(invoice);
    }

    public IActionResult Create()
    {
        ViewBag.WorkOrders = _context.WorkOrders
            .Include(w => w.Vehicle)
            .ThenInclude(v => v.Customer)
            .Select(w => new
            {
                w.Id,
                Label = $"#{w.Id} - {w.Vehicle.Make} {w.Vehicle.Model} ({w.Vehicle.Customer.FullName})"
            }).ToList();

        return View();
    }


[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create([Bind("InvoiceNumber,WorkOrderId,TaxAmount,SubTotal,Total,AmountDue,AmountPaid,PaymentType,Status,DateIssued,DatePaid")] Invoice invoice)
{
    if (ModelState.IsValid)
    {
        _context.Add(invoice);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    return View(invoice);
}


    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return BadRequest();
        var invoice = await _context.Invoices.FindAsync(id);
        if (invoice == null) return NotFound();
        return View(invoice);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,InvoiceNumber,WorkOrderId,TaxAmount,SubTotal,Total,AmountDue,AmountPaid,PaymentType,Status,DateIssued,DatePaid")] Invoice invoice)
    {
        if (id != invoice.Id) return BadRequest();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(invoice);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Invoices.Any(e => e.Id == id)) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(invoice);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var invoice = await _context.Invoices.FindAsync(id);
        if (invoice == null) return NotFound();
        _context.Invoices.Remove(invoice);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}