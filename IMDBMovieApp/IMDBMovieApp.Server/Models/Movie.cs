using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieApp.Server.Models
{
    [Table("Movies")]
    public class Movie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Poster_Link { get; set; }
        [Required]
        public string? Series_Title { get; set; }
        public string? Released_Year { get; set; }
        public string? Certificate {  get; set; }
        public string? Runtime { get; set; }
        public string? Genre  { get; set; }
        public double? IMDB_Rating { get; set; }
        public string? Overview { get; set; }
        public int? Meta_score  { get; set; }
        public string? Director { get; set; }
        public string? Star1 { get; set; }
        public string? Star2 { get; set; }
        public string? Star3 { get; set; }
        public string? Star4 { get; set; }
        public int? No_of_Votes { get; set; }
       

    }
}
