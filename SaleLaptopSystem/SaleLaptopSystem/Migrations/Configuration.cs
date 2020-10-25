namespace SaleLaptopSystem.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SaleLaptopSystem.DAL.SaleLaptopSystemContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "SaleLaptopSystem.DAL.SaleLaptopSystemContext";
        }

        protected override void Seed(SaleLaptopSystem.DAL.SaleLaptopSystemContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
