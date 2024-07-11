using Microsoft.EntityFrameworkCore;
using RecipieLandAPI.Entity;

namespace RecipieLandAPI.Concrete
{
    public class RecipieContext : DbContext
    {
        public RecipieContext(DbContextOptions<RecipieContext> options)
        : base(options)
        {
        }

        public DbSet<RecipieCategory> RecipieCategories { get; set; }
        public DbSet<Nutritions> Nutritions { get; set; }
        public DbSet<NutritionRecipie> RecipieNutritions { get; set; }
        public DbSet<RecipieSteps> RecipieSteps { get; set; }
        public DbSet<Recipie> Recipies { get; set; }
        public DbSet<UserRecipie> UserRecipies { get; set; }
        public DbSet<UserLikedRecipies> UserLikedRecipies { get; set; }
        public DbSet<UserLikedCategory> UserLikedCategories { get; set; }
        public DbSet<UserFollowing> UserFollowings { get; set; }
        public DbSet<DefaultUser> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Additional configurations can be done here
            modelBuilder.Entity<RecipieCategory>().HasKey(rc => rc.Id);
            modelBuilder.Entity<Nutritions>().HasKey(n => n.Id);
            modelBuilder.Entity<NutritionRecipie>().HasKey(nr => nr.Id);
            modelBuilder.Entity<RecipieSteps>().HasKey(rs => rs.Id);
            modelBuilder.Entity<Recipie>().HasKey(r => r.Id);
            modelBuilder.Entity<UserRecipie>().HasKey(ur => ur.Id);
            modelBuilder.Entity<UserLikedRecipies>().HasKey(ulr => ulr.Id);
            modelBuilder.Entity<UserLikedCategory>().HasKey(ulc => new { ulc.UserId, ulc.CategoryID });
            modelBuilder.Entity<UserFollowing>().HasKey(uf => new { uf.OwnUserId, uf.FollowerId });
            modelBuilder.Entity<DefaultUser>().HasKey(u => u.Id);

            modelBuilder.Entity<Recipie>()
                .HasOne(r => r.Category)
                .WithMany()
                .HasForeignKey(r => r.CategoryID);

            modelBuilder.Entity<Recipie>()
                .HasMany(r => r.UserLikes)
                .WithOne()
                .HasForeignKey(ul => ul.RecipieId);

            modelBuilder.Entity<Recipie>()
                .HasMany(r => r.RecipieSteps)
                .WithOne()
                .HasForeignKey(rs => rs.RecipieId);

            modelBuilder.Entity<NutritionRecipie>()
                .HasOne(nr => nr.Nutrition)
                .WithMany()
                .HasForeignKey(nr => nr.NutritionId);

            modelBuilder.Entity<NutritionRecipie>()
                .HasOne(nr => nr.Recipie)
                .WithMany()
                .HasForeignKey(nr => nr.RecipieId);

            modelBuilder.Entity<DefaultUser>()
                .HasMany(u => u.OwnerRecipies)
                .WithOne()
                .HasForeignKey(ur => ur.UserRecipieId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<DefaultUser>()
                .HasMany(u => u.LikeCategoryUsers)
                .WithOne()
                .HasForeignKey(ulc => ulc.UserId)
                 .OnDelete(DeleteBehavior.NoAction); ;

            modelBuilder.Entity<DefaultUser>()
                .HasMany(u => u.LikedRecipies)
                .WithOne()
                .HasForeignKey(ulr => ulr.UserId)
                 .OnDelete(DeleteBehavior.NoAction); ;

            modelBuilder.Entity<DefaultUser>()
                .HasMany(u => u.Followers)
                .WithOne()
                .HasForeignKey(uf => uf.OwnUserId)
                 .OnDelete(DeleteBehavior.NoAction); ;
        }

    }
}
