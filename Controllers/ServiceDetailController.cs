using GaragePRO.Data;
using GaragePRO.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GaragePRO.Controllers;

public class ServiceDetailController : Controller
{
    private readonly ApplicationDbContext _context;

    public ServiceDetailController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Create(int? workOrderId)
    {

        var serviceDetail = new ServiceDetail { WorkOrderId = workOrderId.Value };
        ViewBag.WorkOrderId = workOrderId.Value;
        return View(serviceDetail);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ServiceDetail serviceDetail)
    {
        if (!await _context.WorkOrders.AnyAsync(w => w.Id == serviceDetail.WorkOrderId))
        {
            ModelState.AddModelError("", "Invalid work order.");
            return View(serviceDetail);
        }

        if (ModelState.IsValid)
        {
            serviceDetail.CreatedAt = DateTime.Now;
            _context.ServiceDetails.Add(serviceDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "WorkOrder", new { id = serviceDetail.WorkOrderId });
        }

        ViewBag.WorkOrderId = serviceDetail.WorkOrderId;
        return View(serviceDetail);
    }

}