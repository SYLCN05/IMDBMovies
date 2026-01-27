using System.ComponentModel.DataAnnotations;

namespace MovieApp.Server.DTO
{
    public class MovieDTO
    {
        [Required]
        public string Series_Title { get; set; }
        public int Released_Year { get; set; }
        public string Certificate { get; set; }
        public string Runtime { get; set; }
        public string Genre { get; set; }
        public int IMDB_Rating { get; set; }
        public string Overview { get; set; }
        public int Meta_score { get; set; }
        public string Director { get; set; }
        public string Star1 { get; set; }
        public string Star2 { get; set; }
        public string Star3 { get; set; }
        public string Star4 { get; set; }
        public int No_Of_Votes { get; set; }
        public decimal Gross { get; set; }
    }
}
