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
            .Include(c => c.Invoices)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (invoice == null) return NotFound();

        return View(invoice);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind(
            "InvoiceNumber,WorkOrderId,TaxAmount,SubTotal,Total,AmountDue,AmountPaid" +
            "AmountReturned,PaymentType,Status,DateIssued,DatePaid")]
        Invoice invoice)
    {
        _context.Add(invoice);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
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
    public async Task<IActionResult> Edit(int id,
        [Bind("Id,InvoiceNumber,WorkOrderId,TaxAmount,SubTotal,Total,AmountDue,AmountPaid\" +\n\"" +
              "AmountReturned,PaymentType,Status,DateIssued,DatePaid")]
        Invoice invoice)
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
                if (invoice == null) return NotFound();
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