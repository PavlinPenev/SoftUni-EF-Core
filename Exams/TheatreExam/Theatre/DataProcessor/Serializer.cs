namespace Theatre.DataProcessor
{
    using AutoMapper;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Theatre.Data;
    using Theatre.DataProcessor.ExportDto;

    public class Serializer
    {
        private static IMapper mapper;

        public static string ExportTheatres(TheatreContext context, int numbersOfHalls)
        {
            var theatres = context.Theatres
                .ToList()
                .Where(t => 
                    t.NumberOfHalls >= numbersOfHalls
                    && t.Tickets.Count >= 20
                )
                .Select(t => new 
                {
                    Name = t.Name,
                    Halls = t.NumberOfHalls,
                    TotalIncome = t.Tickets
                        .Where(tick => tick.RowNumber >= 1 && tick.RowNumber <= 5)
                        .Sum(tick => tick.Price),
                    Tickets = t.Tickets.Where(tick => tick.RowNumber >= 1 && tick.RowNumber <= 5).Select(tick => new
                    {
                        Price = tick.Price,
                        RowNumber = tick.RowNumber
                    }).OrderByDescending(tick => tick.Price)
                    .ToList()
                }).OrderByDescending(t => t.Halls)
                .ThenBy(t => t.Name)
                .ToList();

            return JsonConvert.SerializeObject(theatres, Formatting.Indented);
        }

        public static string ExportPlays(TheatreContext context, double rating)
        {
            GenerateMapper();

            var plays = context.Plays
                .ToList()
                .Where(p => p.Rating <= rating)
                .ToList();

            var playDtos = mapper.Map<List<ExportPlayDto>>(plays)
                .OrderBy(p => p.Title)
                .ThenByDescending(p => p.Genre)
                .ToList();

            foreach (var play in playDtos)
            {
                play.Actors = play.Actors.OrderByDescending(a => a.FullName).ToList();
                
                foreach (var actor in play.Actors)
                {
                    actor.MainCharacter = $"Plays main character in '{play.Title}'.";
                }
            }

            return XmlSerializer<List<ExportPlayDto>>(playDtos, "Plays");
        }

        private static void GenerateMapper()
        {
            MapperConfiguration config = new MapperConfiguration(cnfg =>
            {
                cnfg.AddProfile<TheatreProfile>();
            });

            mapper = config.CreateMapper();
        }

        private static string XmlSerializer<T>(T dto, string rootTag)
        {
            var sb = new StringBuilder();

            var root = new XmlRootAttribute(rootTag);
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(String.Empty, String.Empty);

            var serializer = new XmlSerializer(typeof(T), root);

            using (StringWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, dto, namespaces);
            }

            return sb.ToString().TrimEnd();
        }
    }
}
