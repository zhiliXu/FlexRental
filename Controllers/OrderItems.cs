using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FlexRental.Models;
/**
 * This class object is used when user try to add an item to the shopping cart.
 * @Author Zhili Xu
 * */
namespace FlexRental.Controllers
{
    public class OrderItems
    {
        private InventoryView item = new InventoryView();

        public InventoryView Item
        {
            get { return item; }
            set { item = value; }
        }
        private int quantity;

        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        public OrderItems()
        {

        }
        public OrderItems(InventoryView product, int qty)
        {
            item = product;
            quantity = qty;
        }
    }
}