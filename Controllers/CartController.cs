using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommerceApp.Data;
using ECommerceApp.Model.ViewModels;
using ECommerceApp.Model;

namespace ECommerceApp.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            this._context = context;
        }
        private string GetCartSessionId()
        {
            var sessionId = HttpContext.Session.GetString("CartSessionId");
            if (string.IsNullOrEmpty(sessionId))
            {
                sessionId = Guid.NewGuid().ToString();
                HttpContext.Session.SetString("CartSessionId", sessionId);
            }
            return sessionId;
        }
        public async Task<IActionResult> Index()
        {
            var sessionId = GetCartSessionId();
            var cartItems = await _context.CartItems
                .Include(c => c.Product)
                .Where(c => c.SessionId == sessionId)
                .ToListAsync();

            var viewModel = new CartViewModel
            {
                Items = cartItems.Select(c => new CartItemViewModel
                {
                    CartItemId = c.Id,
                    ProductId = c.ProductId,
                    ProductName = c.Product.Name,
                    Price = c.Product.Price,
                    Quantity = c.Quantity,
                    ImageUrl = c.Product.ImageUrl
                }).ToList()
            };

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            var sessionId = GetCartSessionId();
            var product = await _context.Products.FindAsync(productId);

            if (product == null || product.Stock < quantity)
            {
                return Json(new { success = false, message = "San pham khong co san" });
            }
            var existingItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.SessionId == sessionId && c.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                var cartItem = new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    SessionId = sessionId
                };
                _context.CartItems.Add(cartItem);
            }
            await _context.SaveChangesAsync();
            return Json(new { success = true, messsage = "Da them vao gio hang" });
        }
        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int cartItemId, int quantity)
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);

            if (cartItem == null)
            {
                return Json(new { success = false });
            }

            if (quantity <= 0)
            {
                _context.CartItems.Remove(cartItem);
            }
            else
            {
                cartItem.Quantity = quantity;
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }
        [HttpPost]
        public async Task<IActionResult> RemoveItem(int cartItemId)
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);

            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}