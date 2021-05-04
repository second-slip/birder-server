using Birder.Data.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Birder.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Observation> Observations { get; set; }
        public DbSet<Bird> Birds { get; set; }
        public DbSet<ConservationStatus> ConservationStatuses { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ObservationTag> ObservationTags { get; set; }
        public DbSet<TweetDay> TweetDays { get; set; }
        public DbSet<Network> Network { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<ObservationPosition> ObservationPosition { get; set; }
        public DbSet<ObservationNote> ObservationNotes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Observation>().ToTable("Observation");
            builder.Entity<Bird>().ToTable("Bird");
            builder.Entity<ConservationStatus>().ToTable("ConservationStatus");
            builder.Entity<Tag>().ToTable("Tag");
            builder.Entity<ObservationTag>().ToTable("ObservationTag");
            builder.Entity<ObservationNote>().ToTable("ObservationNote");
            builder.Entity<TweetDay>().ToTable("TweetDay");

            //Add SQL indexes to any property you sort or filter on.
            builder.Entity<Observation>().HasIndex(i => i.ApplicationUserId);
            builder.Entity<Observation>().HasIndex(d => d.ObservationDateTime);
            //builder.Entity<Observation>().HasIndex(p => p.SelectedPrivacyLevel);
            builder.Entity<Observation>().HasIndex(b => b.BirdId);



            //builder.Entity<Photograph>().ToTable("Photograph");

            //builder.Entity<ObservationTag>()
            //        .HasKey(ot => new { ot.TagId, ot.ObervationId });

            //builder.Entity<ObservationTag>()
            //        .HasOne(ot => ot.Observation)
            //        .WithMany(o => o.ObservationTags)
            //        .HasForeignKey(ot => ot.ObervationId);

            //builder.Entity<ObservationTag>()
            //        .HasOne(ot => ot.Tag)
            //        .WithMany(t => t.ObservationTags)
            //        .HasForeignKey(ot => ot.TagId);

            builder.Entity<Network>()
                    .HasKey(k => new { k.ApplicationUserId, k.FollowerId });

            builder.Entity<Network>()
                    .HasOne(l => l.ApplicationUser)
                    .WithMany(a => a.Followers)
                    .HasForeignKey(l => l.ApplicationUserId)
                    .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Network>()
                    .HasOne(l => l.Follower)
                    .WithMany(a => a.Following)
                    .HasForeignKey(l => l.FollowerId)
                    .OnDelete(DeleteBehavior.NoAction);



            builder.Entity<ConservationStatus>().HasData(new ConservationStatus
            {
                ConservationStatusId = 1,
                ConservationList = "Red",
                ConservationListColourCode = "Red",
                Description = "Red is the highest conservation priority, with these species needing urgent action.  Species are placed on the Red-list if they meet one or more of the following criteria: are globally important, have declined historically, show recent severe decline, and have failed to recover from historic decline.",
                CreationDate = new DateTime(2021, 2, 16, 21, 18, 37, 756, DateTimeKind.Local).AddTicks(2143),
                LastUpdateDate =  new DateTime(2021, 2, 16, 21, 18, 37, 756, DateTimeKind.Local).AddTicks(2143)
            });

            builder.Entity<ConservationStatus>().HasData(new ConservationStatus
            {
                ConservationStatusId = 2,
                ConservationList = "Amber",
                ConservationListColourCode = "Yellow",
                Description = "Amber is the second most critical group.  Species are placed on the Amber-list if they meet one or more of these criteria: are important in Europe, show recent moderate decline, show some recovery from historical decline, or occur in internationally important numbers, have a highly localised distribution or are important to the wider UK.",
                CreationDate = new DateTime(2021, 2, 16, 21, 18, 37, 756, DateTimeKind.Local).AddTicks(2143),
                LastUpdateDate = new DateTime(2021, 2, 16, 21, 18, 37, 756, DateTimeKind.Local).AddTicks(2143)
            });

            builder.Entity<ConservationStatus>().HasData(new ConservationStatus
            {
                ConservationStatusId = 3,
                ConservationList = "Green",
                ConservationListColourCode = "Green",
                Description = "Species on the green list are the least critical group.  These are species that occur regularly in the UK but do not qualify under the Red or Amber criteria.",
                CreationDate = new DateTime(2021, 2, 16, 21, 18, 37, 756, DateTimeKind.Local).AddTicks(2143),
                LastUpdateDate = new DateTime(2021, 2, 16, 21, 18, 37, 756, DateTimeKind.Local).AddTicks(2143)
            });

            builder.Entity<ConservationStatus>().HasData(new ConservationStatus
            {
                ConservationStatusId = 4,
                ConservationList = "Former breeder",
                ConservationListColourCode = "Black",
                Description = "This is species is a former breeder and was not was not assessed in the UK Birds of Conservation Concern 4.",
                CreationDate = new DateTime(2021, 2, 16, 21, 18, 37, 756, DateTimeKind.Local).AddTicks(2143),
                LastUpdateDate = new DateTime(2021, 2, 16, 21, 18, 37, 756, DateTimeKind.Local).AddTicks(2143)
            });

            builder.Entity<ConservationStatus>().HasData(new ConservationStatus
            {
                ConservationStatusId = 5,
                ConservationList = "Not assessed",
                ConservationListColourCode = "Black",
                Description = "This species was not assessed in the UK Birds of Conservation Concern 4.",
                CreationDate = new DateTime(2021, 2, 16, 21, 18, 37, 756, DateTimeKind.Local).AddTicks(2143),
                LastUpdateDate = new DateTime(2021, 2, 16, 21, 18, 37, 756, DateTimeKind.Local).AddTicks(2143)
            });


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
