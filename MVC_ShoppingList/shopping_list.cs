//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MVC_ShoppingList
{
    using System;
    using System.Collections.Generic;
    
    public partial class shopping_list
    {
        public int list_id { get; set; }
        public string list_name { get; set; }
        public int list_owner_id { get; set; }
        public Nullable<bool> is_shopping_started { get; set; }
        public Nullable<bool> is_shopping_completed { get; set; }
    }
}
