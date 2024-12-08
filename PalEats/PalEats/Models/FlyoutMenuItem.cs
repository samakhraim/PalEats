using System;
using System.Collections.Generic;
using System.Text;

namespace PalEats.Models
{
   public class FlyoutMenuItem
    {
        public string Title { get; set; }
        public string IconSource { get; set; }
        public Type TargetPage { get; set; }

    }
}