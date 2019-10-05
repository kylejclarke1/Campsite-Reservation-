using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Site
    {
        public int Id { get; set; }
        public int CampgroundId { get; set; }
        public int SiteNumber { get; set; }
        public int MaxOcc { get; set; }
        public bool Accessible { get; set; }
        public int RVLength { get; set; }
        public bool Utilities { get; set; }
        public double Cost { get; set; }
        public double totalCost { get; set; }
    }
}
