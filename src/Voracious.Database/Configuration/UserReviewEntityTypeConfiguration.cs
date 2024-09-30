//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//using Voracious.Core.;
//namespace Voracious.Database.Configuration;

//public class UserReviewEntityTypeConfiguration : IEntityTypeConfiguration<UserReview>
//{
//    public void Configure(EntityTypeBuilder<UserReview> builder)
//    {
//        builder
//            .HasKey(u => u.Id);
            
//        builder
//            .ToTable("UserReview");

//        builder
//            .Property(u => u.Id)
//            .IsRequired()
//            .HasComment("An unambiguous reference to the resource within a given context. Recommended practice is to identify the resource by means of a string conforming to an identification system");

//        //builder
//        //    .HasOne(u => u.Book)
//        //    .WithOne(b => b.Review)
//        //    .HasForeignKey<Resource>(b => b.About);

//        builder
//            .Property(u => u.CreateDate)
//            .IsRequired()
//            .HasComment("The create date for the book.");

//        builder
//            .Property(u => u.MostRecentModificationDate)
//            .IsRequired()
//            .HasComment("The most recent modification date for the book.");

//        builder
//            .Property(u => u.Tags)
//            .IsRequired()
//            .HasComment("The tags of the note.");

//        builder
//            .Property(u => u.NStars)
//            .IsRequired()
//            .HasComment("The number of stars for the review.");
//    }
//}