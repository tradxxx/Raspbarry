namespace Raspberry.Models
{
    public class Order
    {
        public long Id { get; set; }
        public string Phone_buyer { get; set;}
        public string Name_product { get; set; }
        public string Image_product { get; set; }
        public DateTime Time_order { get; set; }
        public int Count { get; set; }
        public bool Check_Order { get; set; }
        
    }
}
