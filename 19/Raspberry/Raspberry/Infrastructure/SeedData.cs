using Microsoft.EntityFrameworkCore;
using Raspberry.Models;

namespace Raspberry.Infrastructure
{
    public class SeedData
    {
        public static void SeedDatabase(DataContext context)
        {
            context.Database.Migrate();

            if (!context.Products.Any() && !context.Orders.Any())
            {

                context.Products.AddRange(
                    new Product
                    {
                        Supplier = "ООО''Глинка''",
                        Name = "Огурец",
                        Description = "Зеленый",
                        Price = 100.01M,
                        Image = "огурцы.jpg"
                    }
                    );
                context.Orders.AddRange(
                    new Order
                    {
                        Phone_buyer = "89517144719",
                        Name_product = "Огурец",
                        Image_product = "огурцы.jpg",
                        Time_order = DateTime.Parse("5 / 1 / 2008 8:30:52 AM", System.Globalization.CultureInfo.InvariantCulture),
                        Count = 4,
                        Check_Order = true
                    }
                    );
            }

            context.SaveChanges();
        }
    }
}
