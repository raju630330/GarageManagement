using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class ConditionalRequiredAttribute : ValidationAttribute
{
    private readonly string _dependentProperty;
    private readonly object _targetValue;

    public ConditionalRequiredAttribute(string dependentProperty, object targetValue)
    {
        _dependentProperty = dependentProperty;
        _targetValue = targetValue;
        ErrorMessage = "{0} is required.";
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        // Get the value of the dependent property
        PropertyInfo property = validationContext.ObjectType.GetProperty(_dependentProperty);
        if (property == null)
            return new ValidationResult($"Unknown property: {_dependentProperty}");

        var dependentValue = property.GetValue(validationContext.ObjectInstance, null);

        // Check condition
        if (dependentValue != null && dependentValue.ToString() == _targetValue.ToString())
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return new ValidationResult(string.Format(ErrorMessage, validationContext.DisplayName));
            }
        }

        return ValidationResult.Success;
    }
}
