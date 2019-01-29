namespace DataAccess.Migrations
{
    using System;
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
                new AppUser() { Id = 1, Name = "User 1", UserName = "user1", Password = PasswordHasher.Hash("password1") },
                new AppUser() { Id = 2, Name = "User 2", UserName = "user2", Password = PasswordHasher.Hash("password2") }
            );

            context.Posts.AddOrUpdate(x => x.Id,
                new Post() { Id = 1, Body = "Test Post 1", CreatedAt = DateTime.Now, User = context.Users.Find(1) },
                new Post() { Id = 2, Body = "Test Post 2", CreatedAt = DateTime.Now, User = context.Users.Find(2) }
            );

            context.Comments.AddOrUpdate(x => x.Id,
                new Comment() { Id = 1, Body = "Test Comment 1", CreatedAt = DateTime.Now, User = context.Users.Find(2), Post = context.Posts.Find(1) },
                new Comment() { Id = 2, Body = "Test Comment 2", CreatedAt = DateTime.Now, User = context.Users.Find(1), Post = context.Posts.Find(2) }
            );
        }
    }
}
