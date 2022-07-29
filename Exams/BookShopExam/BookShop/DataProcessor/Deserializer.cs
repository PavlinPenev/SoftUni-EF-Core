namespace BookShop.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using BookShop.Data.Models;
    using BookShop.Data.Models.Enums;
    using BookShop.DataProcessor.ImportDto;
    using Data;
    using Newtonsoft.Json;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedBook
            = "Successfully imported book {0} for {1:F2}.";

        private const string SuccessfullyImportedAuthor
            = "Successfully imported author - {0} with {1} books.";

        public static string ImportBooks(BookShopContext context, string xmlString)
        {
            var bookDtos = XmlDeserializer<List<ImportBookDto>>(Constants.BOOKS_XML_ROOT, xmlString);

            var books = new List<Book>();

            var sb = new StringBuilder();

            foreach (var book in bookDtos)
            {
                if (!IsValid(book))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var isDateValid = DateTime.TryParseExact(book.PublishedOn, "MM/dd/yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out DateTime bookPublishedDate);

                if (!isDateValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var currentBook = new Book
                {
                    Name = book.Name,
                    Genre = (Genre)book.Genre,
                    Pages = book.Pages,
                    Price = book.Price,
                    PublishedOn = bookPublishedDate
                };

                books.Add(currentBook);

                sb.AppendLine(String.Format(SuccessfullyImportedBook, currentBook.Name, currentBook.Price));
            }

            context.Books.AddRange(books);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportAuthors(BookShopContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var authorDtos = JsonConvert.DeserializeObject<List<ImportAuthorDto>>(jsonString);

            var existingEmails = context.Authors.Select(a => a.Email).ToHashSet();

            var existingBookIds = context.Books.Select(b => b.Id).ToHashSet();

            var authors = new List<Author>();

            foreach (var author in authorDtos)
            {
                if (!IsValid(author))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (existingEmails.Contains(author.Email))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                existingEmails.Add(author.Email);

                var currentAuthor = new Author
                {
                    FirstName = author.FirstName,
                    LastName = author.LastName,
                    Email = author.Email,
                    Phone = author.Phone,
                };

                foreach (var bookIdDto in author.Books)
                {
                    if (bookIdDto.Id != null && existingBookIds.Contains((int)bookIdDto.Id))
                    {
                        currentAuthor.AuthorsBooks.Add(new AuthorBook
                        {
                            Author = currentAuthor,
                            BookId = (int)bookIdDto.Id
                        });
                    }
                }

                if (!currentAuthor.AuthorsBooks.Any())
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                authors.Add(currentAuthor);
                sb.AppendLine(String.Format(
                    SuccessfullyImportedAuthor,
                    $"{currentAuthor.FirstName} {currentAuthor.LastName}", 
                    currentAuthor.AuthorsBooks.Count
                    ));
            }

            context.AddRange(authors);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }

        #region Deserializer Methods
        private static T XmlDeserializer<T>(string rootTag, string inputXml)
        {
            XmlRootAttribute root = new XmlRootAttribute(rootTag);
            XmlSerializer serializer = new XmlSerializer(typeof(T), root);

            T dtos;

            using (StringReader reader = new StringReader(inputXml))
            {
                dtos = (T)serializer.Deserialize(reader);
            }

            return dtos;
        }
        #endregion
    }
}