using Newtonsoft.Json;
using System.Text;
using MessagingToolkit.QRCode.Codec;
using MessagingToolkit.QRCode.Codec.Data;

namespace RaspberryWinForms
{
    public partial class Form1 : Form
    {
        public class Product
        {
            public long Id { get; set; }
            public string Supplier { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
          
            public decimal Price { get; set; }
            public string Image { get; set; }
        }
        public class Order
        {
            public long Id { get; set; }
            public string Phone_buyer { get; set; }
            public string Name_product { get; set; }
            public string Image_product { get; set; }
            public DateTime Time_order { get; set; }
            public int Count { get; set; }
            public bool Check_Order { get; set; }

        }
        public Form1()
        {
            InitializeComponent();
        }

        private const string APP_PATH = "https://localhost:7011";
        private void button1_Click(object sender, EventArgs e)
        {
            GetAllProducts();
        }
        private async void GetAllProducts()
        {
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync(APP_PATH + "/ClientProduct/SubmitProduct_WinForm"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var productJsonString = await response.Content.ReadAsStringAsync();
                        dataGridView1.DataSource = JsonConvert.DeserializeObject<Product[]>(productJsonString).ToList();
                        dataGridView1.Columns[0].HeaderText = "Id";
                        dataGridView1.Columns[1].HeaderText = "Supplier";
                        dataGridView1.Columns[2].HeaderText = "Name";
                        dataGridView1.Columns[3].HeaderText = "Description";
                        dataGridView1.Columns[4].HeaderText = "Price";
                        dataGridView1.Columns[5].HeaderText = "Image";
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GetAllOrders();
        }

        private async void GetAllOrders()
        {
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync(APP_PATH + "/ClientOrder/SubmitOrder_WinForm"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var orderJsonString = await response.Content.ReadAsStringAsync();
                        dataGridView1.DataSource = JsonConvert.DeserializeObject<Order[]>(orderJsonString).ToList();
                        dataGridView1.Columns[0].HeaderText = "Id";
                        dataGridView1.Columns[1].HeaderText = "Phone_buyer";
                        dataGridView1.Columns[2].HeaderText = "Name_product";
                        dataGridView1.Columns[3].HeaderText = "Image_product ";
                        dataGridView1.Columns[4].HeaderText = "Time_order";
                        dataGridView1.Columns[5].HeaderText = "Count";
                        dataGridView1.Columns[6].HeaderText = "Check_Order";
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddProduct();
        }
        private async void AddProduct()
        {
            Product p = new Product();
            p.Id = 0;
            p.Supplier = textBox1.Text;
            p.Name = textBox2.Text;
            p.Description = textBox3.Text;
            p.Price = int.Parse(textBox4.Text);
            p.Image = textBox5.Text;

            string qrtext = textBox6.Text.Trim(); ; //считываем текст из TextBox'a - например, Bublic SMOLGU
            QRCodeEncoder encoder = new QRCodeEncoder(); //создаЄм новую "генерацию кода"
            Bitmap qrcode = encoder.Encode(qrtext,Encoding.UTF8); // кодируем слово, полученное из TextBox'a (qrtext) в переменную qrcode. класса Bitmap(класс, который используетс€ дл€ работы с изображени€ми)
            PictureBox picture = new PictureBox();
            picture.Image = qrcode as Image;

            //—охранить изображение - QR-код в желаемом месте
            SaveFileDialog save = new SaveFileDialog(); // save будет запрашивать у пользовател€ место, в которое он захочет сохранить файл. 
            save.Filter = "PNG|*.png|JPEG|*.jpg|GIF|*.gif|BMP|*.bmp";
            save.FileName = String.Format("{0}/{1}", "H:/ѕрактика —амойлова €нварь 2023/ѕрототипы малинки/20/Raspberry/Raspberry/wwwroot/qr_code_product/", "qr-"+p.Image);
            picture.Image.Save(save.FileName);

            using (var client = new HttpClient())
            {
                var serializedProduct = JsonConvert.SerializeObject(p);
                var content = new StringContent(serializedProduct, Encoding.UTF8, "application/json");
                var result = await client.PostAsync(APP_PATH + "/ClientProduct/CreateProduct", content);

                if (result.IsSuccessStatusCode)
                {
                    var response = result.StatusCode.ToString();
                    MessageBox.Show("ƒобавление в базу выполнено!!!");
                }
                else { MessageBox.Show("ѕроблемы с добавлением в базу !!??"); }
            }

            GetAllProducts();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            UpdateProduct();
        }
        private async void UpdateProduct()
        {
            Product p = new Product();
            int selRowNum = dataGridView1.CurrentCell.RowIndex;
            p.Id = int.Parse(dataGridView1.Rows[selRowNum].Cells[0].Value.ToString());
            p.Supplier = dataGridView1.Rows[selRowNum].Cells[1].Value.ToString();
            p.Name = dataGridView1.Rows[selRowNum].Cells[2].Value.ToString();
            p.Description = dataGridView1.Rows[selRowNum].Cells[3].Value.ToString();
            p.Price = Convert.ToDecimal(dataGridView1.Rows[selRowNum].Cells[4].Value.ToString());
            p.Image = dataGridView1.Rows[selRowNum].Cells[5].Value.ToString();
            using (var client = new HttpClient())
            {
                var serializedProduct = JsonConvert.SerializeObject(p);
                var content = new StringContent(serializedProduct, Encoding.UTF8, "application/json");
                var result = await client.PutAsync((APP_PATH + "/ClientProduct/UpdateProduct"), content);
            }
            GetAllProducts();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DeleteProduct();
        }

        private async void DeleteProduct()
        {

            int selRowNum = dataGridView1.CurrentCell.RowIndex;
            int Num = int.Parse(dataGridView1.Rows[selRowNum].Cells[0].Value.ToString());

            Product p = new Product();
            p.Id = Num;
            
            using (var client = new HttpClient())
            {
                var result = await client.DeleteAsync(String.Format("{0}/{1}", APP_PATH + "/ClientProduct/DeleteProduct", p.Id));
            }
            GetAllProducts();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            AddOrder();
        }

        private async void AddOrder()
        {
            Order o = new Order();
            o.Id = 0;
            o.Phone_buyer = textBox10.Text;
            o.Name_product = textBox9.Text;
            o.Image_product = textBox8.Text;
            o.Time_order = DateTime.Now;
            o.Count = int.Parse(textBox7.Text);
            o.Check_Order = checkBox1.Checked;


            string qrtext = textBox11.Text.Trim(); //считываем текст из TextBox'a - например, Bublic SMOLGU
            QRCodeEncoder encoder = new QRCodeEncoder(); //создаЄм новую "генерацию кода"
            Bitmap qrcode = encoder.Encode(qrtext, Encoding.UTF8); // кодируем слово, полученное из TextBox'a (qrtext) в переменную qrcode. класса Bitmap(класс, который используетс€ дл€ работы с изображени€ми)
            PictureBox picture = new PictureBox();
            picture.Image = qrcode as Image;

            //—охранить изображение - QR-код в желаемом месте
            SaveFileDialog save = new SaveFileDialog(); // save будет запрашивать у пользовател€ место, в которое он захочет сохранить файл. 
            save.Filter = "PNG|*.png|JPEG|*.jpg|GIF|*.gif|BMP|*.bmp";
            save.FileName = String.Format("{0}/{1}", "H:/ѕрактика —амойлова €нварь 2023/ѕрототипы малинки/20/Raspberry/Raspberry/wwwroot/qr_code_order/", "qr-"+o.Image_product);
            picture.Image.Save(save.FileName);


            using (var client = new HttpClient())
            {
                var serializedOrder = JsonConvert.SerializeObject(o);
                var content = new StringContent(serializedOrder, Encoding.UTF8, "application/json");
                var result = await client.PostAsync(APP_PATH + "/ClientOrder/CreateOrder", content);

                if (result.IsSuccessStatusCode)
                {
                    var response = result.StatusCode.ToString();
                    MessageBox.Show("ƒобавление в базу выполнено!!!");
                }
                else { MessageBox.Show("ѕроблемы с добавлением в базу !!??"); }
            }

            GetAllOrders();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            UpdateOrder();
        }
        private async void UpdateOrder()
        {
            Order o = new Order();
            int selRowNum = dataGridView1.CurrentCell.RowIndex;
            o.Id = int.Parse(dataGridView1.Rows[selRowNum].Cells[0].Value.ToString());
            o.Phone_buyer = dataGridView1.Rows[selRowNum].Cells[1].Value.ToString();
            o.Name_product = dataGridView1.Rows[selRowNum].Cells[2].Value.ToString();
            o.Image_product = dataGridView1.Rows[selRowNum].Cells[3].Value.ToString();
            o.Time_order = DateTime.Now;
            o.Count = int.Parse(dataGridView1.Rows[selRowNum].Cells[5].Value.ToString());
            o.Check_Order = Convert.ToBoolean(dataGridView1.Rows[selRowNum].Cells[6].Value.ToString());

            using (var client = new HttpClient())
            {
                var serializedOrder = JsonConvert.SerializeObject(o);
                var content = new StringContent(serializedOrder, Encoding.UTF8, "application/json");
                var result = await client.PutAsync((APP_PATH + "/ClientOrder/UpdateOrder"), content);
            }
            GetAllOrders();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            DeleteOrder();
        }
        private async void DeleteOrder()
        {

            int selRowNum = dataGridView1.CurrentCell.RowIndex;
            int Num = int.Parse(dataGridView1.Rows[selRowNum].Cells[0].Value.ToString());

            Order p = new Order();
            p.Id = Num;

            using (var client = new HttpClient())
            {
                var result = await client.DeleteAsync(String.Format("{0}/{1}", APP_PATH + "/ClientOrder/DeleteOrder", p.Id));
            }
            GetAllOrders();
        }
    }
}