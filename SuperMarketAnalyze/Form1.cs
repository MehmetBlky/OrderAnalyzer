using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperMarketAnalyze
{
    public partial class Form1 : Form
    {

        private StoreDBEntities db;
        public Form1()
        {
            InitializeComponent();
            db = new StoreDBEntities();

            var marka = db.stock.Select(a => a.brand);

            comboBox1.DataSource = marka.ToArray();

            var customer = db.Customers.Select(a => a.Name);

            comboBox3.DataSource = customer.ToArray();

            string[] time_period = { "Daily", "Weekly", "Monthly", "Yearly" };
            comboBox4.DataSource = time_period;

            string[] analyzer_type = {
                "Ürün Satış Adet - zaman",
                "Ürün Satış Hacim - zaman",
                "Marka adet - zaman",
                "Tek Müşteri İşlemi adet - zaman",
                "Tek Müşteri işlem hacim - zaman",
                "Müşteri işlem yapan adet sıralama [BAR]",
                "Müşteri işlem hacim yapan sıralama [BAR]",
            };

            comboBox5.DataSource = analyzer_type;

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var mdl = db.stock.Where(b => b.brand == comboBox1.SelectedItem.ToString()).Select(a => a.product);

            comboBox2.DataSource = mdl.ToArray();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var period = comboBox4.SelectedItem.ToString();
            switch (comboBox5.SelectedItem.ToString())
            {
                
                case "Ürün Satış Adet - zaman":
                    ProductSalesQuantity_time(period);
                    break;
                case "Ürün Satış Hacim - zaman":
                    ProductSalesVolume_time(period);
                    break;


                case "Marka adet - zaman":
                    ProductSalesVolume_time(period);
                    break;
                case "Tek Müşteri İşlemi adet - zaman":
                    BrandPieces_time(period);
                    break;
                case "Tek Müşteri işlem hacim - zaman":
                    SingleCustomerTransactionquantity_time(period);
                    break;
                case "Müşteri işlem yapan adet sıralama [BAR]":
                    CustomerTransactionCount(period);
                    break;
                case "Müşteri işlem hacim yapan sıralama [BAR]":
                    CustomerTransactionVolumeRanking(period);
                    break;
            }

        }

        private Dictionary<string, int> data = new Dictionary<string, int>();
        DateTime startDT = new DateTime();
        DateTime endDT = new DateTime();

        private void ProductSalesQuantity_time(String period) {
            switch (period) {
                case "Daily":
                    startDT = dateTimePicker1.Value;
                    endDT = dateTimePicker2.Value;

                   var  product_id = db.stock.Where(x => x.product == comboBox2.SelectedItem.ToString()).Select(y=>y.id).First();



                    while (startDT.Date < endDT.Date)
                    {
                        var order_ids = db.Orders.Where(x => x.Date == startDT.Date).Select(y => y.id);
                        

                        foreach (int order_id in order_ids)
                        {
                            var products = db.OrderProductRelationship.Where(c => c.order_id == order_id).Where(y => y.product_id == product_id);

                            var count = products.Count();
                            if (count != 0)
                            {
                                try
                                {
                                    if (data[startDT.Date.ToString("dd/MM/yyyy")] == 0)
                                    {
                                        data[startDT.Date.ToString("dd/MM/yyyy")] = count;
                                    }
                                    else
                                    {
                                        data[startDT.Date.ToString("dd/MM/yyyy")] += count;
                                    }
                                }
                                catch (Exception ex) { data[startDT.Date.ToString("dd/MM/yyyy")] = count; }

                            }

                        }
                        startDT = startDT.AddDays(1);
                    }
                    drawChart(data);
                    break;
                case "Weekly":

                    startDT = dateTimePicker1.Value;
                    endDT = dateTimePicker2.Value;

                    product_id = db.stock.Where(x => x.product == comboBox2.SelectedItem.ToString()).Select(y => y.id).First();

                    var value_count = 0;
                    var week_number = 1;

                    while (startDT.Date < endDT.Date)
                    {

                        var order_ids = db.Orders.Where(x => x.Date == startDT.Date).Select(y => y.id);


                        foreach (int order_id in order_ids)
                        {
                            var products = db.OrderProductRelationship.Where(c => c.order_id == order_id).Where(y => y.product_id == product_id);

                            var count = products.Count();

                            
                            if (count != 0)
                            {
                                try
                                {

                                        value_count += count;

                                }
                                catch (Exception ex) { data[startDT.Date.ToString("dd/MM/yyyy")] = count; }

                            }

                        }
                        startDT = startDT.AddDays(1);

                        if (startDT.DayOfWeek.ToString() == "Monday") {
                            if (value_count > 0)
                            {
                                data["Week " + week_number.ToString() + " : " + startDT.ToString()] = value_count;
                            }
                            week_number++;
                            value_count = 0;
                        }
                    }
                    drawChart(data);



                    break;
            
            
            
            }


            
        }
        private void ProductSalesVolume_time(String period)
        {
            switch (period)
            {
                case "Daily":
                    startDT = dateTimePicker1.Value;
                    endDT = dateTimePicker2.Value;

                    var product_id = db.stock.Where(x => x.product == comboBox2.SelectedItem.ToString()).Select(y => y.id).First();
                    var product_price = db.stock.Where(x => x.product == comboBox2.SelectedItem.ToString()).Select(y => y.sale_price).First();
                    product_price = Convert.ToInt32(product_price);

                    while (startDT.Date < endDT.Date)
                    {
                        var order_ids = db.Orders.Where(x => x.Date == startDT.Date).Select(y => y.id);


                        foreach (int order_id in order_ids)
                        {
                            var products = db.OrderProductRelationship.Where(c => c.order_id == order_id).Where(y => y.product_id == product_id);

                            var count = products.Count();
                            if (count != 0)
                            {
                                try
                                {
                                    if (data[startDT.Date.ToString("dd/MM/yyyy")] == 0)
                                    {
                                        data[startDT.Date.ToString("dd/MM/yyyy")] = Convert.ToInt32(count * product_price);
                                    }
                                    else
                                    {
                                        data[startDT.Date.ToString("dd/MM/yyyy")] += Convert.ToInt32(count * product_price);
                                    }
                                }
                                catch (Exception ex) { data[startDT.Date.ToString("dd/MM/yyyy")] = Convert.ToInt32(count * product_price); }

                            }

                        }
                        startDT = startDT.AddDays(1);
                    }
                    drawChart(data);
                    break;
            }
        }
        private void BrandPieces_time(String period) {
        
        }
        private void SingleCustomerTransactionquantity_time(String period) {
        
        }

        private void SingleClientTransactionVolume_time(String period) {
        
        }

        private void CustomerTransactionCount(String period) {
        
        }

        private void CustomerTransactionVolumeRanking(String period) {
        
        }

        private void drawChart(Dictionary<string, int> My_dict) {
            chart1.Series["data"].Points.Clear();
           chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;

            chart1.Visible = true;
            foreach (string key in My_dict.Keys)
            {
                chart1.Series["data"].Points.AddXY(key, My_dict[key]);
            }
            data = new Dictionary<string, int>();
        }


    }
}
