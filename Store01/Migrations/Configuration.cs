namespace Store01.Migrations
{
    using Store01.Enums;
    using Store01.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Store01.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Store01.Models.ApplicationDbContext";
        }

        protected override void Seed(Store01.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.


            var genres = new List<Genre>
            {
                new Genre {Title="Science Fiction"},//1
                new Genre {Title="Fantasy"},//2
                new Genre {Title="Horror Novels"},//3
                new Genre {Title="Children Books"},//4
                new Genre {Title="Crime Novels"},//5
                new Genre {Title="Classic Literature"},//6
                new Genre {Title="History Novels"},//7
                new Genre {Title="Anthologies"},//8
                new Genre {Title="Satire"}//9
            };

            genres.ForEach(s => context.Genres.AddOrUpdate(p => p.Title, s));
            context.SaveChanges();

            var authors = new List<Author>
            {
                new Author {FullName="J.R.R Tolkien", AboutInfo="Short Biography of J.R.R. Tolkien"}, //1
                new Author {FullName="H.P. Lovecraft"},//2
                new Author {FullName="Aldus Huxley"},//3
                new Author {FullName="Frank Herbert"},//4
                new Author {FullName="Arthur Conan Doyle"},//5
                new Author {FullName="Steven Pressfield"},//6
                new Author {FullName="Flann O'Brien", AboutInfo="Short Biography of Flann O'Brien"},//7
                new Author {FullName="Joseph Heller"},//8
                new Author {FullName="G.R.R. Martin"},//9
                new Author {FullName="George Orwell"}//10
            };

            authors.ForEach(s => context.Authors.AddOrUpdate(p => p.FullName, s));
            context.SaveChanges();

            var publishers = new List<Publisher>
            {
                new Publisher {Name = "Simon & Shchuster" },//1 Catch 22
                new Publisher {Name = "Mariner Books" },//2 LOTR vol1
                new Publisher {Name = "HMH Books for Young Readers"},// 3 LOTR vol2
                new Publisher {Name = "Ace"}, //4 Dune vol1
                new Publisher {Name="Dalkey Archive Press"},//5 3rd policemnan
                new Publisher {Name="Bantam"},//6
                new Publisher {Name="Signet Classic"},//7
                new Publisher {Name="Design Studio Press"}//8

            };
            publishers.ForEach(s => context.Publishers.AddOrUpdate(p => p.Name, s));
            context.SaveChanges();


            var books = new List<Book>
            {
                new Book {Title="Lord of the Rings: The Fellowship of the Ring",
                         Isbn="978-0618574940",
                         Code="00000001",
                         Description="The Fellowship of the Ring, the first volume in the LOTR trilogy, tells of the fateful power of the One Ring. It begins a magnificent tale of adventure that will plunge the members of the Fellowship of the Ring into a perilous quest and set the stage for the ultimate clash between the powers of good and evil. ",
                         AuthorId=authors.SingleOrDefault(a=>a.FullName=="J.R.R Tolkien").Id,//1,
                         PublisherId=publishers.SingleOrDefault(p=>p.Name=="Mariner Books").Id,//2,
                         GenreId=genres.SingleOrDefault(g=>g.Title=="Fantasy").Id,//2,
                         Pages=544,
                         Price=10.95m,
                         ReleaseDate=DateTime.Parse("2005-06-01"),
                         Availability = Availability.In_Stock,
                         }, //1
                new Book {Title="Lord of the Rings: The Two Towers",
                         Isbn="978-0358380245",
                         Code="00000002",
                         Description="The Two Towers is the second volume of J.R.R. Tolkien's epic saga, The Lord of the Rings.The Fellowship has been forced to split up. Frodo and Sam must continue alone towards Mount Doom, where the One Ring must be destroyed. Meanwhile, at Helm’s Deep and Isengard, the first great battles of the War of the Ring take shape. ",
                         AuthorId=authors.SingleOrDefault(a=>a.FullName=="J.R.R Tolkien").Id,//1,
                         PublisherId=publishers.SingleOrDefault(p=>p.Name=="HMH Books for Young Readers").Id,//3,
                         GenreId=genres.SingleOrDefault(g=>g.Title=="Fantasy").Id,//2,
                         Pages=448,
                         Price=11.49m,
                         ReleaseDate=DateTime.Parse("2020-10-06"),
                         Availability = Availability.In_Stock,
                         },//2
                new Book {Title="Lord of the Rings: The Return of the King",
                         Isbn="978-0358380252",
                         Code="00000003",
                         Description="The Return of the King is the towering climax to J. R. R. Tolkien’s trilogy that tells the saga of the hobbits of Middle-earth and the great War of the Rings. In this concluding volume, Frodo and Sam make a terrible journey to the heart of the Land of the Shadow in a final reckoning with the power of Sauron. ",
                         AuthorId=authors.SingleOrDefault(a=>a.FullName=="J.R.R Tolkien").Id,//1,
                         PublisherId=publishers.SingleOrDefault(p=>p.Name=="HMH Books for Young Readers").Id,//3,
                         GenreId=genres.SingleOrDefault(g=>g.Title=="Fantasy").Id,//2,
                         Pages=544,
                         Price=11.49m,
                         ReleaseDate=DateTime.Parse("2020-10-06"),
                         Availability = Availability.In_Stock,
                         },//3
                new Book {Title="Dune",
                         Isbn="978-0441013593",
                         Code= "00000004",
                         Description= "Set on the desert planet Arrakis, Dune is the story of the boy Paul Atreides, who would become the mysterious man known as Maud'dib. He would avenge the traitorous plot against his noble family - and would bring to fruition humankind's most ancient and unattainable dream",
                         AuthorId=authors.SingleOrDefault(a=>a.FullName=="Frank Herbert").Id,//4,
                         PublisherId=publishers.SingleOrDefault(p=>p.Name=="Ace").Id,//4,
                         GenreId=genres.SingleOrDefault(g=>g.Title=="Science Fiction").Id,//1,
                         Pages=704,
                         Price=9.49m,
                         ReleaseDate=DateTime.Parse("2005-08-02"),
                         Availability = Availability.In_Stock,
                         },//4
                new Book {Title="Dune Messiah",
                         Isbn="978-0593098233",
                         Code= "00000005",
                         Description= "The epic, multimillion-selling science-fiction series continues! The second Dune installment explores new developments on the planet Arrakis, with its intricate social order and strange, threatening environment. ",
                         AuthorId=authors.SingleOrDefault(a=>a.FullName=="Frank Herbert").Id,//4,
                         PublisherId=publishers.SingleOrDefault(p=>p.Name=="Ace").Id,//4,
                         GenreId=genres.SingleOrDefault(g=>g.Title=="Science Fiction").Id,//1,
                         Pages=352,
                         Price=9.99m,
                         ReleaseDate=DateTime.Parse("2019-06-04"),
                         },//5
                new Book {Title="Children of Dune",
                         Isbn="978-0593201749",
                         Code= "00000006",
                         Description= "The sand-blasted world of Arrakis has become green, watered, and fertile. Old Paul Atreides, who led the desert Fremen to political and religious domination of the galaxy, is gone. But for the children of Dune, the very blossoming of their land contains the seeds of its own destruction.",
                         AuthorId=authors.SingleOrDefault(a=>a.FullName=="Frank Herbert").Id,//4,
                         PublisherId=publishers.SingleOrDefault(p=>p.Name=="Ace").Id,//4,
                         GenreId=genres.SingleOrDefault(g=>g.Title=="Science Fiction").Id,//1,
                         Pages=496,
                         Price=9.59m,
                         ReleaseDate=DateTime.Parse("2020-11-03"),
                         Availability = Availability.In_Stock,
                         },//6 ////mexri edw kala
                 new Book {Title="The Third Policeman",
                          Isbn="978-1564782144",
                          Code= "00000008",
                          Description= "The Third Policeman is Flann O'Brien's brilliantly dark comic novel about the nature of time, death, and existence. ",
                          AuthorId=authors.SingleOrDefault(a=>a.FullName=="Flann O'Brien").Id,//7,
                          PublisherId=publishers.SingleOrDefault(p=>p.Name=="Dalkey Archive Press").Id,//5,
                          GenreId=genres.SingleOrDefault(g=>g.Title=="Satire").Id,//9,
                          Pages=200,
                          Price=7.46m,
                          ReleaseDate=DateTime.Parse("2002-03-01"),
                          Availability = Availability.In_Stock,
                          },//8
                new Book {Title="Catch 22: 50th Anniversary Edition",
                          Isbn="978-1451626650",
                          Code= "00000009",
                          Description="This fiftieth-anniversary edition commemorates Joseph Heller’s masterpiece with a new introduction; critical essays and reviews by Norman Mailer, Alfred Kazin, Anthony Burgess, and others; rare papers and photos; and much more.",
                          AuthorId=authors.SingleOrDefault(a=>a.FullName=="Joseph Heller").Id,//8,
                          PublisherId=publishers.SingleOrDefault(p=>p.Name=="Simon & Shchuster").Id,//1,
                          GenreId=genres.SingleOrDefault(g=>g.Title=="Satire").Id,//9,
                          Pages=544,
                          Price=15.00m,
                          ReleaseDate=DateTime.Parse("2011-04-05"),
                          Availability = Availability.In_Stock,
                          },//9
                new Book {Title="The Gates of Fire",
                          Isbn="978-0553383683",
                          Code= "00000012",
                          Description= "Gates of Fire puts you at the side of valiant Spartan warriors in 480 BC for the bloody, climactic battle at Thermopylae. There, a few hundred of Sparta’s finest sacrificed their lives to hold back the invading Persian millions.",
                          AuthorId=authors.SingleOrDefault(a=>a.FullName=="Steven Pressfield").Id,//6,
                          PublisherId=publishers.SingleOrDefault(p=>p.Name=="Bantam").Id,//6,
                          GenreId=genres.SingleOrDefault(g=>g.Title=="History Novels").Id,//7,
                          Pages=400,
                          Price=14.99m,
                          ReleaseDate=DateTime.Parse("2005-09-25"),
                          Availability = Availability.In_Stock,
                          }, //12
                new Book {Title="A Song of Fire and Ice: A Game of Thrones",
                          Isbn="978-0553386790",
                          Code= "00000010",
                          Description= "NOW THE ACCLAIMED HBO SERIES GAME OF THRONES—THE MASTERPIECE THAT BECAME A CULTURAL PHENOMENON",
                          AuthorId=authors.SingleOrDefault(a=>a.FullName=="G.R.R. Martin").Id,//9,
                          PublisherId=publishers.SingleOrDefault(p=>p.Name=="Bantam").Id,//6,
                          GenreId=genres.SingleOrDefault(g=>g.Title=="Fantasy").Id,//2,
                          Pages=720,
                          Price=8.98m,
                          ReleaseDate=DateTime.Parse("2011-03-22"),
                          Availability = Availability.In_Stock,
                          },//10
                new Book {Title="Nighteen Eighty-Four",
                         Isbn= "978-0679417392",
                         Code= "00000007",
                         Description= "Written more than 70 years ago, 1984 was George Orwell’s chilling prophecy about the future. And while 1984 has come and gone, his dystopian vision of a government that will do anything to control the narrative is timelier than ever...",
                         AuthorId=authors.SingleOrDefault(a=>a.FullName=="George Orwell").Id,//10,
                         PublisherId=publishers.SingleOrDefault(p=>p.Name=="Signet Classic").Id,//7,
                         GenreId=genres.SingleOrDefault(g=>g.Title=="Satire").Id,//9
                         Pages=384,
                         Price=8.44m,
                         ReleaseDate=DateTime.Parse("1992-03-01"),
                         Availability = Availability.In_Stock,
                         },
                new Book {Title = "At the Mountains of Madness",
                          Isbn="978-1624650086",
                          Code="00000013",
                          Description="The story details the events of a disastrous expedition to the Antarctic continent in September 1930, and what was found there by a group of explorers led by the narrator, Dr. William Dyer of Miskatonic University. ",
                          AuthorId=authors.SingleOrDefault(a=>a.FullName=="H.P. Lovecraft").Id,//2,
                          PublisherId=publishers.SingleOrDefault(p=>p.Name=="Design Studio Press").Id,//8,
                          GenreId=genres.SingleOrDefault(g=>g.Title=="Horror Novels").Id,//3,
                          Pages=64,
                          Price=21.78m,
                          ReleaseDate=DateTime.Parse("2020-11-08"),
                          Availability=Availability.In_Stock,
                          }

            };
            books.ForEach(s => context.Books.AddOrUpdate(p => p.Isbn, s));
            context.SaveChanges();


        }
    }
}
