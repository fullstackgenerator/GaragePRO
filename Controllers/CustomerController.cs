using GaragePRO.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GaragePRO.Models;

namespace GaragePRO.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomerController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var customer = await _context.Customers.ToListAsync();
            return View(customer);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return BadRequest();

            var customer = await _context.Customers
                .Include(c => c.Vehicles) 
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null) return NotFound();

            return View(customer);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("FullName,Email,Phone,Address,City,PostalCode")] Customer customer)
        {
            Console.WriteLine("POST /Customer/Create hit");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState invalid:");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($" - {error.ErrorMessage}");
                }

                return View(customer);
            }

            _context.Add(customer);
            await _context.SaveChangesAsync();
            Console.WriteLine("Customer saved.");
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return NotFound();
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Email,Phone,Address,City,PostalCode")] Customer customer)
        {
            if (id != customer.Id) return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (customer == null) return NotFound();
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}