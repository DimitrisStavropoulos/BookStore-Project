using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Store01.Models;
using Stripe;

namespace Store01.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ??
             HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [HttpPost]
        [Obsolete]
        public ActionResult Charge(string stripeToken, string stripeEmail)
        {
            Stripe.StripeConfiguration.SetApiKey("pk_test_51IBkDtCJuzxdYhOtyq9CXeZq4YYCXwlntkpwWloLKsmRJ9RiTKMW0UWriwsfbk1mFIK7IklimZQuDDkQTc2fvfdr00jHEdU28M");
            Stripe.StripeConfiguration.ApiKey = "sk_test_51IBkDtCJuzxdYhOtnAcKUjVmnkHNrG9BvNTd666CsYqe8wm4H4D9VtttOSPb0sst5X6vJhCz2NWpTWSccloeFlfp00zpv1gwFX";

            var myCharge = new Stripe.ChargeCreateOptions();
            // always set these properties
            myCharge.Amount = 500;
            myCharge.Currency = "USD";
            myCharge.ReceiptEmail = stripeEmail;
            myCharge.Description = "Sample Charge";
            myCharge.Source = stripeToken;
            myCharge.Capture = true;
            var chargeService = new Stripe.ChargeService();
            Charge stripeCharge = chargeService.Create(myCharge);
            return View();
        }

        // GET: Orders
        public ActionResult Index(string orderSearch, string startDate, string endDate, string orderSortOrder)
        {
            ViewBag.StripePublishKey = ConfigurationManager.AppSettings["stripePublishableKey"];
     
            var orders = db.Orders.OrderBy(o => o.DateCreated).Include(o => o.OrderLines);

            if (!User.IsInRole("Admin"))
            {
                orders = orders.Where(o => o.UserID == User.Identity.Name);
            }

            if (!String.IsNullOrEmpty(orderSearch))
            {
                orders = orders.Where(o => o.OrderID.ToString().Equals(orderSearch) ||
                 o.UserID.Contains(orderSearch) ||
                 o.DeliveryName.Contains(orderSearch) ||
                 o.DeliveryAddress.AddressLine.Contains(orderSearch) ||
                 o.DeliveryAddress.City.Contains(orderSearch) ||
                 o.DeliveryAddress.Postcode.Contains(orderSearch) ||
                 o.TotalPrice.ToString().Equals(orderSearch) ||
                 o.OrderLines.Any(ol => ol.BookTitle.Contains(orderSearch)));
            }

            DateTime parsedStartDate;
            if (DateTime.TryParse(startDate, out parsedStartDate))
            {
                orders = orders.Where(o => o.DateCreated >= parsedStartDate);
            }

            DateTime parsedEndDate;
            if (DateTime.TryParse(endDate, out parsedEndDate))
            {
                orders = orders.Where(o => o.DateCreated <= parsedEndDate);
            }

            ViewBag.DateSort = String.IsNullOrEmpty(orderSortOrder) ? "date" : "";
            ViewBag.UserSort = orderSortOrder == "user" ? "user_desc" : "user";
            ViewBag.PriceSort = orderSortOrder == "price" ? "price_desc" : "price";
            ViewBag.CurrentOrderSearch = orderSearch;
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;

            switch (orderSortOrder)
            {
                case "user":
                    orders = orders.OrderBy(o => o.UserID);
                    break;
                case "user_desc":
                    orders = orders.OrderByDescending(o => o.UserID);
                    break;
                case "price":
                    orders = orders.OrderBy(o => o.TotalPrice);
                    break;
                case "price_desc":
                    orders = orders.OrderByDescending(o => o.TotalPrice);
                    break;
                case "date":
                    orders = orders.OrderBy(o => o.DateCreated);
                    break;
                default:
                    orders = orders.OrderByDescending(o => o.DateCreated);
                    break;
            }

            return View(orders);
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Order order = db.Orders.Include(o => o.OrderLines).Where(o => o.OrderID == id).SingleOrDefault();
            
            if (order == null)
            {
                return HttpNotFound();
            }
            if (order.UserID ==User.Identity.Name || User.IsInRole("Admin"))
            {
                return View(order);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            
        }

        // GET: Orders/Create
        public async Task<ActionResult> Review()
        {
           
            Basket basket = Basket.GetBasket();
            Models.Order order = new Models.Order();
            order.UserID = User.Identity.Name;
            ApplicationUser user = await UserManager.FindByNameAsync(order.UserID);
            order.DeliveryName = user.FirstName + " " + user.LastName;
            order.DeliveryAddress = user.Address;
            order.OrderLines = new List<OrderLine>();
            foreach (var basketLine in basket.GetBasketLines())
            {
                OrderLine line = new OrderLine
                {
                    Book = basketLine.Book,
                    BookId = basketLine.BookID,
                    BookTitle = basketLine.Book.Title,
                    Quantity = basketLine.Quantity,
                    UnitPrice = basketLine.Book.Price
                };
                order.OrderLines.Add(line);
                order.TotalPrice = basket.GetTotalCost();
            }
            
            return View(order);
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,DeliveryName,DeliveryAddress")] Models.Order order)
        {
            if (ModelState.IsValid)
            {
                order.DateCreated = DateTime.Now;
                db.Orders.Add(order);
                db.SaveChanges();

                //add the orderlines to the database after creating the order
                Basket basket = Basket.GetBasket();
                order.TotalPrice = basket.CreateOrderLines(order.OrderID);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = order.OrderID });
            }
            return RedirectToAction("Review");
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }


        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderID,UserID,DeliveryName,DeliveryAddress,TotalPrice,DateCreated")] Models.Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Models.Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
