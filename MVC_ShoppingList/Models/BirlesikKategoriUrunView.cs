﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_ShoppingList.Models
{
    public class BirlesikKategoriUrunView
    {
        public List<ab_category> Categories { get; set; }
        public List<shopping_list> Shopping { get; set; }
    }
}