using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_ShoppingList.Controllers
{
    public class AdminController : Controller
    {
        //DB
        ShoppingCartEntities db = new ShoppingCartEntities();

        // GET: Admin
        [HttpGet]
        public ActionResult Giris()
        {
            return View();
        }
        //POST: Admin
        [HttpPost]
        public ActionResult Giris(ab_admin adm)
        {
            ab_admin ad = db.ab_admin.Where(x => x.ab_admin_mail == adm.ab_admin_mail && x.ab_admin_password == adm.ab_admin_password).SingleOrDefault();
            if (ad != null)
            {

                Session["ab_admin_id"] = ad.ab_admin_id.ToString();
                return RedirectToAction("AdminPanel"); // Admin Panele Yönlendir 

            }
            else
            {
                ViewBag.error = "Geçersiz Mail veya Şifre";

            }

            return View();
        }
        public ActionResult AdminPanel()
        {
            if (Session["ab_admin_id"] == null)
            {
                return RedirectToAction("Giris");
            }
            return View();
        }
        public ActionResult Kategoriler()
        {
            if (Session["ab_admin_id"] == null)
            {
                return RedirectToAction("Giris");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Kategoriler(ab_category ctg, HttpPostedFileBase imgfile)
        {
            string path = uploadimgfile(imgfile);
            if (path.Equals("-1"))
            {
                ViewBag.error = "Resim Bulunamadi...";
            }
            else
            {
                ab_category cat = new ab_category();
                cat.cat_name = ctg.cat_name;
                cat.cat_image = path;
                cat.cat_fk_ad = Convert.ToInt32(Session["ab_admin_id"].ToString());
                db.ab_category.Add(cat);
                db.SaveChanges();
                return RedirectToAction("ViewKategoriler");
            }

            return View();
        }
        public ActionResult ViewKategoriler()
        {
            var list = db.ab_category
                 .OrderByDescending(x => x.cat_id)
                 .ToList();
            return View(list);
        }
        public ActionResult KSil(int id)
        {
            // İlgili kategoriyi veritabanından bulun
            var category = db.ab_category.Find(id);

            if (category != null)
            {
                // Kategoriyi veritabanından silin
                db.ab_category.Remove(category);
                db.SaveChanges();

                // Silme işlemi başarılı olduysa başka bir sayfaya yönlendirin veya mesaj gösterin
                TempData["SuccessMessage"] = "Kategori başarıyla silindi.";
            }
            else
            {
                // Kategori bulunamadıysa hata mesajı gösterin
                TempData["ErrorMessage"] = "Kategori bulunamadı.";
            }

            // Kategorileri görüntüleyen sayfaya geri dönün
            return RedirectToAction("AdminPanel");
        }
        [HttpGet]
        public ActionResult Urunler()
        {
            if (Session["ab_admin_id"] == null)
            {
                return RedirectToAction("Giris");
            }
            List<ab_category> li = db.ab_category.ToList();
            ViewBag.categorylist = new SelectList(li, "cat_id", "cat_name");
            return View();
        }
        [HttpPost]
        public ActionResult Urunler(ab_product pvm, HttpPostedFileBase imgfile)
        {
            List<ab_category> li = db.ab_category.ToList();
            ViewBag.categorylist = new SelectList(li, "cat_id", "cat_name");


            string path = uploadimgfile(imgfile);
            if (path.Equals("-1"))
            {
                ViewBag.error = "Resim Yüklenemedi....";
            }
            else
            {
                ab_product p = new ab_product();
                p.product_name = pvm.product_name;
                p.product_price = pvm.product_price;
                p.product_image = path;
                p.product_fk_cat = pvm.product_fk_cat;
                p.product_desc = pvm.product_desc;
                p.product_fk_admin = Convert.ToInt32(Session["ab_admin_id"].ToString());
                db.ab_product.Add(p);
                db.SaveChanges();
                Response.Redirect("Urunler");

            }

            return View();
        }

        // UPLOAD IMG
        public string uploadimgfile(HttpPostedFileBase file)
        {
            Random r = new Random();
            string path = "-1";
            int random = r.Next();
            if (file != null && file.ContentLength > 0)
            {
                string extension = Path.GetExtension(file.FileName);
                if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".jpeg") || extension.ToLower().Equals(".png"))
                {
                    try
                    {

                        path = Path.Combine(Server.MapPath("~/Content/upload"), random + Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        path = "~/Content/upload/" + random + Path.GetFileName(file.FileName);

                        //    ViewBag.Message = "File uploaded successfully";
                    }
                    catch (Exception ex)
                    {
                        path = "-1";
                    }
                }
                else
                {
                    Response.Write("<script>alert('Only jpg ,jpeg or png formats are acceptable....'); </script>");
                }
            }

            else
            {
                Response.Write("<script>alert('Please select a file'); </script>");
                path = "-1";
            }



            return path;
        }
    }
}