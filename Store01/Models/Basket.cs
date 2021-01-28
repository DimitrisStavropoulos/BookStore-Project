using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Store01.Models
{
    public class Basket
    {
        //basket Id can be either a guid or a userId
        private string BasketID { get; set; }
        //work around for anonymous shoppers
        private const string BasketSessionKey = "BasketID";
        private ApplicationDbContext db = new ApplicationDbContext();

        private string GetBasketID()
        {   //if session's basket id is null
            if (HttpContext.Current.Session[BasketSessionKey] == null)
            {
                //if user is logged in set session's basketId to current user's name
                if (!string.IsNullOrWhiteSpace(HttpContext.Current.User.Identity.Name))
                {
                    HttpContext.Current.Session[BasketSessionKey] =
                    HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    //otherwise create Guid and set session basket entry to the GUID
                    //Globally unique identifier (in theory unique) in practice at some point
                    //after a long time you will get a duplicate
                    Guid tempBasketID = Guid.NewGuid();
                    HttpContext.Current.Session[BasketSessionKey] = tempBasketID.ToString();
                }
            }
            //return BasketSessionKey
            return HttpContext.Current.Session[BasketSessionKey].ToString();
        }

        //create basket entity set its BasketId to current session's BasketId
        public static Basket GetBasket()
        {
            Basket basket = new Basket();
            basket.BasketID = basket.GetBasketID();
            return basket;
        }

        public void AddToBasket(int productID, int quantity)
        {
            //fetch basketline of current's session's Basket with given productId
            var basketLine = db.BasketLines.FirstOrDefault(b => b.BasketID == BasketID && b.BookID
             == productID);
            //if no such basketline exists, create it with given product,quantity
            if (basketLine == null)
            {
                basketLine = new BasketLine
                {
                    BookID = productID,
                    BasketID = BasketID,//defaut for each session
                    Quantity = quantity,
                    DateCreated = DateTime.Now
                };
                db.BasketLines.Add(basketLine);
            }
            //else increase its quantity by value provided
            else
            {
                basketLine.Quantity += quantity;
            }
            db.SaveChanges();
        }

        public void RemoveLine(int productID)
        {
            //fetch basketline of current's session's Basket with given productId
            var basketLine = db.BasketLines.FirstOrDefault(b => b.BasketID == BasketID && b.BookID == productID);
            //if it exists, delete it.
            if (basketLine != null)
            {
                db.BasketLines.Remove(basketLine);
            }
            db.SaveChanges();
        }

        public void UpdateBasket(List<BasketLine> lines)
        {
            // accept a list of basketlines as input and loop through them
            foreach (var line in lines)
            {
                //foreach basketline search the database single out the ones with current session's
                //basket Id and restrict to the one with the same bookId
                var basketLine = db.BasketLines.FirstOrDefault(b => b.BasketID == BasketID && b.BookID== line.BookID);
                //if basketline not null
                if (basketLine != null)
                {   //if basketline is empty remove it
                    if (line.Quantity == 0)
                    {
                        RemoveLine(line.BookID);
                    }
                    else
                    {//else update its quantity to input parameter value
                        basketLine.Quantity = line.Quantity;
                    }
                }
                //if session has expired a new empty basket is generated and returned
                //to the user by Basket Class
            }
            db.SaveChanges();
        }
        //Empty current session basket
        public void EmptyBasket()
        {
            var basketLines = db.BasketLines.Where(b => b.BasketID == BasketID);
            foreach (var basketLine in basketLines)
            {
                db.BasketLines.Remove(basketLine);
            }
            db.SaveChanges();
        }

        //get all basket lines of current session basket
        public List<BasketLine> GetBasketLines()
        {
            return db.BasketLines.Where(b => b.BasketID == BasketID).ToList();
        }

        //calculate total cost of current sesion's basket lines
        public decimal GetTotalCost()
        {
            decimal basketTotal = decimal.Zero;

            //fetch all basketlines of current session's basket
            if (GetBasketLines().Count > 0)
            {                                                                   //linq magic.                    
                basketTotal = db.BasketLines.Where(b => b.BasketID == BasketID).Sum(b => b.Book.Price * b.Quantity);
            }

            return basketTotal;
        }

        //get total number of products in current basket
        public int GetNumberOfItems()
        {
            int numberOfItems = 0;
            if (GetBasketLines().Count > 0)
            {
                numberOfItems = db.BasketLines.Where(b => b.BasketID == BasketID).Sum(b => b.Quantity);
            }

            return numberOfItems;
        }

        //migrate the BasketID from a GUID to a username, when a user logs in/registrates.
        public void MigrateBasket(string userName)
        {
            //find the current basket and store it in memory using ToList()
            var basket = db.BasketLines.Where(b => b.BasketID == BasketID).ToList();

            //find if the user already has a basket or not and store it in memory using ToList()
            var usersBasket = db.BasketLines.Where(b => b.BasketID == userName).ToList();

            //if the user has a basket then add the current items to it
            if (usersBasket != null)
            {
                //set the basketID to the username
                string prevID = BasketID; //store guid id
                BasketID = userName; //set current id to username
                //add the lines in anonymous basket to the user's basket
                foreach (var line in basket)
                {
                    AddToBasket(line.BookID, line.Quantity);
                }
                //delete the lines in the anonymous basket from the database
                BasketID = prevID;//reset basket id to guid
                EmptyBasket();//delete all relevant basketlines
                
            }
            else
            {
                //if the user does not have a basket then just migrate this one
                foreach (var basketLine in basket)
                {
                    basketLine.BasketID = userName;
                }

                db.SaveChanges();
            }
            //current session's basket is no longer anonymous.
            HttpContext.Current.Session[BasketSessionKey] = userName;
            
        }


        //use Basket class to get data for Ordelines via GetBasketLines() method
        public decimal CreateOrderLines(int orderID)
        {
            decimal orderTotal = 0;

            var basketLines = GetBasketLines();

            foreach (var item in basketLines)
            {
                OrderLine orderLine = new OrderLine
                {
                    Book = item.Book,
                    BookId = item.BookID,
                    BookTitle = item.Book.Title,
                    Quantity = item.Quantity,
                    UnitPrice = item.Book.Price,
                    OrderID = orderID
                };

                orderTotal += (item.Quantity * item.Book.Price);
                db.OrderLines.Add(orderLine);
            }

            db.SaveChanges();
            EmptyBasket();
            return orderTotal;
        }
    }


}

