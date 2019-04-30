using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using CharacterHub.ViewModels;

namespace CharacterHub.Models
{
	public class CharacterContext : IdentityDbContext<ApplicationUser>
	{
        public CharacterContext() : base("CharacterContext", throwIfV1Schema: false)
        {
            Database.SetInitializer<CharacterContext>(null);
        }
		public DbSet<Character> Characters { get; set; }
		public DbSet<Rating> Ratings { get; set; }
		public DbSet<CharacterRating> CharacterRatings { get; set; }
		public DbSet<Notification> Notifications { get; set; }
		public DbSet<UserNotification> UserNotifications { get; set; }

        public static CharacterContext Create()
        {
            return new CharacterContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
            //modelBuilder.Entity<Notification>()
            //    .HasRequired(a => a.Character)
            //    .WithMany()
            //    .WillCascadeOnDelete(true);

            base.OnModelCreating(modelBuilder);
		}
	}
}