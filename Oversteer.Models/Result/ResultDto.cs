using Oversteer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oversteer.Models.Result
{
    public class ResultDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid RaceId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public SessionType SessionType { get; set; }
        public ACCResult ACCResult { get; set; } = new ACCResult();
    }
}
