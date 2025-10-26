using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ECommerceApp.Data;
using ECommerceApp.Model;

namespace ECommerceApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        
        public AdminController(ApplicationDbContext context)
        {
            this._context = context;
        }

        // Dashboard
        public async Task<IActionResult> Index()
        {
            var statistics = new
            {
                TotalProducts = await _context.Products.CountAsync(),
                TotalCategories = await _context.Categories.CountAsync(),
                TotalOrders = await _context.Orders.CountAsync(),
                TotalRevenue = await _context.Orders
                    .Where(o => o.Status != OrderStatus.Cancelled)
                    .SumAsync(o => o.TotalAmount),
                PendingOrders = await _context.Orders
                    .CountAsync(o => o.Status == OrderStatus.Pending),
                LowStockProducts = await _context.Products
                    .Where(p => p.Stock < 10)
                    .CountAsync()
            };

            ViewBag.Statistics = statistics;
            return View();
        }

        #region Products Management
        // Chi thi tien xu ly (preprocessor directives)
        // GET: Admin/Products
        public async Task<IActionResult> Products()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
            return View(products);
        }

        // POST: Admin/CreateProduct
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                product.CreatedAt = DateTime.Now;
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Them san pham thanh cong";
                return RedirectToAction(nameof(Products)); // chuyen sang action Products
            }

            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View(product);       
        }

        // GET: Admin/EditProduct/id
        public async Task<IActionResult> EditProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if(product == null)
            {
                return NotFound();
            }

            ViewBag.Categories = await _context.Categories.FindAsync(id);
            return View(product);
        }

        // POST: Admin/Product/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(int id , Product product)
        {
            if(id != product.Id){
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Cap nhat san pham thanh cong";
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound(ex.Message);
                    }

                    throw;
                }
                return RedirectToAction(nameof(Products));
            }

            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View(product);
        }

        // POST: Admin/DeleteProduct/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null) 
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Xoa san pham thanh cong";
            }
            return RedirectToAction(nameof(Products));
        }
        #endregion

        #region Categories Management
        // GET: Admin/Categories
        public async Task<IActionResult> Categories()
        {
            var categories = await _context.Categories
                .Include(c => c.Products)
                .ToListAsync();
            return View(categories);
        }

        // GET: Admin/CreateCategory
        public IActionResult CreateCategory()
        {
            return View();
        }

        // POST: Admin/CreateCategory
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Them danh muc thanh cong";
                return RedirectToAction(nameof(Categories));
            }
            return View(category);
        }

        // GET: Admin/EditCategory/id
        public async Task<IActionResult> EditCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if(category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Admin/EditCategory/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(int id,  Category category)
        {
            if(id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Cap nhat danh muc thanh cong";
                }catch(DbUpdateConcurrencyException ex)
                {
                    if (!CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Categories));
            }
            return View(category);
        }

        // POST: Admin/DeleteCategory/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);

            if(category != null)
            {
                if(category.Products.Any())
                {
                    TempData["Error"] = "Khong the xoa danh muc dang co san pham";
                }
                else
                {
                    _context.Categories.Remove(category);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Xoa danh muc thanh cong";
                }
            }

            return RedirectToAction(nameof(Categories));
        }
        #endregion

        #region Order Mangement
        
        // GET: Admin/Orders
        public async Task<IActionResult> Orders()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
            return View(orders);
        }

        // GET: Admin/OrderDetails/Id
        public async Task<IActionResult> OrderDetails(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);
            
            if(order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Admin/UpdateOrderStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, OrderStatus status)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if(order != null)
            {
                order.Status = status;
                await _context.SaveChangesAsync();
                TempData["Success"] = "Cap nhat trang thai don hang thanh cong";
            }
            return RedirectToAction(nameof(OrderDetails), new { id = orderId });
        }

        #endregion
        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);

        }

        private bool CategoryExists(int id) 
        { 
            return _context.Categories.Any(e => e.Id == id);
        }

    }
}
