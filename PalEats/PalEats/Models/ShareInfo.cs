using System;
using System.Collections.Generic;
using System.Text;

namespace PalEats.Models
{
    internal class ShareInfo
    {
        public string DishName { get; set; }
        public string Description { get; set; }
        public List<Ingredients> Ingredients { get; set; }
        public List<string> Preparation { get; set; }
        public string NumberOfPeople { get; set; }

    }
}
