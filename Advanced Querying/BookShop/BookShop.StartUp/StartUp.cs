using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore;
using BookShop.Data;
using BookShop.Initializer;
using BookShop.Models;

namespace BookShop
{

    public class StartUp
    {
        public static void Main()
        {

            using (var db = new BookShopContext())
            {
                var result = RemoveBooks(db);
                Console.WriteLine($"{result} books were deleted");
            }
        }

        //01.Age Restriction
        public static string GetBooksByAgeRestriction(BookShopContext db, string command)
        {
            int enumValue = -1;

            switch (command.ToLower())
            {
                case "minor":
                    enumValue = 0;
                    break;

                case "teen":
                    enumValue = 1;
                    break;

                case "adult":
                    enumValue = 2;
                    break;
            }

            var titles = db.Books
                .Where(b => b.AgeRestriction == (AgeRestriction)enumValue)
                .Select(b => b.Title).OrderBy(t => t).ToArray();

            var result = String.Join(Environment.NewLine, titles);
            return result;
        }

        //02.Golden Books
        public static string GetGoldenBooks(BookShopContext db)
        {

            var titles = db.Books
                .Where(b => b.EditionType == EditionType.Gold && b.Copies < 5000)
                .Select(b => b.Title)
                .ToArray();

            var result = String.Join(Environment.NewLine, titles);
            return result;
        }

        //03.Books by Price
        public static string GetBooksByPrice(BookShopContext db)
        {
            var sb = new StringBuilder();
            var books = db.Books.Select(b => new
            {
                Title = b.Title.ToString(),
                Price = b.Price
            })
                .Where(b => b.Price > 40m)
                .OrderByDescending(b => b.Price)
                .ToList();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - ${book.Price:F2}");
            }

            return sb.ToString();
        }

        //04.Not Released In
        public static string GetBooksNotRealeasedIn(BookShopContext db, int year)
        {
            var sb = new StringBuilder();
            var booksYears = db.Books.Select(b => new
            {
                BookId = b.BookId,
                Title = b.Title,
                ReleseDate = b.ReleaseDate
            })
            .OrderBy(b => b.BookId)
            .ToList();

            foreach (var book in booksYears)
            {

                if (book.ReleseDate.Value.Year != year)
                {
                    sb.AppendLine(book.Title);
                }
            }

            return sb.ToString();
        }

        //05.Book Titles by Category
        public static string GetBooksByCategory(BookShopContext db, string input)
        {
            var categoryNames = input.ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            var titles = db.Books
                .Where(b => b.BookCategories.Any(c => categoryNames.Contains(c.Category.Name.ToLower())))
                .Select(b => b.Title)
                .OrderBy(t => t)
                .ToArray();

            var result = String.Join(Environment.NewLine, titles);
            return result;
        }

        //06.Released Before Date
        public static string GetBooksReleasedBefore(BookShopContext db, string date)
        {
            var parsedDate = DateTime.ParseExact(date, "dd-MM-yyyy", null);

            var books = db.Books
                .Where(b => b.ReleaseDate < parsedDate)
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => $"{b.Title} - {b.EditionType} - ${b.Price:F2}")
                .ToArray();

            var result = String.Join(Environment.NewLine, books);
            return result;
        }

        //07.Author Search
        public static string GetAuthorNamesEndingIn(BookShopContext db, string args)
        {
            var authors = db.Authors
                .Where(a => a.FirstName.EndsWith(args))
                .OrderBy(a => a.FirstName)
                .ThenBy(a => a.LastName)
                .Select(a => $"{a.FirstName} {a.LastName}")
                .ToArray();

            var result = String.Join(Environment.NewLine, authors);

            return result;
        }

        //08.Book Search
        public static string GetBookTitlesContaining(BookShopContext db, string input)
        {
            var titles = db.Books
                .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToArray();

            var result = String.Join(Environment.NewLine, titles);
            return result;
        }

        //09.Book Search by Author
        public static string GetBooksByAuthor(BookShopContext db, string input)
        {
            var books = db.Books
                .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(b => b.BookId)
                .Select(b => $"{b.Title} ({b.Author.FirstName} {b.Author.LastName})")
                .ToList();

            var result = String.Join(Environment.NewLine, books);

            return result;
        }

        //10.Count Books
        public static int CountBooks(BookShopContext db, int lengthCheck)
        {
            var books = db.Books
                .Where(b => b.Title.Length > lengthCheck)
                .Select(b => b.Title)
                .ToList();

            return books.Count;
        }

        //11.Total Book Copies
        public static string CountCopiesByAuthor(BookShopContext db)
        {
            var sb = new StringBuilder();

            var books = db.Authors
                .Select(a => new
                {
                    a.FirstName,
                    a.LastName,
                    BookCopies = a.Books.Sum(b => b.Copies)
                })
                .OrderByDescending(b => b.BookCopies)
                .ToList();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.FirstName} {book.LastName} - {book.BookCopies}");
            }

            return sb.ToString().Trim();
        }

        //12.Profit by Category
        public static string GetTotalProfitByCategory(BookShopContext db)
        {
            var sb = new StringBuilder();

            var categories = db.Categories.Select(c => new
            {
                Name = c.Name,
                TotalPrice = c.CategoryBooks.Sum(b => b.Book.Price * b.Book.Copies)
            })
            .OrderByDescending(c => c.TotalPrice)
            .ThenBy(c => c.Name)
            .ToList();

            foreach (var cat in categories)
            {
                sb.AppendLine($"{cat.Name} ${cat.TotalPrice}");
            }
            return sb.ToString();
        }

        //13.Most Recent Books
        public static string GetMostRecentBooks(BookShopContext db)
        {
            var sb = new StringBuilder();
            var sortedCategories = db.Categories
                .OrderBy(c => c.Name)
                .Select(c => new
                {
                    Name = c.Name,
                    Books = c.CategoryBooks.Select(b => b.Book)
                        .OrderByDescending(b => b.ReleaseDate).Take(3)
                })
                .ToList();
            foreach (var category in sortedCategories)
            {
                sb.AppendLine($"--{category.Name}");
                foreach (var book in category.Books)
                {
                    sb.AppendLine($"{book.Title} ({book.ReleaseDate.Value.Year})");
                }
            }
            return sb.ToString();
        }

        //14.Increase Prices
        public static void IncreasePrices(BookShopContext db)
        {
            var books = db.Books
                .Where(b => b.ReleaseDate.Value.Year < 2010);

            foreach (var book in books)
            {
                book.Price += 5m;
            }

            db.SaveChanges();
        }

        //15.Remove Books
        public static int RemoveBooks(BookShopContext db)
        {
            var books = db.Books
                .Where(b => b.Copies < 4200);

            int result = books.Count();
            db.RemoveRange(books);
            db.SaveChanges();

            return result;
        }
    }
}