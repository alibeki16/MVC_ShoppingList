using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_ShoppingList.Models
{
    public class ShoppingListViewModel
    {
        public shopping_list ShoppingList { get; set; }
        public List<shopping_list_items> ShoppingListItems { get; set; }
    }
}