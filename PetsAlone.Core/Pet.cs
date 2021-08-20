using System;
using System.ComponentModel.DataAnnotations;

namespace PetsAlone.Core
{
    // pet class
    public class My_Pet_Class
    {
        [Key]
        // the id
        public int Id { get; set; }
        
        // the name
        public string Name { get; set; }
        
        // type
        public int PetType { get; set; } // 1 = Cat, 2 = Dog, 3 = Hamster, 4 = Bird, 5 = Rabbit, 6 = Fish, 7 = Lizard, 8 = Horse, 9 = Gerbil, 10 = Tortoise
        
        // missing since
        public DateTime MissingSince { get; set; }
    }
}