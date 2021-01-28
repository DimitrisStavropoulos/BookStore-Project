using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Store01.Hubs;
using Store01.Models;
using Store01.ViewModels;

namespace Store01.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BooksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Books
        [AllowAnonymous]
        public ActionResult Index(string genre, string author, string publisher, string search, string sortBy, int? page)
        {
            LiveBookIndexViewModel viewModel = new LiveBookIndexViewModel()
            {
                genre = genre == null?"":genre,
                author = author == null ? "" :author,
                publisher = publisher == null ? "" :publisher,
                search = search == null ? "" :search,
                sortBy = sortBy == null ? "" :sortBy,
                page = page == null ? 1 :page

            };
            return View(viewModel);
        }



        // GET: Books/Create
        public ActionResult Create()//ok
        {
            BookViewModel viewModel = new BookViewModel();
            viewModel.ReleaseDate = DateTime.Now;
            viewModel.GenreList = new SelectList(db.Genres, "Id", "Title");
            viewModel.AuthorList = new SelectList(db.Authors, "Id", "FullName");
            viewModel.PublisherList = new SelectList(db.Publishers, "Id", "Name");

            viewModel.ImageLists = new List<SelectList>();
            for (int i = 0; i < Constants.NumberOfProductImages; i++)
            {
                viewModel.ImageLists.Add(new SelectList(db.BookImages, "ID", "FileName"));
            }
            return View(viewModel);
        }









        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookViewModel viewModel)
        {
            Book book = new Book();
            book.AuthorId = viewModel.AuthorId;
            book.Code = viewModel.Code;
            book.Description = viewModel.Description;
            book.GenreId = viewModel.GenreId;
            book.Isbn = viewModel.Isbn;
            book.Pages = viewModel.Pages;
            book.Price = viewModel.Price;
            book.PublisherId = viewModel.PublisherId;
            book.ReleaseDate = viewModel.ReleaseDate;
            book.Title = viewModel.Title;
            book.BookImageMappings = new List<BookImageMapping>();
            string[] bookImages = viewModel.BookImages.Where(c => !string.IsNullOrEmpty(c)).ToArray();
            for (int i = 0; i < bookImages.Length; i++)
            {
                book.BookImageMappings.Add(new BookImageMapping
                {
                    BookImage = db.BookImages.Find(int.Parse(bookImages[i])),
                    ImageNumber = i
                });
            }
            if (ModelState.IsValid)
            {
                db.Books.Add(book);
                db.SaveChanges();
                BooksHub.BroadcastData();
                return RedirectToAction("Index");
            }

            viewModel.AuthorList = new SelectList(db.Authors, "Id", "FullName", book.AuthorId);
            viewModel.GenreList = new SelectList(db.Genres, "Id", "Title", book.GenreId);
            viewModel.PublisherList = new SelectList(db.Publishers, "Id", "Name", book.PublisherId);
            viewModel.ImageLists = new List<SelectList>();
            for (int i = 0; i < Constants.NumberOfProductImages; i++)
            {
                viewModel.ImageLists.Add(new SelectList(db.BookImages, "Id", "FileName", viewModel.BookImages[i]));
            }
            return View(viewModel);
        }

        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }





        // GET: Books/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            BookEditViewModel viewModel = new BookEditViewModel();
            viewModel.ImageLists = new List<SelectList>();
            PopulateAllDropDownLists(book.AuthorId, book.PublisherId, book.GenreId);

            foreach (var item in book.BookImageMappings.OrderBy(c => c.ImageNumber))
            {
                viewModel.ImageLists.Add(new SelectList(db.BookImages, "Id", "FileName", item.BookImageId));
            }

            for (int i = viewModel.ImageLists.Count; i < Constants.NumberOfProductImages; i++)
            {
                viewModel.ImageLists.Add(new SelectList(db.BookImages, "Id", "FileName"));
            }

            viewModel.Id = book.Id;
            viewModel.Code = book.Code;
            viewModel.Title = book.Title;
            viewModel.Description = book.Description;
            viewModel.Isbn = book.Isbn;
            viewModel.ReleaseDate = book.ReleaseDate;
            viewModel.Pages = book.Pages;
            viewModel.Price = book.Price;
            viewModel.Availability = book.Availability;

            return View(viewModel);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BookEditViewModel viewModel)
        {
            var updatedbook = db.Books.Include(c => c.BookImageMappings).Where(c => c.Id == viewModel.Id).Single();
            PopulateAllDropDownLists(updatedbook.AuthorId, updatedbook.PublisherId, updatedbook.GenreId);
            if (TryUpdateModel(updatedbook, "", new string[] { "Code","Title","Description","Isbn",
                "ReleaseDate","Pages","AuthorId","PublisherId","GenreId","Price","Availability"}))
            {
                if (updatedbook.BookImageMappings == null)
                {
                    updatedbook.BookImageMappings = new List<BookImageMapping>();
                }
                string[] bookImages = viewModel.BookImages.Where(c => !string.IsNullOrEmpty(c)).ToArray();

                for (int i = 0; i < bookImages.Length; i++)
                {
                    var imageMappingForEdit = updatedbook.BookImageMappings.Where(c => c.ImageNumber == i).FirstOrDefault();
                    var image = db.BookImages.Find(int.Parse(bookImages[i]));

                    if (imageMappingForEdit == null)
                    {
                        updatedbook.BookImageMappings.Add(new BookImageMapping
                        {
                            ImageNumber = i,
                            BookImage = image,
                            BookImageId = image.Id
                        });
                    }
                    else
                    {
                        if (imageMappingForEdit.BookImageId != int.Parse(bookImages[i]))
                        {
                            imageMappingForEdit.BookImage = image;
                        }
                    }
                }
                for (int i = bookImages.Length; i < Constants.NumberOfProductImages; i++)
                {
                    var imageMappingForEdit = updatedbook.BookImageMappings.Where(c => c.ImageNumber == i).FirstOrDefault();
                    if (imageMappingForEdit != null)
                    {
                        db.BookImageMappings.Remove(imageMappingForEdit);
                    }
                }
                db.SaveChanges();
                BooksHub.BroadcastData();
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }


        // GET: Books/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Books.Find(id);
            var orderLinesCount = db.OrderLines.Where(a => a.BookId == book.Id).Count();
            if (orderLinesCount==0)
            {
                db.Books.Remove(book);
                db.SaveChanges();
                
            }
            else
            {
                db.Entry(book).State = EntityState.Modified;
                book.Availability = Enums.Availability.Unavailable;
                db.SaveChanges();
            }
            BooksHub.BroadcastData();
            return RedirectToAction("Index");

        }

       

        private void PopulateAllDropDownLists(object selectedAuthor = null, object selectedPublisher = null, object selectedGenre = null)
        {
            var authorsQuery = from d in db.Authors
                                   orderby d.FullName
                                   select d;
            var publisherQuery = from d in db.Publishers
                               orderby d.Name
                               select d;
            var genreQuery = from d in db.Genres
                               orderby d.Title
                               select d;
            ViewBag.AuthorId = new SelectList(authorsQuery, "Id", "FullName", selectedAuthor);
            ViewBag.PublisherId = new SelectList(publisherQuery, "Id", "Name", selectedPublisher);
            ViewBag.GenreId = new SelectList(genreQuery, "Id", "Title", selectedGenre);
        }


        [AllowAnonymous]
        public ActionResult GetBooksData(string genre, string author, string publisher, string search, string sortBy, int? page)
        {
            //instantiate a new view model
            BookIndexViewModel viewModel = new BookIndexViewModel();


            //select the books
            var books = db.Books.Include(b => b.Author).Include(b => b.Genre).Include(b => b.Publisher);



            //To search from navbar at any given page to book index
            //also perform the search and save the searchstring to the viewmodel
            if (!String.IsNullOrEmpty(search))
            {
                books = books.Where(p => p.Title.Contains(search) ||
                p.Author.FullName.Contains(search) ||
                p.Publisher.Name.Contains(search) ||
                p.Genre.Title.Contains(search));

                viewModel.Search = search;
                viewModel.Genre = genre;
                viewModel.Author = author;
                viewModel.Publisher = publisher;
            }


            //group search results into genres and count how many items in ech genre
            viewModel.GenresWithCount = from matchingBooks in books
                                        where
                                        matchingBooks.GenreId != null
                                        group matchingBooks by
                                        matchingBooks.Genre.Title into
                                        genreGroup
                                        select new GenreWithCount()
                                        {
                                            GenreTitle = genreGroup.Key,
                                            BookCount = genreGroup.Count()
                                        };

            viewModel.AuthorsWithCount = from matchingBooks in books
                                         where
                                         matchingBooks.AuthorId != null
                                         group matchingBooks by
                                         matchingBooks.Author.FullName into
                                         authorGroup
                                         select new AuthorWithCount()
                                         {
                                             AuthorFullName = authorGroup.Key,
                                             BookCount = authorGroup.Count()
                                         };
            viewModel.PublishersWithCount = from matchingBooks in books
                                            where
                                            matchingBooks.PublisherId != null
                                            group matchingBooks by
                                            matchingBooks.Publisher.Name into
                                            publisherGroup
                                            select new PublisherWithCount()
                                            {
                                                PublisherName = publisherGroup.Key,
                                                BookCount = publisherGroup.Count()
                                            };

            // (1) To navigate from genre index to book index
            //TO DO (?)
            //Apply the same to Author, Publisher (?)
            if (!String.IsNullOrEmpty(genre) && String.IsNullOrEmpty(author) && String.IsNullOrEmpty(publisher))
            {
                books = books.Where(p => p.Genre.Title == genre);
            }
            if (!String.IsNullOrEmpty(author) && String.IsNullOrEmpty(genre) && String.IsNullOrEmpty(publisher))
            {
                books = books.Where(p => p.Author.FullName == author);
            }

            if (!String.IsNullOrEmpty(publisher) && String.IsNullOrEmpty(genre) && String.IsNullOrEmpty(author))
            {
                books = books.Where(p => p.Publisher.Name == publisher);
            }
            //sorting the results
            switch (sortBy)
            {

                case "author_ascending":
                    books = books.OrderBy(n => n.Author.FullName);
                    break;

                case "publisher_ascending":
                    books = books.OrderBy(n => n.Publisher.Name);
                    break;

                case "price_ascending":
                    books = books.OrderBy(n => n.Price);
                    break;

                case "price_descending":
                    books = books.OrderByDescending(n => n.Price);
                    break;

                case "release_date_ascending":
                    books = books.OrderBy(n => n.ReleaseDate);
                    break;

                case "release_date_descending":
                    books = books.OrderByDescending(n => n.ReleaseDate);
                    break;
                default:
                    books = books.OrderBy(n => n.Author.FullName);
                    break;
            }
            //const int PageItems = 3;
            int currentPage = (page ?? 1);
            viewModel.Books = books.ToPagedList(currentPage, Constants.PageItems);
            viewModel.SortBy = sortBy;

            //
            viewModel.Sorts = new Dictionary<string, string>
            {
                {"Author","author_ascending" },
                {"Publisher","publisher_ascending" },
                {"Lowest Price","price_ascending" },
                {"Highest Price","price_descending" },
                {"Oldest Release","release_date_ascending" },
                {"Newest Release","release_date_descending" }
            };

            return PartialView("_LiveUpdate",viewModel);
        }

























        //OBSOLETE

        //public ActionResult EditNew(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Book book = db.Books.Find(id);
        //    if (book == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    PopulateAllDropDownLists(book.AuthorId, book.PublisherId, book.GenreId);
        //    return View(book);
        //}

        //[HttpPost, ActionName("EditNew")]
        //[ValidateAntiForgeryToken]
        //public ActionResult EditNewPost(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    var bookToUpdate = db.Books.Find(id);//Include(a=>a.Author).Include(b=>b.Publisher).Include(c=>c.Genre).Where(i=>i.Id==id).First();
        //    if (TryUpdateModel(bookToUpdate, "",
        //       new string[] { "Code","Title","Description","Isbn",
        //        "ReleaseDate","Pages","AuthorId","PublisherId","GenreId","Price","Availability"}))
        //    {

        //        db.SaveChanges();

        //        return RedirectToAction("Index");


        //    }
        //    PopulateAllDropDownLists(bookToUpdate.AuthorId, bookToUpdate.PublisherId, bookToUpdate.GenreId);
        //    return View(bookToUpdate);
        //}














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
