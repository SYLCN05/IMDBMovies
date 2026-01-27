using Microsoft.AspNetCore.Mvc;
using MovieApp.Server.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
namespace MovieApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class SeedController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly IWebHostEnvironment _environment;

        public SeedController(ApplicationDBContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        [HttpPut("AddMovieData")]
        [Authorize]
        public async Task<IActionResult> MovieData()
        {
            var config = new CsvConfiguration(CultureInfo.GetCultureInfo("pt-BR"))
            {
                HasHeaderRecord = true,
                Delimiter = ",",
                HeaderValidated = null,
                MissingFieldFound = null

            };

            using var reader = new StreamReader(System.IO.Path.Combine(_environment.ContentRootPath, "Data/imdb_top_1000.csv"));

            using var csv = new CsvReader(reader, config);

            var existingMovies = await _context.Movies.ToDictionaryAsync(m => m.Series_Title);

            var records = csv.GetRecords<Movie>();
            var skippedRows = 0;


            foreach (var record in records) 
            {
                if (!string.IsNullOrEmpty(record.Series_Title) && !existingMovies.ContainsKey(record.Series_Title))
                {
                    var movie = new Movie()
                    {
                        Poster_Link = record.Poster_Link,
                        Series_Title = record.Series_Title,
                        Released_Year = record.Released_Year,
                        Certificate = record.Certificate,
                        Runtime = record.Runtime,
                        Genre = record.Genre,
                        IMDB_Rating = record.IMDB_Rating,
                        Overview = record.Overview,
                        Meta_score = record.Meta_score,
                        Director = record.Director,
                        Star1 = record.Star1,
                        Star2 = record.Star2,
                        Star3 = record.Star3,
                        Star4 = record.Star4,
                        No_of_Votes = record.No_of_Votes,
                      
                    };

                    _context.Movies.Add(movie);
                   


                }
                else
                {
                    skippedRows++;
                }
               
            }
            await _context.SaveChangesAsync();
            return new JsonResult(new
            {
                Movies = _context.Movies.Count(),
                SkippedRows = skippedRows
            });
        }



    }
}
