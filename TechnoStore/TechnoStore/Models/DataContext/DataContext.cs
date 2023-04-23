using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TechnoStore.Models.DataContext
{
    public class DataContext: IdentityDbContext
    {
        public DataContext(DbContextOptions<DataContext> options):base(options) { }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Features> Features { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Favourite> Favourites { get; set; }
        public DbSet<ContactCenter> ContactCenters { get; set; }
        public DbSet<ContactService> ContactServices { get; set; }
        public DbSet<HomePageBanner> HomePageBanners { get; set; }
        public DbSet<ZipCode> ZipCodes { get; set; }
        public DbSet<WidgetItem> WidgetItems { get; set; }
        public DbSet<WidgetTitle> WidgetTitles { get; set; }
        public DbSet<Support> Supports { get; set; }
        public DbSet<OldPassword> OldPasswords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<BasketItem>()
				.HasKey(x => x.Id);

			modelBuilder.Entity<BasketItem>()
				.Property(x => x.IsDeleted)
				.HasDefaultValue(false);

			modelBuilder.Entity<BasketItem>()
				.HasOne(x => x.Product)
				.WithMany()
				.HasForeignKey(x => x.ProductId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<BasketItem>()
				.HasOne(x => x.AppUser)
				.WithMany()
				.HasForeignKey(x => x.AppUserId)
            .OnDelete(DeleteBehavior.Cascade);
        }
		
	}
}
