using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommerceApp.Data;
using ECommerceApp.Model;
using ECommerceApp.Model.ViewModels;

namespace ECommerceApp.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ApplicationDbContext _context;
        
        public CheckoutController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Index()
        {
            var sessionId = HttpContext.Session.GetString("CartSessionId");
            if (string.IsNullOrEmpty(sessionId))
            {
                return RedirectToAction("Index", "Cart");
            }
            
            var cartItems = await _context.CartItems
                .Include(c => c.Product)
                .Where(c => c.SessionId == sessionId)
                .ToListAsync();
                
            if (!cartItems.Any())
            {
                return RedirectToAction("Index", "Cart");
            }
            
            var viewModel = new CheckoutViewModel
            {
                Items = cartItems.Select(c => new CartItemViewModel
                {
                    ProductId = c.ProductId,
                    ProductName = c.Product.Name,
                    Price = c.Product.Price,
                    Quantity = c.Quantity,
                    ImageUrl = c.Product.ImageUrl
                }).ToList(),
                TotalAmount = cartItems.Sum(c => c.Product.Price * c.Quantity)
            };
            
            return View(viewModel);
        }
        
        [HttpPost]
        public async Task<IActionResult> PlaceOrder(CheckoutViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }
            
            var sessionId = HttpContext.Session.GetString("CartSessionId");
            var cartItems = await _context.CartItems
                .Include(c => c.Product)
                .Where(c => c.SessionId == sessionId)
                .ToListAsync();
                
            if (!cartItems.Any())
            {
                return RedirectToAction("Index", "Cart");
            }
            
            var order = new Order
            {
                CustomerName = model.CustomerName,
                Email = model.Email,
                Phone = model.Phone,
                Address = model.Address,
                TotalAmount = cartItems.Sum(c => c.Product.Price * c.Quantity),
                OrderItems = cartItems.Select(c => new OrderItem
                {
                    ProductId = c.ProductId,
                    Quantity = c.Quantity,
                    UnitPrice = c.Product.Price
                }).ToList()
            };
            
            _context.Orders.Add(order);
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
            
            HttpContext.Session.Remove("CartSessionId");
            
            return RedirectToAction("Success", new { orderId = order.Id });
        }
        
        public async Task<IActionResult> Success(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId);
                
            if (order == null)
            {
                return NotFound();
            }
            
            return View(order);
        }
    }
}