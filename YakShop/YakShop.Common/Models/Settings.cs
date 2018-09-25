using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YakShop.Common.Models
{
    public class Settings
    {
        public int Id { get; set; }
        public DateTime LoadDate { get; set; }
        public int AgeRule { get; set; }

        public int MaxAge { get; set; }

        public int ElapsedDays { get; set; }

    }
}
