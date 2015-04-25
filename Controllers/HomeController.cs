using System.Collections.Generic;
using System;
using System.Globalization;
using System.Linq;
using System.Data.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using FlexRental.Models;
using System.IO;

namespace FlexRental.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;



        public HomeController()
        {
        }

        public HomeController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
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


        public ActionResult Index()
        {
            updateNews();
            return View();
        }

        public ActionResult About()
        {
            
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            List<FlexRental.Controllers.OrderItems> cart = new List<FlexRental.Controllers.OrderItems>();
            cart.Add(new FlexRental.Controllers.OrderItems(db.Inventories.Find(5), 1));
            Session["cart"] = cart;
            ViewBag.Message = "Your contact page.";

            return View();
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
    }
}