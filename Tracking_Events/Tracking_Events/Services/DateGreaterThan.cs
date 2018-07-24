using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Tracking_Events.Services
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class DateGreaterThan : ValidationAttribute
    {
        private readonly string _other;

        public DateGreaterThan(string otherProperty)
        {
            _other = otherProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty(_other);
            if (property == null)
            {
                return new ValidationResult(string.Format("Unknown property: {0}", _other));
            }

            //Get other property value
            var other = (DateTime) property.GetValue(validationContext.ObjectInstance);

            //Compare with attached property value with other property value
            if (!((DateTime)value > other))
            {
                return new ValidationResult(ErrorMessage);
            }
            return null;
        }
    }
}
