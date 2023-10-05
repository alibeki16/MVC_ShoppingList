using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_ShoppingList.Controllers
{
    public class HomeController : Controller
    {
        ShoppingCartEntities db = new ShoppingCartEntities();
        public ActionResult Index()
        {
            var list = db.ab_category
                 .OrderByDescending(x => x.cat_id)
                 .ToList();
            return View(list);
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