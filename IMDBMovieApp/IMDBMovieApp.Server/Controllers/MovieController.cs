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
              return  BadRequest("Movie is already inside of your playlist");
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

        [Authorize]
        [HttpGet("get")]
        public async Task<IActionResult> GetUserWatchList()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return NotFound();
            }

            var exists = await _context.UserWatchLists.AnyAsync(uw => uw.UserId == userId);

            if (exists)
            {
                var userWatchlist = await _context.UserWatchLists.Where(uw => uw.UserId == userId).Include(uw => uw.Movie).ToListAsync();
                return Ok(userWatchlist);
            }

            return BadRequest("You dont have a watchlist yet, make sure to add movies to your watchlist");
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteWatchlist()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var watchlistToBeDeleted = await _context.UserWatchLists
                .Where(wl => wl.UserId == userId)
                .FirstAsync();
             
            if(watchlistToBeDeleted != null)
            {
                _context.UserWatchLists.Remove(watchlistToBeDeleted);
                await _context.SaveChangesAsync();
                return Ok("Your playlist has sucessfully been deleted");
            }
            else
            {
                return NotFound("No playlist found to be deleted");
            }

            
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteMovieFromWatchlist(int movieId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if(userId != null)
            {
                var movieToBeDeleted = await _context.UserWatchLists
                    .Where(wl => wl.UserId == userId && wl.MovieId == movieId)
                    .FirstAsync();

                if (movieToBeDeleted != null)
                {
                    _context.UserWatchLists
                        .Remove(movieToBeDeleted);
                    await _context.SaveChangesAsync();

                    return Ok("Movie has been removed from your playlist");
                }
            }
            return BadRequest();
        }
    }
}
