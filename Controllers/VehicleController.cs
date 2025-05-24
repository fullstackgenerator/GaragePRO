using GaragePRO.Data;
using Microsoft.AspNetCore.Mvc;

namespace GaragePRO.Controllers;

public class VehicleController : Controller
{
    private readonly ApplicationDbContext _context;

    public VehicleController(ApplicationDbContext context)
    {
        _context = context;
    }
}