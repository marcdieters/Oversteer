using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oversteer.Enums;

namespace Oversteer.Models.Racing
{
    public class Session
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Range(0, 23)]
        public int Hour { get; set; }
        public DayOfTheWeek DayOfTheWeek { get; set; } = DayOfTheWeek.Friday;
        [Range(0, 24)]
        public int TimeMultiplier { get; set; } = 1;
        public SessionType SessionType { get; set; } = SessionType.Practice;
        [Range(5, 1440)]
        public int DurationMinutes { get; set; }
    }
}
