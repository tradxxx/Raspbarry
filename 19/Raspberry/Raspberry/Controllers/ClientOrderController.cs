using Microsoft.AspNetCore.Mvc;
using Raspberry.Infrastructure;
using Raspberry.Models;

namespace Raspberry.Controllers
{
    [Produces("application/json")]
    public class ClientOrderController : Controller
    {
        private readonly DataContext _context;

        public ClientOrderController(DataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IEnumerable<Order> SubmitOrder_WinForm()
        {
            return _context.Orders.ToList();
        }

        [HttpPost]
        public IActionResult CreateOrder([FromBody] Order item)
        {
            if (item == null)
            {
                return BadRequest();
            }
            Order order = new Order { Phone_buyer=item.Phone_buyer,Name_product=item.Name_product, Image_product=item.Image_product, Time_order=DateTime.Now, Count=item.Count, Check_Order=item.Check_Order };
            _context.Orders.Add(order);
            _context.SaveChanges();
            return new NoContentResult();
        }

        [HttpPut]
        public IActionResult UpdateOrder([FromBody] Order item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            var order = _context.Orders.FirstOrDefault(t => t.Id == item.Id);
            if (order == null)
            {
                return NotFound();
            }

            order.Phone_buyer = item.Phone_buyer;
            order.Name_product = item.Name_product;
            order.Image_product = item.Image_product;
            order.Time_order = DateTime.Now;
            order.Count = item.Count;
            order.Check_Order = item.Check_Order;
            _context.Orders.Update(order);
            _context.SaveChanges();
            return new NoContentResult();
        }

        [HttpDelete]
        public IActionResult DeleteOrder(long id)
        {
            var order = _context.Orders.FirstOrDefault(t => t.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            _context.SaveChanges();
            return new NoContentResult();
        }
    }
}
