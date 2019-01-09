using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;


namespace BookStore.Controllers
{


    public class HomeController : Controller
    {

        public int number;
        public DateTime first;
        public DateTime second;
        

        //GET: Home/Form
        public ActionResult Form()
        {
            return View();
        }

        //POST: Home/Form
        [HttpPost]
        public ActionResult Form(FormCollection collection)
        {
            try
            {
                ViewData["Number"] = collection[1];
                ViewData["FirstDate"] = collection[2];
                ViewData["SecondDate"] = collection[3];

                number = int.Parse(collection[1]);
                first = DateTime.Parse(collection[2]);
                second = DateTime.Parse(collection[3]);

                TempData["number"] = number; // Για μεταφορά του number από τον ένα Action στο άλλο!
                TempData["first"] = first;
                TempData["second"] = second;


                return RedirectToAction("TopSales"); // Sends data to TopSales Action
            }
            catch
            {
                return View();
            }

            
            
        }

        private pubsEntities db = new pubsEntities();
        //GET: Home/TopSales
        public ActionResult TopSales()
        {

            int num = Convert.ToInt32(TempData["number"]);
            DateTime first = Convert.ToDateTime(TempData["first"]);
            DateTime second = Convert.ToDateTime(TempData["second"]);


            //SQL
            if (num != 0)
            {
                var topsales = (from ta in db.titleauthors
                                join t in db.titles on ta.title_id equals t.title_id
                                join s in db.sales on t.title_id equals s.title_id
                                where s.ord_date > first  && s.ord_date < second
                                orderby t.ytd_sales descending
                                select ta).Take(num);

                return View(topsales.ToList());
            }
            else
            {
                num = 5;
                var au = (from s in db.titleauthors
                             select s).Take(num);
                //return RedirectToAction("");
                return View(au.ToList());
            }
 
             
        }

        //GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}