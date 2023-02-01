using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Raspberry.Infrastructure;
using Raspberry.Models;

namespace Raspberry.Controllers
{
 
    public class ProductController : Controller
    {
         private readonly DataContext _context;

        public ProductController(DataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var Products = _context.Products;
            return View(Products);
        }
        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product item)
        {
            if (item == null)
            {
                return BadRequest();
            }
            Product product = new Product
            {
                Supplier = item.Supplier,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                Image = item.Image,
            };
            _context.Products.Add(product);
            _context.SaveChanges();
            return View("Thanks");
        }

        [HttpGet]      
        public ViewResult Delete()
        {
            return View();
        
        }
        [HttpPost]
        public IActionResult Delete(Product item)
        {
            var product = _context.Products.FirstOrDefault(t => t.Id == item.Id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            _context.SaveChanges();
            return View("Thanks");
        }


        [HttpGet]
        public ViewResult Put()
        {
            return View();

        }
        [HttpPost]
        public IActionResult Put(Product item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            var product = _context.Products.FirstOrDefault(t => t.Id == item.Id);
            if (product == null)
            {
                return NotFound();
            }


            product.Supplier = item.Supplier;
            product.Name = item.Name;
            product.Description = item.Description;
            product.Price = item.Price;
            product.Image = item.Image;
            _context.Products.Update(product);
            _context.SaveChanges();
            return View("Thanks");
        }

        [HttpGet]
        public ViewResult GetById()
        {
            return View();
        }
        [HttpPost]
        public IActionResult GetById(Product item)
        {
            var product = _context.Products.FirstOrDefault(t => t.Id == item.Id);
            if (product == null)
            {
                return NotFound();
            }
            
            return View("GetByIdFind",product);
        }
      
    }
}
