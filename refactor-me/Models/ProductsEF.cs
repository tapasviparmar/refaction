namespace refactor_me.Models
{
    using System.Data.Entity;

    public partial class ProductsEF : DbContext
    {
        public ProductsEF()
            : base("name=ProductsEF")
        {
        }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductOption> ProductOptions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
