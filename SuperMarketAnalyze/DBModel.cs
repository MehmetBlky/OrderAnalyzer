using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarketAnalyze
{
    internal class DBModel
    {
        StoreDBEntities db;

        public DBModel() {
            db = new StoreDBEntities();
        }


        public System.Linq.IQueryable<string> getBrandNamelist() => db.stock.Select(a => a.brand);
        public System.Linq.IQueryable<string> getCustomersNames() => db.Customers.Select(a => a.Name);
        public System.Linq.IQueryable<string> getProductListByBrand(string brand) => db.stock.Where(b => b.brand == brand).Select(a => a.product);
        public int getProductID(string product) => db.stock.Where(x => x.product == product).Select(y => y.id).First();
        public System.Linq.IQueryable<int> getOrdersIDByDate(DateTime dt) => db.Orders.Where(x => x.Date == dt).Select(y => y.id);
        public System.Linq.IQueryable<SuperMarketAnalyze.OrderProductRelationship> getProductOrderListByOrderIDs(System.Linq.IQueryable<int> order_ids, int product_id) => db.OrderProductRelationship.Where(c => order_ids.Contains(c.order_id)).Where(y => y.product_id == product_id);
        public double getProductPrice(int product_id) => db.stock.Where(x => x.id == product_id).Select(y => y.sale_price).First();
        public System.Linq.IQueryable<int> getProductIDsByBrand(String brand) => db.stock.Where(x => x.brand == brand).Select(y => y.id);
        public System.Linq.IQueryable<SuperMarketAnalyze.OrderProductRelationship> getProductsListByOrderIDsANDProductIDs(System.Linq.IQueryable<int> order_ids, System.Linq.IQueryable<int> product_ids) => db.OrderProductRelationship.Where(c => order_ids.Contains(c.order_id)).Where(y => product_ids.Contains(y.product_id));
        public int getCustomerIDByName(string name) => db.Customers.Where(x => x.Name == name).Select(y => y.id).First();
        public System.Linq.IQueryable<int> getOrderIDsByDateAndCustomer(DateTime dt, int cust_id) => db.Orders.Where(x => x.Date == dt).Where(v => v.CustomerID == cust_id).Select(y => y.id);
        public System.Linq.IQueryable<int> getProductIDSByOrderID(int oid) => db.OrderProductRelationship.Where(x => x.order_id == oid).Select(y => y.product_id);
        public System.Linq.IQueryable<SuperMarketAnalyze.OrderProductRelationship> getOrdersListByOrderIDAndProductID(int oid, int pid) => db.OrderProductRelationship.Where(x => x.order_id == oid).Where(y => y.product_id == pid);
        public System.Linq.IQueryable<int> getCustomerIDsByDateRange(DateTime start, DateTime end) => db.Orders.Where(x => x.Date > start).Where(z => z.Date < end).Select(d => d.CustomerID);
        public System.Linq.IQueryable<SuperMarketAnalyze.Customers> getCustomersListByIDs(System.Linq.IQueryable<int> customers_ids) => db.Customers.Where(x => customers_ids.Contains(x.id));
        public System.Linq.IQueryable<int> getOrdersIDsBeyCustomerIDAndDateRange(DateTime start, DateTime end, int cid) => db.Orders.Where(c => c.CustomerID == cid).Where(x => x.Date > start).Where(z => z.Date < end).Select(i => i.id);
        public string getCustomerNameByID(int cid) => db.Customers.Where(x => x.id == cid).Select(d => d.Name).First().ToString();

    }
}
