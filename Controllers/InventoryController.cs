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
/**
 * This is the class deal with seller's request
 * @Author Zhili Xu
 * */
namespace FlexRental.Controllers
{
    public class InventoryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;



        public InventoryController()
        {
        }

        public InventoryController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
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

        // GET: Inventory
        public ActionResult Index()
        {
            updateNews();
            return View("Index");
        }

        // GET: Inventory/Details/5
        public ActionResult Details(int id)
        {
            var image = from m in db.Images
                        where m.InventoryViewId == id
                        orderby m.ImageId
                        select m.ImageUrl;
            var inventory = from i in db.Inventories
                            where i.InventoryId == id
                            orderby i.InventoryId descending
                            select new InventoryDetailsViewModel
                            {
                                InventoryId= i.InventoryId,
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

        // GET: Inventory/Create
        public ActionResult Create()
        {/*
            List<SelectListItem> years = new List<SelectListItem>();
            for (int i = 0; i <= 9; i++)
            {
                string year = "200" + Convert.ToString(i);
                years.Add(new SelectListItem
                {
                    Text = year,
                    Value = year
                });
            }
          * */
            string[] years = new string[10];
            
            for (int i = 0; i <= 9; i++)
            {
                string year = "200" + Convert.ToString(i);
                years[i] = year;
            }
            SelectList yearList = new SelectList(years);
            ViewBag.Year = yearList;
            string[] categories = new string[3];
            categories[0] = "Furniture";
            categories[1] = "Electronic";
            categories[2] = "Automobile";
            SelectList categoryList = new SelectList(categories);
            ViewBag.Category = categoryList;
            return View();
        }

        // POST: Inventory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create(AddInventoryViewModel collection)
        {
            //try
            //{
            string[] years = new string[10];

            for (int i = 0; i <= 9; i++)
            {
                string year = "200" + Convert.ToString(i);
                years[i] = year;
            }
            SelectList yearList = new SelectList(years);
            ViewBag.Year = yearList;
            string[] categories = new string[3];
            categories[0] = "Furniture";
            categories[1] = "Electronic";
            categories[2] = "Automobile";
            SelectList categoryList = new SelectList(categories);
            ViewBag.Category = categoryList;
            var validImageTypes = new string[]
            {
                "image/gif",
                "image/jpeg",
                "image/pjpeg",
                "image/png"
            };
            if (ModelState.IsValid)
            {
                if (collection.ImageUpload.Count() > 7)
                {
                    ModelState.AddModelError("ImageUpload", "Please choose less than 7 images to upload");
                }
                var db = new ApplicationDbContext();
                var identity = (ClaimsIdentity)User.Identity;
                var newInventory = new InventoryView { Date = DateTime.Now, Description = collection.Description, ApplicationUserId = User.Identity.GetUserId(), Name = collection.Name, Color = collection.Color, Material = collection.Material, Brand = collection.Brand, Category = collection.Category, Year = collection.Year, Status = "On Sale", Condition = collection.Condition, Price = collection.Price, Quantity = collection.Quantity };
                var a = db.Inventories.Add(newInventory);
                db.SaveChanges();

                var queryNewInventory = from i in db.Inventories
                                        where i.Name == collection.Name
                                        orderby i.InventoryId descending
                                        select i.InventoryId;
                var newInventoryId = queryNewInventory.ToList().First();
                updateNews();
                if (collection.ImageUpload.First() == null)
                {
                    var image = new ImageView { ImageUrl = "~/ImageUploads/No_Image_Available.png", InventoryViewId = newInventoryId, Index = 1 };
                    db.Images.Add(image);
                    db.SaveChanges();
                    return RedirectToAction("InventoryIndex");
                }

                if (collection.ImageUpload != null)
                {
                    int count = 1;
                    foreach (var item in collection.ImageUpload)
                    {
                        if (!validImageTypes.Contains(item.ContentType))
                        {
                            ModelState.AddModelError("ImageUpload", "Please choose either a GIF, JPG or PNG image.");
                        }
                        if (item.ContentLength > 0)
                        {
                            var username = identity.FindFirstValue(ClaimTypes.GivenName) ?? identity.GetUserName();
                            var fileName = username + "_" + newInventoryId + "_" + count + "_" + item.FileName;
                            var uploadDir = "~/ImageUploads";
                            var imagePath = Path.Combine(Server.MapPath(uploadDir), fileName);
                            var imageUrl = Path.Combine(uploadDir, fileName);
                            item.SaveAs(imagePath);
                            var image = new ImageView { ImageUrl = imageUrl, InventoryViewId = newInventoryId, Index = count };
                            db.Images.Add(image);
                            db.SaveChanges();
                            count = count + 1;
                        }
                        else
                            ModelState.AddModelError("ImageUpload", "This field is required");
                    }
                }
                return RedirectToAction("Details", new { id = newInventoryId });
            }
          
                return View(collection);

           
            //}
            //catch
            //{
            //    return View();
            //}
        }
        private void updateNews()
        {
            var news = from i in db.Inventories
                       orderby i.Date descending
                       select new NewsViewModel { InventoryId = i.InventoryId, name = i.Name };
            
            var listnews = news.ToList().Take(5);
            List<NewsViewModel> final = new List<NewsViewModel>();
            foreach (var one in listnews){
                final.Add(one);
            }
            Session["news"] = final;
        }

        [Authorize]
        public ActionResult InventoryIndex()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            var inventories = from i in db.Inventories
                              where i.User.Id == user.Id
                              orderby i.InventoryId descending
                              select new InventoyIndexViewModel { InventoryID = i.InventoryId, Condition = i.Condition, Name = i.Name, Price = i.Price, Quantity = i.Quantity, Status = i.Status };
            return View(inventories.ToList());
        }
        // GET: Inventory/Edit/5
        public ActionResult Edit(int id)
        {
            string[] years = new string[10];

            for (int i = 0; i <= 9; i++)
            {
                string year = "200" + Convert.ToString(i);
                years[i] = year;
            }
            

            string[] status = new string[10];
            status[0] = "On Sale";
            status[1] = "Delivering";
            status[2] = "Locked";
            status[3] = "Sold";
            status[4] = "Returned";
            
            
            var itemQuery = from i in db.Inventories
                             where i.InventoryId == id
                             orderby i.InventoryId
                             select i;
            var item = itemQuery.ToList().First();
            SelectList statusList = new SelectList(status,item.Status);
            ViewBag.Status = statusList;
            SelectList yearList = new SelectList(years, item.Year);
            ViewBag.Year = yearList;
            return View(item);
        }

        // POST: Inventory/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, InventoryView collection)
        {
            try
            {
                string[] years = new string[10];

                for (int i = 0; i <= 9; i++)
                {
                    string year = "200" + Convert.ToString(i);
                    years[i] = year;
                }


                string[] status = new string[10];
                status[0] = "On Sale";
                status[1] = "Delivering";
                status[2] = "Locked";
                status[3] = "Sold";
                status[4] = "Returned";

                SelectList statusList = new SelectList(status, collection.Status);
                ViewBag.Status = statusList;
                SelectList yearList = new SelectList(years, collection.Year);
                ViewBag.Year = yearList;

                if (ModelState.IsValid)
                {
                    var item = db.Inventories.Find(id);
                    item.Brand = collection.Brand;
                    item.Color = collection.Color;
                    item.Condition = collection.Condition;
                    item.Description = collection.Description;
                    item.Material = collection.Material;
                    item.Name = collection.Name;
                    item.Price = collection.Price;
                    item.Quantity = collection.Quantity;
                    item.Status = collection.Status;
                    item.Year = collection.Year;
                    item.Date = DateTime.Now;
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = id });
                }
                else
                    return View(collection);
            }
            catch
            {
                return View();
            }
        }

        // GET: Inventory/Delete/5
        public ActionResult Delete(int id)
        {
            var deleteInvDetails = from i in db.Inventories
                                     where i.InventoryId == id
                                     select i;
            var delete = deleteInvDetails.First();
            db.Inventories.Remove(delete);
            db.SaveChanges( );
            var deleteImage = from m in db.Images
                              where m.InventoryViewId == id
                              select m;
            var image = deleteImage.ToList();
            foreach(var element in image){
                db.Images.Remove(element);
                if (System.IO.File.Exists(element.ImageUrl))
                {
                    System.IO.File.Delete(element.ImageUrl);
                }
                db.SaveChanges();
            }
            ViewBag.message = "Successfully delete the item!";
            return View();
        }

        // POST: Inventory/Delete/5
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
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       