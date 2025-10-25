using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommerceApp.Data;

namespace ECommerceApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ProductsController(ApplicationDbContext context)
        {
            this._context = context;
        }
        public async Task<IActionResult> Index(int? categoryId)
        {
            var products = _context.Products
                .Include(p => p.Category)
                .AsQueryable();
            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == categoryId.Value);
            }

            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.SelectedCategory = categoryId;

            return View(await products.ToListAsync());
        }
        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
    }
}