using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using NorthWnd.Models;

namespace NorthWnd.Controllers
{
    public class ProductsController : Controller
    {
        NORTHWNDEntities db = new NORTHWNDEntities();
        // GET: Products
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetProductList(int pageSize, int pageIndex, string ProductName)
        {
            IEnumerable<Products> Productlist = db.Products.AsEnumerable();
            int ProductCnt = 0;
            if (!string.IsNullOrEmpty(ProductName))
            {
                Productlist = Productlist.Where(x => x.ProductName.Contains(ProductName)).AsEnumerable();
            }

            ProductCnt = Productlist.Count();

            Productlist = Productlist.OrderBy(x => x.ProductID)
                                     .Skip(pageSize * (pageIndex - 1))
                                     .Take(pageSize)
                                     .AsEnumerable();

            DataTableReturnedData<Products> dataTableReturnedData = new DataTableReturnedData<Products>()
            {
                data = Productlist,
                recordsTotal = ProductCnt,
                recordsFiltered = ProductCnt
            };

            return Json(JsonConvert.SerializeObject(new ApiResult<DataTableReturnedData<Products>>(dataTableReturnedData)));
        }
        public ActionResult GetProductInfo(int id)
        {
            var product = db.Products.FirstOrDefault(x => x.ProductID == id);

            return Json(JsonConvert.SerializeObject(new ApiResult<Products>(product)));
        }

        [HttpPost]
        public ActionResult Remove(int id)
        {
            ApiResult<object> apiresult = null;
            if (id != null)
            {
                var product = db.Products.FirstOrDefault(x => x.ProductID == id);
                if (product != null)
                {
                    db.Products.Remove(product);
                    db.SaveChanges();
                    apiresult = new ApiResult<object>(product);
                }
                else
                {
                    apiresult = new ApiError("12345", "查無資料");
                }
            }
            else
            {
                apiresult = new ApiError("999", "未有key值");
            }

            return Json(JsonConvert.SerializeObject(apiresult));

        }

        [HttpPost]
        public ActionResult Create(Products product)
        //public ActionResult Create(FormCollection formCollection)
        {
            ApiResult<Products> apiResult = null;
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                apiResult = new ApiResult<Products>(product);
                return Json(JsonConvert.SerializeObject(apiResult));
            }
            else
            {
                return Json(JsonConvert.SerializeObject(new ApiError("12345", "驗證失敗")));
            }
        }

        public ActionResult Modify(int ProductID, FormCollection formCollection)
        {
            ApiResult<Products> apiResult = null;
            var oldProduct = db.Products.FirstOrDefault(x => x.ProductID == ProductID);

            if (oldProduct != null)
            {
                if (ModelState.IsValid && TryUpdateModel(oldProduct, formCollection.AllKeys))
                {
                    db.SaveChanges();
                    apiResult = new ApiResult<Products>(oldProduct);

                    return Json(JsonConvert.SerializeObject(apiResult));
                }
                else
                {
                    return Json(JsonConvert.SerializeObject(new ApiError("12345", "失敗")));
                }

            }

            return Json(JsonConvert.SerializeObject(apiResult));
        }

    }
}