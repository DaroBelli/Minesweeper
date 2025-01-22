using Base.Models.Response;
using Microsoft.EntityFrameworkCore;
using Base.Models;

namespace Database
{
    public class MinesweeperContext : DbContext
    {
        public DbSet<GameInfoResponse> GameInfoResponses { get; set; } = null!;

        public DbSet<Position> Positions { get; set; } = null!;

        public MinesweeperContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=minesweeper;Username=postgres;Password=1234");
        }
    }
}
