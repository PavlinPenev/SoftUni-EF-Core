namespace BookShop
{
    using System;
    using Data;
    using Initializer;
    using System.Linq;
    using System.Text;
    using System.Globalization;
    using Microsoft.EntityFrameworkCore;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            DbInitializer.ResetDatabase(db);

            Console.WriteLine(RemoveBooks(db));
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var books = context.Books
                            .Select(b => new
                            {
                                b.Title,
                                b.AgeRestriction
                            })
                            .ToList()
                            .Where(b => b.AgeRestriction.ToString().ToLower() == command.ToLower())
                            .OrderBy(b => b.Title);

            var sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine(book.Title);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            var goldenBooks = context.Books
                                .Select(b => new
                                {
                                    b.Title,
                                    b.BookId,
                                    b.Copies,
                                    b.EditionType
                                })
                                .ToList()
                                .Where(b => b.Copies < 5000 && b.EditionType == Models.Enums.EditionType.Gold)
                                .OrderBy(b => b.BookId);

            var sb = new StringBuilder();

            foreach (var book in goldenBooks)
            {
                sb.AppendLine(book.Title);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            var books = context.Books
                            .Select(b => new
                            {
                                b.Title,
                                b.Price
                            })
                            .OrderByDescending(b => b.Price)
                            .ToList()
                            .Where(b => b.Price > 40);

            var sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - ${book.Price:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var books = context.Books
                            .Select(b => new
                            {
                                b.Title,
                                b.ReleaseDate,
                                b.BookId
                            })
                            .OrderBy(b => b.BookId)
                            .ToList()
                            .Where(b => b.ReleaseDate.Value.Year != year);

            var sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine(book.Title);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var categories = input.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();

            var books = context.Books
                            .Select(b => new
                            {
                                b.Title,
                                BookCategories = b.BookCategories.Select(bc => bc.Category.Name).ToList()
                            })
                            .OrderBy(b => b.Title)
                            .ToList()
                            .Where(b =>
                            {
                                var inputToLower = categories.Select(c => c.ToLower()).ToList();

                                var categoriesToLower = b.BookCategories.Select(bc => bc.ToLower()).ToList();
                    
                                var intersectedCollection = inputToLower.Intersect(categoriesToLower).ToList();
                                return intersectedCollection.Count > 0;
                            });

            var sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine(book.Title);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var books = context.Books
                            .Select(b => new
                            {
                                b.Title,
                                b.EditionType,
                                b.Price,
                                b.ReleaseDate
                            })
                            .OrderByDescending(b => b.ReleaseDate)
                            .ToList()
                            .Where(b => b.ReleaseDate < DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture));

            var sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - {book.EditionType} - ${book.Price:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authors = context.Authors
                                .Select(b => new
                                {
                                    b.FirstName,
                                    b.LastName
                                })
                                .ToList()
                                .OrderBy(b => $"{b.FirstName} {b.LastName}")
                                .Where(b => b.FirstName.EndsWith(input));

            var sb = new StringBuilder();

            foreach (var author in authors)
            {
                sb.AppendLine($"{author.FirstName} {author.LastName}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var books = context.Books
                            .Select(b => b.Title)
                            .OrderBy(b => b)
                            .ToList()
                            .Where(b => b.Contains(input, StringComparison.InvariantCultureIgnoreCase));

            var sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine(book);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var books = context.Books
                            .Include(b => b.Author)
                            .Select(b => new
                            {
                                b.BookId,
                                b.Title,
                                b.Author.FirstName,
                                b.Author.LastName
                            })
                            .OrderBy(b => b.BookId)
                            .ToList()
                            .Where(b => b.LastName.StartsWith(input, StringComparison.InvariantCultureIgnoreCase));

            var sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} ({book.FirstName} {book.LastName})");
            }

            return sb.ToString().TrimEnd();
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            return context.Books
                            .Select(b => b.Title)
                            .ToList()
                            .Where(b => b.Length > lengthCheck)
                            .Count();               
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var authors = context.Authors
                                .Include(a => a.Books)
                                .Select(a => new
                                {
                                    FullName = $"{a.FirstName} {a.LastName}",
                                    CountOfBookCopies = a.Books.Select(b => b.Copies).Sum()
                                })
                                .OrderByDescending(a => a.CountOfBookCopies)
                                .ToList();

            var sb = new StringBuilder();

            foreach (var author in authors)
            {
                sb.AppendLine($"{author.FullName} - {author.CountOfBookCopies}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var categories = context.Categories
                                .Include(c => c.CategoryBooks)
                                .Select(c => new
                                {
                                    c.Name,
                                    TotalBooksPrice = c.CategoryBooks
                                                        .Select(b => b.Book.Copies * b.Book.Price)
                                                        .Sum()
                                })
                                .OrderByDescending(c => c.TotalBooksPrice)
                                .ThenBy(c => c.Name)
                                .ToList();

            var sb = new StringBuilder();

            foreach (var category in categories)
            {
                sb.AppendLine($"{category.Name} ${category.TotalBooksPrice}");    
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            var categories = context.Categories
                                .Include(c => c.CategoryBooks)
                                .Select(c => new
                                {
                                    c.Name,
                                    Books = c.CategoryBooks
                                        .Select(cb => new
                                        {
                                            cb.Book.Title,
                                            cb.Book.ReleaseDate
                                        })
                                        .OrderByDescending(cb => cb.ReleaseDate)
                                        .Take(3)
                                })
                                .OrderBy(c => c.Name)
                                .ToList();

            var sb = new StringBuilder();

            foreach (var category in categories)
            {
                sb.AppendLine($"--{category.Name}");

                foreach (var book in category.Books)
                {
                    sb.AppendLine($"{book.Title} ({book.ReleaseDate.Value.Year})");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static void IncreasePrices(BookShopContext context)
        {
            var books = context.Books
                            .Where(b => b.ReleaseDate.Value.Year < 2010);

            foreach (var book in books)
            {
                book.Price += 5;
            }

            context.SaveChanges();
        }

        public static int RemoveBooks(BookShopContext context)
        {
            var books = context.Books
                            .Where(b => b.Copies < 4200);

            var booksDeleted = books.Count();

            context.Books.RemoveRange(books);

            context.SaveChanges();

            return booksDeleted;
        }
    }
}
