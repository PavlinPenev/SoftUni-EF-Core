namespace Footballers.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Footballers.Data.Models;
    using Footballers.Data.Models.Enums;
    using Footballers.DataProcessor.ImportDto;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCoach
            = "Successfully imported coach - {0} with {1} footballers.";

        private const string SuccessfullyImportedTeam
            = "Successfully imported team - {0} with {1} footballers.";

        public static string ImportCoaches(FootballersContext context, string xmlString)
        {
            var coachDtos = XmlDeserializer<List<ImportCoachDto>>("Coaches", xmlString);

            var coaches = new List<Coach>();

            var sb = new StringBuilder();

            foreach (var coach in coachDtos)
            {
                var footballers = new List<Footballer>();

                if (!IsValid(coach))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                foreach (var footballer in coach.Footballers)
                {
                    var isPositionEnumValid = Enum.TryParse<PositionType>(footballer.PositionType, out PositionType positionType);

                    var isBestSkillEnumValid = Enum.TryParse<BestSkillType>(footballer.PositionType, out BestSkillType bestSkillType);

                    if (!IsValid(footballer) || !isPositionEnumValid || !isBestSkillEnumValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var contractEndDate = DateTime.ParseExact(footballer.ContractEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    var contractStartDate = DateTime.ParseExact(footballer.ContractStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    if (contractEndDate < contractStartDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    footballers.Add(new Footballer
                    {
                        BestSkillType = bestSkillType,
                        PositionType = positionType,
                        ContractEndDate = contractEndDate,
                        ContractStartDate = contractStartDate,
                        Name = footballer.Name
                    });
                }

                coaches.Add(new Coach
                {
                    Name = coach.Name,
                    Nationality = coach.Nationality,
                    Footballers = footballers
                });
                sb.AppendLine(String.Format(SuccessfullyImportedCoach, coach.Name, footballers.Count));
            }

            context.AddRange(coaches);
            context.SaveChanges();  

            return sb.ToString().TrimEnd();
        }
        public static string ImportTeams(FootballersContext context, string jsonString)
        {
            var jsonTeams = JsonConvert.DeserializeObject<List<ImportTeamDto>>(jsonString);

            var existingFootballersIds = context.Footballers.Select(f => f.Id).ToHashSet();

            var teams = new List<Team>();

            var sb = new StringBuilder();

            foreach (var team in jsonTeams)
            {
                if (!IsValid(team) || team.Trophies == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var teamFootballers = new List<TeamFootballer>();

                var currentTeam = new Team
                {
                    Name = team.Name,
                    Nationality = team.Nationality,
                    Trophies = team.Trophies
                };

                foreach (var footballerId in team.Footballers.Distinct())
                {
                    if (!existingFootballersIds.Contains(footballerId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    var currentTeamFootballer = new TeamFootballer
                    {
                        FootballerId = footballerId,
                        Team = currentTeam
                    };

                    currentTeam.TeamsFootballers.Add(currentTeamFootballer);
                }

                teams.Add(currentTeam);
                sb.AppendLine(String.Format(SuccessfullyImportedTeam, currentTeam.Name, currentTeam.TeamsFootballers.Count));
            }

            context.AddRange(teams);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }

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
    }
}
