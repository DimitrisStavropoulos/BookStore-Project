using PagedList;
using Store01.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Store01.ViewModels
{
    public class BookIndexViewModel
    {

        public IPagedList<Book> Books { get; set; }
        public string Search { get; set; }
        public IEnumerable<GenreWithCount> GenresWithCount { get; set; }
        public IEnumerable<PublisherWithCount> PublishersWithCount { get; set; }
        public IEnumerable<AuthorWithCount> AuthorsWithCount { get; set; }
        public string Genre { get; set; }
        public string Author { get; set; }

        public string Publisher { get; set; }

        public string SortBy { get; set; }
        public Dictionary<string, string> Sorts { get; set; }

        public IEnumerable<SelectListItem> GenreFilterItems
        {
            get
            {
                var allGenres = GenresWithCount.Select(gc => new SelectListItem
                {
                    Value = gc.GenreTitle,
                    Text = gc.GenreTitleWithCount
                });

                return allGenres;
            }
        }

        public IEnumerable<SelectListItem> AuthorFilterItems
        {
            get
            {
                var allAuthors = AuthorsWithCount.Select(ac => new SelectListItem
                {
                    Value = ac.AuthorFullName,
                    Text = ac.AuthorFullNameWithCount
                });

                return allAuthors;
            }
        }

        public IEnumerable<SelectListItem> PublisherFilterItems
        {
            get
            {
                var allPublishers = PublishersWithCount.Select(ac => new SelectListItem
                {
                    Value = ac.PublisherName,
                    Text = ac.PublisherNameWithCount
                });

                return allPublishers;
            }
        }


    }
    public class GenreWithCount
    {
        public int BookCount { get; set; }
        public string GenreTitle { get; set; }

        public string GenreTitleWithCount
        {
            get
            {
                return GenreTitle + " (" + BookCount.ToString() + ")";
            }
        }

    }

    public class AuthorWithCount
    {
        public int BookCount { get; set; }
        public string AuthorFullName { get; set; }

        public string AuthorFullNameWithCount
        {
            get
            {
                return AuthorFullName + " (" + BookCount.ToString() + ")";
            }
        }

    }

    public class PublisherWithCount
    {
        public int BookCount { get; set; }
        public string PublisherName { get; set; }

        public string PublisherNameWithCount
        {
            get
            {
                return PublisherName + " (" + BookCount.ToString() + ")";
            }
        }

    }


}