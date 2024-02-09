using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oversteer.Validators
{
    public class IntCanBeZeroOrInRange : ValidationAttribute
    {
        private readonly int _minValue;
        private readonly int _maxValue;

        public IntCanBeZeroOrInRange(int minValue, int maxvalue)
        {
            _minValue = minValue;
            _maxValue = maxvalue;
        }

        public string NotSetMessage { get; set; } = string.Empty;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if ((value is int) && ((int)value == 0 || (int)value > _minValue || (int)value < _maxValue))
            {
                return ValidationResult.Success;
            }
            else
            {
                return null;
            }
        }
    }
}