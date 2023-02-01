using Microsoft.AspNetCore.Mvc;
using Raspberry.Infrastructure;
using Raspberry.Models;

namespace Raspberry.Controllers
{
    [Produces("application/json")]
    public class ClientProductController : Controller
    {
        private readonly DataContext _context;

        public ClientProductController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Product> SubmitProduct_WinForm()
        {
            return _context.Products.ToList();
        }

        [HttpPost]
        public IActionResult CreateProduct([FromBody] Product item)
        {
            if (item == null)
            {
                return BadRequest();
            }
            Product product = new Product { Supplier = item.Supplier, Name = item.Name, Description = item.Description, Price = item.Price, Image = item.Image };
            _context.Products.Add(product);
            _context.SaveChanges();
            return new NoContentResult();
        }

        [HttpPut]
        public IActionResult UpdateProduct ([FromBody] Product item)
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
            product.Name = item.Description;
            product.Description = item.Description;
            product.Price = item.Price;
            product.Image = item.Image;
            _context.Products.Update(product);
            _context.SaveChanges();
            return new NoContentResult();
        }

        [HttpDelete]
        public IActionResult DeleteProduct(long id)
        {
            var product = _context.Products.FirstOrDefault(t => t.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            _context.SaveChanges();
            return new NoContentResult();
        }
    }
}
