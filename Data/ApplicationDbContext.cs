using Birder.Data.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Birder.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Observation> Observations { get; set; }
        public DbSet<Bird> Birds { get; set; }
        public DbSet<ConserverationStatus> ConservationStatuses { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ObservationTag> ObservationTags { get; set; }
        public DbSet<TweetDay> TweetDays { get; set; }
        public DbSet<Network> Network { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            // New method for adding iniital data
            //builder.Entity<NotificationTemplate>().HasData(new NotificationTemplate { Id = 1, Name = "Test", Body = "HTML<>", CreatedDate = DateTime.Now, IsInactive = false });

            base.OnModelCreating(builder);
            builder.Entity<Observation>().ToTable("Observation");
            builder.Entity<Bird>().ToTable("Bird");
            builder.Entity<ConserverationStatus>().ToTable("ConservationStatus");
            //builder.Entity<BritishStatus>().ToTable("BritishStatus");
            builder.Entity<Tag>().ToTable("Tag");
            builder.Entity<ObservationTag>().ToTable("ObservationTag");
            builder.Entity<TweetDay>().ToTable("TweetDay");
            //builder.Entity<Photograph>().ToTable("Photograph");

            builder.Entity<ObservationTag>()
                    .HasKey(ot => new { ot.TagId, ot.ObervationId });

            builder.Entity<ObservationTag>()
                    .HasOne(ot => ot.Observation)
                    .WithMany(o => o.ObservationTags)
                    .HasForeignKey(ot => ot.ObervationId);

            builder.Entity<ObservationTag>()
                    .HasOne(ot => ot.Tag)
                    .WithMany(t => t.ObservationTags)
                    .HasForeignKey(ot => ot.TagId);

            builder.Entity<Network>()
                .HasKey(k => new { k.ApplicationUserId, k.FollowerId });

            builder.Entity<Network>()
                .HasOne(l => l.ApplicationUser)
                .WithMany(a => a.Followers)
                .HasForeignKey(l => l.ApplicationUserId);

            builder.Entity<Network>()
                .HasOne(l => l.Follower)
                .WithMany(a => a.Following)
                .HasForeignKey(l => l.FollowerId);


            //builder.Entity<NotificationTemplate>().HasData(new NotificationTemplate { Id = 1, Name = "Test", Body = "HTML<>", CreatedDate = DateTime.Now, IsInactive = false });

            //builder.Entity<ConserverationStatus>().HasData(new ConserverationStatus { ConserverationStatusId = 1, ConservationStatus = "Red", Description = "", CreationDate = DateTime.Now, LastUpdateDate = DateTime.Now });
            //builder.Entity<ConserverationStatus>().HasData(new ConserverationStatus { ConserverationStatusId = 2, ConservationStatus = "Amber", Description = "", CreationDate = DateTime.Now, LastUpdateDate = DateTime.Now });
            //builder.Entity<ConserverationStatus>().HasData(new ConserverationStatus { ConserverationStatusId = 3, ConservationStatus = "Green", Description = "", CreationDate = DateTime.Now, LastUpdateDate = DateTime.Now });

            //builder.Entity<Bird>().HasData(new Bird {  });
            // Note on first migration to change cascade delete in migration file to 'NoAction'

            //migrationBuilder.CreateTable(
            //  ....
            //            table.ForeignKey(
            //                name: "FK_Network_AspNetUsers_ApplicationUserId",
            //                column: x => x.ApplicationUserId,
            //                principalTable: "AspNetUsers",
            //                principalColumn: "Id",
            //                onDelete: ReferentialAction.Cascade);   <-- change to ReferentialAction.NoAction
            //            table.ForeignKey(
            //                name: "FK_Network_AspNetUsers_FollowerId",
            //                column: x => x.FollowerId,
            //                principalTable: "AspNetUsers",
            //                principalColumn: "Id",
            //                onDelete: ReferentialAction.Cascade);  <-- change to ReferentialAction.NoAction
            //        });
        }
    }
}
