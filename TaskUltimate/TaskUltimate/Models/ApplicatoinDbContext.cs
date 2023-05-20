using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TaskUltimate.Models
{
    public class ApplicatoinDbContext: IdentityDbContext<ApplicationUser>
    {
        public ApplicatoinDbContext(DbContextOptions<ApplicatoinDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Reservation> Reservation { get; set; }
        public virtual DbSet<ReservationDetails> ReservationDetails { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
