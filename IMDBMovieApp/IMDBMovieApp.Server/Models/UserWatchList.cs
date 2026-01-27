using Microsoft.AspNetCore.Identity;

namespace MovieApp.Server.Models
{
    public class UserWatchList
    {
        public string UserId { get; set; }
        public MovieUser User { get; set; }

        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}
