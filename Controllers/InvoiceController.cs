using GaragePRO.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GaragePRO.Models;
using GaragePRO.PdfDocuments;
using QuestPDF.Fluent;

namespace GaragePRO.Controllers;

public class InvoiceController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public InvoiceController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<IActionResult> Index(string searchString, DateTime? fromDate, DateTime? toDate, bool showArchived = false)
    {
        var invoicesQuery = _context.Invoices
            .Include(i => i.WorkOrder)
            .ThenInclude(w => w.Vehicle)
            .ThenInclude(v => v.Customer)
            .AsQueryable();

        if (showArchived)
        {
            invoicesQuery = invoicesQuery.Where(i => i.Status == InvoiceStatus.Archived);
        }
        else
        {
            invoicesQuery = invoicesQuery.Where(i => i.Status != InvoiceStatus.Archived);
        }

        if (!string.IsNullOrEmpty(searchString))
        {
            searchString = searchString.ToLower();

            invoicesQuery = invoicesQuery.Where(i =>
                i.WorkOrderId.ToString().Contains(searchString) ||
                (i.WorkOrder != null && i.WorkOrder.Vehicle != null && i.WorkOrder.Vehicle.Customer != null &&
                 i.WorkOrder.Vehicle.Customer.FullName.ToLower().Contains(searchString)) ||
                (i.WorkOrder != null && i.WorkOrder.Vehicle != null &&
                 i.WorkOrder.Vehicle.Make.ToLower().Contains(searchString)) ||
                (i.WorkOrder != null && i.WorkOrder.Vehicle != null &&
                 i.WorkOrder.Vehicle.Model.ToLower().Contains(searchString)) ||
                (i.WorkOrder != null && i.WorkOrder.Vehicle != null &&
                 i.WorkOrder.Vehicle.VIN.ToLower().Contains(searchString))
            );
        }

        if (fromDate.HasValue)
        {
            invoicesQuery = invoicesQuery.Where(i => i.DateIssued.Date >= fromDate.Value.Date);
        }

        if (toDate.HasValue)
        {
            invoicesQuery = invoicesQuery.Where(i => i.DateIssued.Date <= toDate.Value.Date);
        }

        var invoices = await invoicesQuery.ToListAsync();
        return View(invoices);
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

        ViewBag.WorkOrders = _context.WorkOrders
            .Include(w => w.Vehicle)
            .ThenInclude(v => v.Customer)
            .Select(w => new
            {
                w.Id,
                Label = $"#{w.Id} - {w.Vehicle.Make} {w.Vehicle.Model} ({w.Vehicle.Customer.FullName})"
            }).ToList();

        if (invoice == null) return NotFound();

        return View(invoice);
    }

    public IActionResult Create()
    {
        PopulateWorkOrdersDropdown();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("WorkOrderId,TaxAmount,SubTotal,Total,PaymentType,Status,DateIssued")]
        Invoice invoice)
    {
        if (ModelState.IsValid)
        {
            _context.Add(invoice);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        PopulateWorkOrdersDropdown();
        return View(invoice);
    }


    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return BadRequest();
        var invoice = await _context.Invoices.FindAsync(id);
        if (invoice == null) return NotFound();
        PopulateWorkOrdersDropdown();
        return View(invoice);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id,
        [Bind("Id,WorkOrderId,TaxAmount,SubTotal,Total,PaymentType,Status,DateIssued")]
        Invoice invoice)
    {
        if (id != invoice.Id) return BadRequest();

        if (ModelState.IsValid)
        {
            try
            {
                var existingInvoice = await _context.Invoices.FindAsync(id);
                if (existingInvoice == null) return NotFound();

                existingInvoice.WorkOrderId = invoice.WorkOrderId;
                existingInvoice.TaxAmount = invoice.TaxAmount;
                existingInvoice.SubTotal = invoice.SubTotal;
                existingInvoice.Total = invoice.Total;
                existingInvoice.DateIssued = invoice.DateIssued;
                existingInvoice.PaymentType = invoice.PaymentType;
                existingInvoice.Status = invoice.Status;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Invoices.Any(e => e.Id == id)) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        PopulateWorkOrdersDropdown();
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

    [HttpGet]
    public async Task<IActionResult> GetWorkOrderDetails(int id)
    {
        var workOrder = await _context.WorkOrders
            .Include(w => w.ServiceDetails)
            .Include(w => w.PartsUsed)
            .ThenInclude(p => p.PartCatalog)
            .Include(w => w.Vehicle)
            .ThenInclude(v => v.Customer)
            .FirstOrDefaultAsync(w => w.Id == id);

        if (workOrder == null) return NotFound();

        decimal laborTotal = workOrder.ServiceDetails.Sum(s => s.LaborHours * s.HourlyRate);
        decimal partsTotal = workOrder.PartsUsed.Sum(p => p.Quantity * p.PartCatalog.PartPrice);

        decimal laborVat = laborTotal * 0.095M; // 9.5%
        decimal partsVat = partsTotal * 0.22M; // 22%

        decimal subTotal = laborTotal + partsTotal;
        decimal tax = laborVat + partsVat;
        decimal total = subTotal + tax;

        return Json(new
        {
            subTotal,
            tax,
            total,
            amountDue = total
        });
    }

    private void PopulateWorkOrdersDropdown()
    {
        var workOrdersWithExistingInvoices = _context.Invoices.Select(i => i.WorkOrderId).ToList();

        ViewBag.WorkOrders = _context.WorkOrders
            .Where(w => !workOrdersWithExistingInvoices.Contains(w.Id))
            .Include(w => w.Vehicle)
            .ThenInclude(v => v.Customer)
            .Select(w => new
            {
                w.Id,
                Label = $"#{w.Id} - {w.Vehicle.Make} {w.Vehicle.Model} ({w.Vehicle.Customer.FullName})"
            }).ToList();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ArchiveInvoice(int id)
    {
        var invoice = await _context.Invoices.FindAsync(id);

        if (invoice == null)
        {
            TempData["ErrorMessage"] = "Invoice not found.";
            return RedirectToAction(nameof(Index));
        }

        if (invoice.Status == InvoiceStatus.Paid)
        {
            invoice.Status = InvoiceStatus.Archived;

            //find associated WorkOrder and archive it
            var workOrder = await _context.WorkOrders.FindAsync(invoice.WorkOrderId);
            if (workOrder != null)
            {
                if (workOrder.Status == WorkOrderStatus.Completed)
                {
                    workOrder.Status = WorkOrderStatus.Archived;
                }
            }

            await _context.SaveChangesAsync();
        }
        
        return RedirectToAction(nameof(Details), new { id = id });
    }
    
    [HttpGet]
    public async Task<IActionResult> ExportPdf(int id)
    {
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

        if (invoice == null)
        {
            return NotFound();
        }
        
        var document = new InvoiceDocument(invoice, _webHostEnvironment);

        byte[] pdfBytes = document.GeneratePdf();

        return File(pdfBytes, "application/pdf", $"Invoice_#{invoice.Id}.pdf");
    }
}