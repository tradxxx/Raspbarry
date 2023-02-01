using Microsoft.AspNetCore.Mvc;
using Raspberry.Infrastructure;
using Raspberry.Models;

namespace Raspberry.Controllers
{
    public class OrderController : Controller
    {
        private readonly DataContext _context;

        public OrderController(DataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var Orders = _context.Orders;
            return View(Orders);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Order item)
        {
            if (item == null)
            {
                return BadRequest();
            }
            Order order = new Order
            {
                Phone_buyer = item.Phone_buyer,
                Name_product = item.Name_product,
                Image_product = item.Image_product,
                Time_order = DateTime.Now,
                Count =item.Count,
                Check_Order=item.Check_Order
            };
            _context.Orders.Add(order);
            _context.SaveChanges();
            return View("Thanks");
        }

        [HttpGet]
        public ViewResult Delete()
        {
            return View();

        }
        [HttpPost]
        public IActionResult Delete(Order item)
        {
            var order = _context.Orders.FirstOrDefault(t => t.Id == item.Id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            _context.SaveChanges();
            return View("Thanks");
        }

        [HttpGet]
        public ViewResult Put()
        {
            return View();

        }
        [HttpPost]
        public IActionResult Put(Order item)
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
            return View("Thanks");
        }

        [HttpGet]
        public ViewResult GetById()
        {
            return View();
        }
        [HttpPost]
        public IActionResult GetById(Order item)
        {
            var order = _context.Orders.FirstOrDefault(t => t.Id == item.Id);
            if (order == null)
            {
                return NotFound();
            }

            return View("GetByIdFind", order);
        }
     
    }
}
