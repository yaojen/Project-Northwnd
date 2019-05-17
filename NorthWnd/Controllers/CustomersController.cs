using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NorthWnd.Models;
using Newtonsoft.Json;


namespace NorthWnd.Controllers
{

    public class CustomersController : Controller
    {
        NORTHWNDEntities db = new NORTHWNDEntities();

        // GET: Customers
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetCustomerList()
        {
            List<Customers> customers = db.Customers.ToList();
            return Json(JsonConvert.SerializeObject(new ApiResult<List<Customers>>(customers)), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCustomerInfo(string id)
        {
            var customer = db.Customers.FirstOrDefault(x => x.CustomerID == id);
            return Json(JsonConvert.SerializeObject(new ApiResult<Customers>(customer)));
        }

        public ActionResult Remove(string id)
        {
            ApiResult<Customers> apiResult;
            var customer = db.Customers.FirstOrDefault(x => x.CustomerID == id);

            if (customer != null)
            {
                db.Customers.Remove(customer);
                db.SaveChanges();

                apiResult = new ApiResult<Customers>(customer) { Message = "刪除成功" };

            }
            else
            {
                apiResult = new ApiResult<Customers>() { Succ = false, Message = "刪除目標不存在" };
            }

            return Json(JsonConvert.SerializeObject(apiResult));

        }

        
        public ActionResult Add(FormCollection formCollection)
        {
            ApiResult<Customers> apiResult = null;
            if (ModelState.IsValid)
            {
                string key = GetRandomKey();
                Customers customer = new Customers()
                {
                    CustomerID = key,
                    Address = formCollection["Address"],
                    City = formCollection["City"],
                    CompanyName = formCollection["CompanyName"],
                    ContactTitle = formCollection["ContactTitle"],
                    Country = formCollection["Country"],
                    Fax = formCollection["Fax"]
                };

                db.Customers.Add(customer);
                db.SaveChanges();

                apiResult = new ApiResult<Customers>(customer);

            }
            else
            {
                //apiResult = new ApiResult<Customers>(customer);

            }
            return Json(JsonConvert.SerializeObject(apiResult),JsonRequestBehavior.AllowGet);
        }

        private string GetRandomKey()
        {
            string[] myArray = new string[] { "1", "2", "3", "4", "5",
                                              "6", "7", "8", "9", "0",
                                              "A", "B", "C", "D", "E",
                                              "F", "G", "H", "I", "J",
                                              "K", "L", "M", "N", "O",
                                              "P", "Q", "R", "S", "T",
                                              "U", "V", "X", "Y", "Z" };

            string key = "";
            Random rnd = new Random();
            for (int i = 0; i < 5; i++)
            {
                int index = rnd.Next() % 25;
                key += myArray[index];
            }
            
            return key;
        }

        public ActionResult Modify(FormCollection formCollection)
        {
            ApiResult<Customers> apiResult = null;
            string customerId = formCollection["CustomerID"];
            Customers oldCustomers = db.Customers.FirstOrDefault(x => x.CustomerID == customerId);

            if (ModelState.IsValid && TryUpdateModel(oldCustomers,formCollection.AllKeys))
            {
                db.SaveChanges();
                apiResult = new ApiResult<Customers>(new Customers());
            }
            else
            {
                apiResult = new ApiResult<Customers>(new Customers());
            }

            return Json(JsonConvert.SerializeObject(apiResult));
        }

    }
}