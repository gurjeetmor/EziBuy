using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EziBuy.Models
{
    public class UserDbContext:DbContext
    {
        public DbSet<User> UserContext { get; set; }
        public DbSet<UserRole> UserRoleContext { get; set; }
        public DbSet<ProductCategory> ProductCategoryContext { get; set; }
        public DbSet<ProductDetail> ProductDetailContext { get; set; }
        public DbSet<HomeCarousel> HomeCarouselContext { get; set; }
        public DbSet<ProductImage> ProductImageContext { get; set; }
        public DbSet<ProductDescriptionModel> ProductDescriptionContext { get; set; }
        public DbSet<Size> SizeContext { get; set; }
        //public DbSet<Login> LoginContext { get; set; }
    }
}