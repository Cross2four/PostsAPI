namespace DataAccess.Migrations
{
    using System.Data.Entity.Migrations;
    using DataAccess.Models;
    using PasswordHasher;

    internal sealed class Configuration : DbMigrationsConfiguration<DataAccess.DataModel>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(DataAccess.DataModel context)
        {
            context.Users.AddOrUpdate(x => x.Id,
                new User() { Id = 1, Name = "User 1", UserName = "user1", Password = PasswordHasher.Hash("password1") },
                new User() { Id = 2, Name = "User 2", UserName = "user2", Password = PasswordHasher.Hash("password2") }
            );
        }
    }
}
