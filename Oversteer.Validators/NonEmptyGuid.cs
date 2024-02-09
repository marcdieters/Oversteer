using System.ComponentModel.DataAnnotations;

namespace Oversteer.Validators
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NonEmptyGuidAttribute : ValidationAttribute
    {
        public string NotSetMessage { get; set; } = string.Empty;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if ((value is Guid) && Guid.Empty == (Guid)value)
            {
                return new ValidationResult(NotSetMessage);
            }
            return null;
        }
    }
}