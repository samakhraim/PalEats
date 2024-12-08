using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
namespace PalEats.Models
{
    public class Recipe
    {
        public string Description { get; set; }
        public int DishId { get; set; }
        public string DishName { get; set; }
        public string DueTime { get; set; }
        public string DishImgUrl { get; set; }
        public string Preparation { get; set; }
        public string NumberOfPeople { get; set; }
        public string Calories { get; set; }
        public int CategoryId { get; set; }
    }
}