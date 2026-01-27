using CsvHelper.Configuration.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MovieApp.Server.Models.Csv
{
    public class MovieRecord
    {
        [Name("Poster_Link")]
        public string? Poster_Link { get; set; }
        [Name("Series_Title")]
        public string? Series_Title { get; set; }
        [Name("Released_Year")]
        public int? Released_Year { get; set; }
        [Name("Certificate")]
        public string? Certificate { get; set; }
        [Name("Runtime")]
        public string? Runtime { get; set; }
        [Name("Genre")]
        public string? Genre { get; set; }
        [Name("IMDB_Rating")]
        public double? IMDB_Rating { get; set; }
        [Name("Overview")]
        public string? Overview { get; set; }
        [Name("Meta_score")]
        public int? Meta_score { get; set; }
        [Name("Director")]
        public string? Director { get; set; }
        [Name("Star1")]
        public string? Star1 { get; set; }
        [Name("Star2")]
        public string? Star2 { get; set; }
        [Name("Star3")]
        public string? Star3 { get; set; }
        [Name("Star4")]
        public string? Star4 { get; set; }
        [Name("No_of_Votes")]
        public int? No_of_Votes { get; set; }
 
    }
}
