namespace CharacterHub.Migrations
{
    using CharacterHub.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CharacterHub.Models.CharacterContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(CharacterHub.Models.CharacterContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            //context.Characters.AddOrUpdate(x => x.Id,
            //    new Character
            //    {
            //        Id = 1,
            //        CharacterName = "Holo",
            //        Likes = "Apples and Alcohol",
            //        Dislikes = "Shepards"
            //    });
            //var characters = new List<Character>
            //{
            //    new Character{Id=1, CharacterName="Holo", Likes="Apples and Alcohol", Dislikes="Shepards"}
            //};
            //characters.ForEach(c => context.Characters.Add(c));
            //context.SaveChanges();
            //base.Seed(context);
        }
    }
}
