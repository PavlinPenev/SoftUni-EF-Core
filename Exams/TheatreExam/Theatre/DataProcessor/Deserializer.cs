namespace Theatre.DataProcessor
{
    using AutoMapper;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Theatre.Data;
    using Theatre.Data.Models;
    using Theatre.Data.Models.Enums;
    using Theatre.DataProcessor.ImportDto;

    public class Deserializer
    {
        private static IMapper mapper;

        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfulImportPlay
            = "Successfully imported {0} with genre {1} and a rating of {2}!";

        private const string SuccessfulImportActor
            = "Successfully imported actor {0} as a {1} character!";

        private const string SuccessfulImportTheatre
            = "Successfully imported theatre {0} with #{1} tickets!";

        public static string ImportPlays(TheatreContext context, string xmlString)
        {
            var xmlPlays = XmlDeserializer<List<ImportPlayDto>>("Plays", xmlString);
            var plays = new List<Play>();

            var sb = new StringBuilder();

            foreach (var play in xmlPlays)
            {
                var isEnumValid = Enum.TryParse<Genre>(play.Genre, out Genre genreType);

                var isTimeSpanValid = TimeSpan.Parse(play.Duration).TotalMinutes >= 60;

                if (!IsValid(play) || !isEnumValid || !isTimeSpanValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                plays.Add(new Play
                {
                    Duration = TimeSpan.Parse(play.Duration),
                    Genre = genreType,
                    Description = play.Description,
                    Rating = play.Rating,
                    Screenwriter = play.Screenwriter,
                    Title = play.Title
                });
                sb.AppendLine(String.Format(SuccessfulImportPlay, play.Title, play.Genre, play.Rating));
            }

            context.Plays.AddRange(plays);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportCasts(TheatreContext context, string xmlString)
        {
            var xmlCasts = XmlDeserializer<List<ImportCastDto>>("Casts", xmlString);

            var casts = new List<Cast>();

            var sb = new StringBuilder();

            foreach (var cast in xmlCasts)
            {
                if (!IsValid(cast))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                casts.Add(new Cast
                {
                    PlayId = cast.PlayId,
                    FullName = cast.FullName,
                    IsMainCharacter = cast.IsMainCharacter,
                    PhoneNumber = cast.PhoneNumber
                });
                sb.AppendLine(String.Format(SuccessfulImportActor, cast.FullName, cast.IsMainCharacter ? "main" : "lesser"));
            }

            context.Casts.AddRange(casts);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
        {
            var theatreDtos = JsonConvert.DeserializeObject<List<ImportTheatreDto>>(jsonString);

            var theatres = new List<Theatre>();

            var sb = new StringBuilder();

            foreach (var theatre in theatreDtos)
            {
                if (!IsValid(theatre))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var tickets = new List<Ticket>();

                foreach (var ticket in theatre.Tickets)
                {
                    if (!IsValid(ticket))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    tickets.Add(new Ticket
                    {
                        PlayId = ticket.PlayId,
                        Price = ticket.Price,
                        RowNumber = ticket.RowNumber
                    });
                }

                theatres.Add(new Theatre
                {
                    Director = theatre.Director,
                    Name = theatre.Name,
                    NumberOfHalls = theatre.NumberOfHalls,
                    Tickets = tickets
                });
                sb.AppendLine(String.Format(SuccessfulImportTheatre, theatre.Name, tickets.Count));
            }

            context.AddRange(theatres);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }


        private static bool IsValid(object obj)
        {
            var validator = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
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

        private static void GenerateMapper()
        {
            MapperConfiguration config = new MapperConfiguration(cnfg =>
            {
                cnfg.AddProfile<TheatreProfile>();
            });

            mapper = config.CreateMapper();
        }
        #endregion
    }
}
