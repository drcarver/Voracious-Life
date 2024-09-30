//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//using Voracious.Core.;

//namespace Voracious.Database.Configuration;

//public class BookNavigationEntityTypeConfiguration : IEntityTypeConfiguration<BookNavigation>
//{
//    public void Configure(EntityTypeBuilder<BookNavigation> builder)
//    {
//        builder
//            .HasKey(n => n.Id);

//        builder
//            .HasAlternateKey(n => new { n.Book,  });

//        builder
//            .ToTable("BookNavigation");

//        builder
//            .Property(n => n.Id)
//            .IsRequired()
//            .HasComment("An unambiguous reference to the resource within a given context.");

//        //builder
//        //    .HasOne(n => n.Book)
//        //    .WithOne(b => b.NavigationData)
//        //    .HasForeignKey<Resource>(b => b.About);

//        builder
//            .Property(n => n.MostRecentNavigationDate)
//            .IsRequired()
//            .HasComment("Last date and time the navigation was used");

//        builder
//            .Property(n => n.NCatalogViews)
//            .IsRequired()
//            .HasComment("Number of catalog views");

//        builder
//            .Property(n => n.NSwipeRight)
//            .IsRequired()
//            .HasComment("Number of swipe rights");

//        builder
//            .Property(n => n.NSwipeLeft)
//            .IsRequired()
//            .HasComment("Number of swipe lefts");

//        builder
//            .Property(n => n.NReading)
//            .IsRequired()
//            .HasComment("Number of time read");

//        builder
//            .Property(n => n.NSpecificSelection)
//            .IsRequired()
//            .HasComment("Number of specific selections");

//        builder
//            .Property(n => n.CurrSpot)
//            .IsRequired()
//            .HasComment("The url of the current spot");

//        builder
//            .Property(n => n.CurrStatus)
//            .IsRequired()
//            .HasComment("The current status");

//        builder
//            .Property(n => n.TimeMarkedDone)
//            .IsRequired()
//            .HasComment("When the book has been read");

//        builder
//            .Property(n => n.FirstNavigationDate)
//            .IsRequired()
//            .HasComment("When the book is started");

//        builder
//            .Property(n => n.IsDone)
//            .IsRequired()
//            .HasComment("When the book done");
//    }
//}