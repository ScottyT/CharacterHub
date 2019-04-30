using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CharacterHub.Dtos
{
    public class CharacterDetailDto
    {
        public int Id { get; set; }
        public string CharacterName { get; set; }
        public string FirstAppearance { get; set; }
        public string Likes { get; set; }
        public string Dislikes { get; set; }
    }
}