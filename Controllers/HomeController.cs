using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GaragePRO.Models;
using GaragePRO.Data;
using Microsoft.EntityFrameworkCore;


namespace GaragePRO.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;
    
    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        //fetch dashboard data
        var pendingWorkOrders = await _context.WorkOrders
            .Where(wo => wo.Status == WorkOrderStatus.Pending || wo.Status == WorkOrderStatus.InProgress)
            .CountAsync();

        var unpaidInvoices = await _context.Invoices
            .Where(inv => inv.Status == InvoiceStatus.Pending || inv.Status == InvoiceStatus.Issued || inv.Status == InvoiceStatus.Unpaid) 
            .CountAsync();

        var totalCustomers = await _context.Customers.CountAsync();
        var totalVehicles = await _context.Vehicles.CountAsync();
        var totalMechanics = await _context.Mechanics.CountAsync();
        
        ViewBag.PendingWorkOrders = pendingWorkOrders;
        ViewBag.UnpaidInvoices = unpaidInvoices;
        ViewBag.TotalCustomers = totalCustomers;
        ViewBag.TotalVehicles = totalVehicles;
        ViewBag.TotalMechanics = totalMechanics; 

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}