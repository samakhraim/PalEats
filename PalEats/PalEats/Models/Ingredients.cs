using System;
using System.Collections.Generic;
using System.Text;

namespace PalEats.Models
{
    public class Ingredients
    {
        public bool IsChecked { get; set; }
        public float Amount { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public int Id { get; set; }
    }
}
