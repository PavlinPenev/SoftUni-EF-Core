namespace MusicHub
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            MusicHubDbContext context = 
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            //Test your solutions here
            Console.WriteLine(ExportSongsAboveDuration(context, 4));
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albums = context.Albums
                            .ToList()
                            .Where(a => a.ProducerId == producerId)
                            .Select(ap => new
                            {
                                ap.Name,
                                ReleaseDate = ap.ReleaseDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                                ProducerName = ap.Producer.Name,
                                AlbumSongs = ap.Songs
                                                .Select(s => new
                                                    {
                                                       SongName = s.Name,
                                                       Price = $"{s.Price:f2}",
                                                       SongWriter = s.Writer.Name
                                                    }
                                                ).OrderByDescending(s => s.SongName)
                                                 .ThenBy(s => s.SongWriter)
                                                 .ToList(),
                                AlbumPrice = ap.Price
                            })
                            .OrderByDescending(a => a.AlbumPrice)
                            .ToList();

            var sb = new StringBuilder();

            foreach (var album in albums)
            {
                var counter = 0;

                sb.AppendLine($"-AlbumName: {album.Name}")
                .AppendLine($"-ReleaseDate: {album.ReleaseDate}")
                .AppendLine($"-ProducerName: {album.ProducerName}")
                .AppendLine($"-Songs:");

                foreach (var song in album.AlbumSongs)
                {
                    sb.AppendLine($"---#{++counter}")
                    .AppendLine($"---SongName: {song.SongName}")
                    .AppendLine($"---Price: {song.Price}")
                    .AppendLine($"---Writer: {song.SongWriter}");
                }

                sb.AppendLine($"-AlbumPrice: {album.AlbumPrice:f2}");

            }

            return sb.ToString().TrimEnd();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var songs = context.Songs
                                .Include(s => s.SongPerformers)
                                .ThenInclude(sp => sp.Performer)
                                .ToList()
                                .Where(s => s.Duration.TotalSeconds > duration)
                                .Select(s => new
                                {
                                    s.Name,
                                    PerformerFullName = s.SongPerformers.Select(sp => $"{sp.Performer.FirstName} {sp.Performer.LastName}").FirstOrDefault(),
                                    WriterName = s.Writer.Name,
                                    AlbumProducer = s.Album.Producer.Name,
                                    s.Duration
                                })
                                .OrderBy(a => a.Name)
                                .ThenBy(a => a.WriterName)
                                .ThenBy(a => a.PerformerFullName)
                                .ToList();

            var sb = new StringBuilder();

            var counter = 0;

            foreach (var song in songs)
            {
                sb.AppendLine($"-Song #{++counter}")
                    .AppendLine($"---SongName: {song.Name}")
                    .AppendLine($"---Writer: {song.WriterName}")
                    .AppendLine($"---Performer: {song.PerformerFullName}")
                    .AppendLine($"---AlbumProducer: {song.AlbumProducer}")
                    .AppendLine($"---Duration: {song.Duration:c}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
