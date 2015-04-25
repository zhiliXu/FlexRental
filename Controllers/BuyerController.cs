using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using FlexRental.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Net;

/**
 * This is the class deal with buyer's request
 * @Author Zhili Xu
 * */
namespace FlexRental.Controllers
{
    public class BuyerController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;



        public BuyerController()
        {
        }

        public BuyerController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: Buyer
        public ActionResult Index()
        {

            return View();
        }



        // GET: Buyer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Buyer/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Buyer/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Buyer/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Buyer/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Buyer/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [AllowAnonymous]
        public ActionResult Search(string Search)
        {

            var result = from i in db.Inventories
                         join m in db.Images
                         on i.InventoryId equals m.InventoryViewId
                         where (i.Name.Contains(Search) || i.Category.Contains(Search) || Search == null) && i.Status == "On Sale" && m.Index == 1
                         orderby i.Name
                         select new ListViewModel { InventoryID = i.InventoryId, Name = i.Name, ImageUrl = m.ImageUrl };
            var strings = result.ToList();
            Session["search"] = Search;
            return View(strings);
        }


        // GET: Inventory/Details/5
        [AllowAnonymous]
        public ActionResult Details(int id)
        {
            updateNews();
            var image = from m in db.Images
                        where m.InventoryViewId == id
                        orderby m.ImageId
                        select m.ImageUrl;
            var inventory = from i in db.Inventories
                            where i.InventoryId == id
                            orderby i.InventoryId descending
                            select new InventoryDetailsViewModel
                            {
                                InventoryId = i.InventoryId,
                                Name = i.Name,
                                Brand = i.Brand,
                                Color = i.Color,
                                Condition = i.Condition,
                                Material = i.Material,
                                Status = i.Status,
                                Price = i.Price,
                                Quantity = i.Quantity,
                                Year = i.Year,
                                Category = i.Category,
                                Description = i.Description,
                                Date = i.Date,
                                Images = image.ToList()
                            };
            return View(inventory.First());
        }
        private int isExisted(int id)
        {
            List<OrderItems> cart = (List<OrderItems>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Item.InventoryId == id)
                    return i;
            }
            return -1;
        }

        public ActionResult CheckOut()
        {
            return View("CheckOut");
        }

        public ActionResult Cart()
        {
            return View("Cart");
        }
        [HttpPost]
        public ActionResult AddToCart(int itemID, int amount)
        {

            if (Session["cart"] == null)
            {
                List<OrderItems> cart = new List<OrderItems>();
                cart.Add(new OrderItems(db.Inventories.Find(itemID), amount));
                Session["cart"] = cart;
            }
            else
            {
                List<OrderItems> cart = (List<OrderItems>)Session["cart"];
                int isExist = isExisted(itemID);
                if (isExist == -1)
                    cart.Add(new OrderItems(db.Inventories.Find(itemID), amount));
                else
                    for (int i = 0; i < amount; i++)
                        cart[isExist].Quantity++;
                Session["cart"] = cart;
            }
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Details", new { id = itemID });

            return PartialView("_Cart", Session["cart"]);
        }

        public ActionResult Order()
        {
            return View();
        }

        [HttpPost]
        public ActionResult updateQtyForCart(int itemID, int updateAmount)
        {
            List<OrderItems> cart = (List<OrderItems>)Session["cart"];
            int index = isExisted(itemID);
            if (updateAmount == 0)
                cart.RemoveAt(index);
            else
                cart[index].Quantity = updateAmount;
            Session["cart"] = cart;

            return RedirectToAction("Cart");
        }
    
        public ActionResult RemoveItem(int itemID)
        {
            List<OrderItems> cart = (List<OrderItems>)Session["cart"];
            int index = isExisted(itemID);
            cart.RemoveAt(index);
            Session["cart"] = cart;
            return RedirectToAction("Cart");
        }
        private void updateNews()
        {
            var news = from i in db.Inventories
                       orderby i.Date descending
                       select new NewsViewModel { InventoryId = i.InventoryId, name = i.Name };

            var listnews = news.ToList().Take(5);
            List<NewsViewModel> final = new List<NewsViewModel>();
            foreach (var one in listnews)
            {
                final.Add(one);
            }
            Session["news"] = final;
        }
        /*
        [HttpPost]
        public ActionResult RemoveFromCart(int itemID)
        {
            List<OrderItems> cart = (List<OrderItems>)Session["cart"];
            int index = isExisted(itemID);
            if (cart[index].Quantity == 1)
                cart.RemoveAt(index);
            else
                cart[index].Quantity--;
            Session["cart"] = cart;

            if (!Request.IsAjaxRequest())
                return RedirectToAction("Details", new { id = itemID });

            return PartialView("_Cart", Session["cart"]);
        }
         * */
    }
}
