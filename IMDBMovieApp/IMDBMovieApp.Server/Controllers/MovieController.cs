using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApp.Server.Models;
using System.Security.Claims;

namespace MovieApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class MovieController : Controller
    {
        private ApplicationDBContext _context;
        public MovieController(ApplicationDBContext context)
        {
            _context = context;
        }
        [HttpGet("GetMovies")]
        public async Task<IEnumerable<Movie>> GetMovies(int pageIndex=0, int pageSize=10)
        {
            if(pageIndex >=0 && pageIndex <= 99)
            {
                return await _context.Movies.Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
            }
            else
            {
                return new List<Movie>();
            }
            
        }
        [HttpGet("GetMovie")]
        public async Task<IActionResult> GetMovie(int movieId)
        {
            if (movieId >=1 && movieId <= 1000 )
            {
                var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == movieId);

                return Ok(movie);
            }
            else
            {
                return BadRequest("Invalid ID");
            }
           
        }
        [Authorize]
        [HttpPost("add/{movieId}")]
        public async Task<IActionResult> AddMovieToWatchList(int movieId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            var exists = await _context.UserWatchLists
                .AnyAsync(uw => uw.UserId == userId && uw.MovieId == movieId);

            if (exists)
            {
              return  BadRequest("Film staat al in je lijst");
            }

            var watchListItem = new UserWatchList
            {
                UserId = userId,
                MovieId = movieId,

            };

            _context.UserWatchLists.Add(watchListItem);
            await _context.SaveChangesAsync();

            return Ok("Added the Movie to the watchlist");
        }
    }
}
