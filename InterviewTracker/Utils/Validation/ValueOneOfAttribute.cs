using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace InterviewTracker.Utils.Validation
{
    public sealed class ValueOneOfAttribute : ValidationAttribute
    {
        public string AllowedValues { get; set; }
        public bool AllowNull { get; set; } = false;   //By default it is not allowed null on properties, if you want specifically then set it on model

        private readonly string _propertyName;

        public ValueOneOfAttribute([CallerMemberName] string propertyName = null)
        {
            _propertyName = propertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            IStringLocalizer<ValueOneOfAttribute> localizer = (IStringLocalizer<ValueOneOfAttribute>)validationContext.GetService(typeof(IStringLocalizer<ValueOneOfAttribute>));

            bool isValid = this.AllowedValues.Split(",").Select(x => x.Trim()).Contains(value?.ToString()) || (AllowNull && value == null);
            if (isValid)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(localizer.GetString("Utils.Validation.ValueOneOf.InvalidValue", value, _propertyName, AllowedValues));
            }
        }
    }
}
