using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using CharacterHub.ViewModels;

namespace CharacterHub.Models
{
    public class CharacterInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<CharacterContext>
    {
        protected override void Seed(CharacterContext context)
        {
            var characters = new List<Character>
            {
                new Character{Id=1, CharacterName="Holo", Likes="Apples and Alcohol", Dislikes="Shepards"}
            };
            characters.ForEach(c => context.Characters.Add(c));
            context.SaveChanges();
            base.Seed(context);
        }
    }
}