using Base.Models.Response;
using Microsoft.EntityFrameworkCore;
using Base.Models;

namespace Database
{
    public class MinesweeperContext : DbContext
    {
        public DbSet<GameInfoResponse> GameInfoResponses { get; set; } = null!;

        public DbSet<Position> Positions { get; set; } = null!;

        public MinesweeperContext(DbContextOptions<MinesweeperContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
