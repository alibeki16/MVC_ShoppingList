using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_ShoppingList.Models
{
    public class UrunView
    {
        public int product_id { get; set; }
        public string product_name { get; set; }
        public string product_image { get; set; }
        public string product_desc { get; set; }
        public Nullable<int> product_price { get; set; }
        public Nullable<int> product_fk_cat { get; set; }
        public Nullable<int> product_fk_admin { get; set; }
        public int cat_id { get; set; }
        public string cat_name { get; set; }
    }
}