using Oversteer.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oversteer.Models.Racing
{
    public class Split
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        [NonEmptyGuid(ErrorMessage = "A car class needs to be selected")]
        public Guid CarClassId { get; set; }
        public CarClass? CarClass { get; set; }
        [Range(1, 1000, ErrorMessage = "There needs to be at least one available spot")]
        public int Capacity { get; set; }
        public bool Open { get; set; }
        public bool ManuallySelectedCars { get; set; }
        public List<CarInSplit> CarsInSplit { get; set; } = new List<CarInSplit>();

        [NotMapped]
        public string DisplayName { get; set; } = string.Empty;
    }

    public class CarInSplit
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CarId { get; set; }
        public Car? Car { get; set; }
        public Guid SplitId { get; set; }
        public Split? Split { get; set; }
    }
}
