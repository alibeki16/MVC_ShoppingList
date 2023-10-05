using MVC_ShoppingList.Models;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_ShoppingList.Controllers
{
    public class KullaniciController : Controller
    {
        ShoppingCartEntities db = new ShoppingCartEntities();
        // GET: Kullanici


        // DENEY ALANI --------------------------------
        public ActionResult DenemeSayfasi()
        {
            var list = db.ab_category.OrderByDescending(x => x.cat_id).ToList();
            var list2 = db.shopping_list.ToList();
            var viewModel = new BirlesikKategoriUrunView
            {
                Categories = list,
                Shopping = list2
            };

            return View(viewModel);
        }
        public ActionResult Kategori(int? id)
        {
            int userId;
            if (Session["ab_user_id"] != null && int.TryParse(Session["ab_user_id"].ToString(), out userId))
            {
                var list = db.ab_product.Where(x => x.product_fk_cat == id)
                                    .OrderByDescending(x => x.product_id)
                                    .ToList();
                return View(list);
            }
            else
            {
                return RedirectToAction("GirisYap");
            }
        }
        public ActionResult Urun(int? id)
        {
            UrunView ad = new UrunView();
            ab_product p = db.ab_product.Where(x => x.product_id == id).SingleOrDefault();
            ad.product_id = p.product_id;
            ad.product_name = p.product_name;
            ad.product_image = p.product_image;
            ad.product_price = p.product_price;
            ad.product_desc = p.product_desc;
            ab_category cat = db.ab_category.Where(x => x.cat_id == p.product_fk_admin).SingleOrDefault();
            ad.cat_name = cat.cat_name;

            return View(ad);
        }
        public ActionResult UrunuListeyeEkle(int product_id)
        {
            var list = db.shopping_list.OrderByDescending(x => x.list_id).ToList();
            return View(list);
        }
        [HttpPost]
        public ActionResult UrunuListeyeEkle(int product_id, int selectedListName, int quantity, string description)
        {
                var newItem = new MVC_ShoppingList.shopping_list_items
                {
                    list_id = selectedListName, // Seçilen alışveriş listesinin list_id'si
                    product_id = product_id, // URL'den gelen product_id
                                             
                    quantity = quantity,
                    description = description
                };

                db.shopping_list_items.Add(newItem);
                db.SaveChanges();
                return RedirectToAction("DenemeSayfasi"); // Başka bir sayfaya yönlendirilebilir.
        }
        public ActionResult Listemden_esya_sil(int item_id, int product_id)
        {
            var itemToDelete = db.shopping_list_items.SingleOrDefault(x => x.item_id == item_id && x.product_id == product_id);
            if (itemToDelete != null)
            {
                db.shopping_list_items.Remove(itemToDelete);
                db.SaveChanges();
            }
            return RedirectToAction("DenemeSayfasi");
        }
        // Liste silme işlemini gerçekleştiren eylem
        [HttpPost]
        [ValidateAntiForgeryToken] // CSRF koruması için
        public ActionResult Sil(int id)
        {
            int userId;
            if (Session["ab_user_id"] != null && int.TryParse(Session["ab_user_id"].ToString(), out userId))
            {
                shopping_list p = db.shopping_list
            .Where(x => x.list_id == id && x.list_owner_id == userId)
                .SingleOrDefault();
                db.shopping_list.Remove(p);
                db.SaveChanges();
                return RedirectToAction("DenemeSayfasi");
            }
            else
            {
                return RedirectToAction("GirisYap");
            }
        }
        public ActionResult Detaylar(int id)
        {
            var shoppingList = db.shopping_list.SingleOrDefault(x => x.list_id == id);
            if (shoppingList == null)
            {
                // Alışveriş listesi bulunamadı, hata işlemleri burada ele alınabilir
                return RedirectToAction("DenemeSayfasi");
            }
            var shoppingListItems = db.shopping_list_items
                .Where(x => x.list_id == id)
                .ToList();

            var viewModel = new ShoppingListViewModel
            {
                ShoppingList = shoppingList,
                ShoppingListItems = shoppingListItems
            };

            return View(viewModel);
        }
        public ActionResult AlisverisBaslasin(int id)
        {
            var shoppingList = db.shopping_list.SingleOrDefault(x => x.list_id == id);
            if (shoppingList == null)
            {
                // Alışveriş listesi bulunamadı, hata işlemleri burada ele alınabilir
                return RedirectToAction("DenemeSayfasi");
            }
            var shoppingListItems = db.shopping_list_items
                .Where(x => x.list_id == id)
                .ToList();

            var viewModel = new ShoppingListViewModel
            {
                ShoppingList = shoppingList,
                ShoppingListItems = shoppingListItems
            };

            return View(viewModel);
        }
        public ActionResult Search(string aramaTerimi, int? kategori)
        {
            var sonuclar = db.ab_product.AsQueryable();
            if (!string.IsNullOrEmpty(aramaTerimi))
            {
                sonuclar = sonuclar.Where(p => p.product_name.Contains(aramaTerimi));
            }
            if (kategori.HasValue && kategori.Value > 0)
            {
                sonuclar = sonuclar.Where(p => p.product_fk_cat == kategori.Value);
            }
            return View(sonuclar.ToList()); // KATEGORİYE GÖRE ARAMADA CİDDİ SIKINTI VAR :(
        }
        public ActionResult CikisYap()
        {
            Session.RemoveAll();
            Session.Abandon();

            return RedirectToAction("DenemeSayfasi");
        }

        //---------------------------------------------
        public ActionResult Listelerim()
        {
            return View();
        }
        public ActionResult ListelerimGoruntule()
        {
            int userId;
            if (Session["ab_user_id"] != null && int.TryParse(Session["ab_user_id"].ToString(), out userId))
            {
                var list = db.shopping_list.Where(x => x.list_owner_id == userId)
                                          .OrderByDescending(x => x.list_id)
                                          .ToList();
                return View(list);
            }
            else
            {
                return RedirectToAction("DenemeSayfasi");
            }
        }
        [HttpPost]
        public ActionResult Listelerim(shopping_list shp)
        {
            shopping_list s = new shopping_list();
            s.list_name = shp.list_name;
            s.list_owner_id = Convert.ToInt32(Session["ab_user_id"].ToString());
            s.is_shopping_started = false;
            s.is_shopping_completed = false;
            db.shopping_list.Add(s);
            db.SaveChanges();
            return RedirectToAction("Listelerim");
        }

        public ActionResult KayitOl()
        {
            return View();
        }
        [HttpPost]
        public ActionResult KayitOl(ab_user uvm)
        {
            ab_user u = new ab_user();
            u.ab_user_name = uvm.ab_user_name;
            u.ab_user_surname = uvm.ab_user_surname;
            u.ab_user_password = uvm.ab_user_password;
            u.ab_user_email = uvm.ab_user_email;
            db.ab_user.Add(u);
            db.SaveChanges();
            return RedirectToAction("GirisYap");
        }
        [HttpGet]
        public ActionResult GirisYap()
        {
            List<ab_category> li = db.ab_category.ToList();
            ViewBag.categorylist = new SelectList(li, "cat_id", "cat_name"); 
            return View();
        }
        [HttpPost]
        public ActionResult GirisYap(ab_user avm)
        {
            ab_user ad = db.ab_user.Where(x => x.ab_user_email == avm.ab_user_email && x.ab_user_password == avm.ab_user_password).SingleOrDefault();
            if (ad != null)
            {

                Session["ab_user_id"] = ad.ab_user_id.ToString();
                return RedirectToAction("DenemeSayfasi", "Kullanici");
            }
            else
            {
                ViewBag.error = "Mail veya Sifre Yanlis";

            }

            return View();
        }
    }
}