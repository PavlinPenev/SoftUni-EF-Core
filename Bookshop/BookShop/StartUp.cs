namespace BookShop
{
    using System;
    using Data;
    using Initializer;
    using System.Linq;
    using System.Text;
    using System.Globalization;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            DbInitializer.ResetDatabase(db);

            Console.WriteLine(GetAuthorNamesEndingIn(db, "e"));
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
    }
}
