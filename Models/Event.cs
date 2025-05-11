using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventFind.Models;

public class Event
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Event title is required.")]
    [StringLength(100, ErrorMessage = "Event title cannot exceed 100 characters.")]
    public string Title { get; set; }

    [Required]
    public string Description { get; set; }

    [Required(ErrorMessage = "Event start date and time are required.")]
    [FutureDate(ErrorMessage = "Event start date must be in the future.")]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "Event end date and time are required.")]
    [FutureDate(ErrorMessage = "Event end date must be in the future.")]
    [EndDateAfterStartDate(nameof(StartDate), ErrorMessage = "Event end date must be after the start date.")]
    public DateTime EndDate { get; set; }

    [Required(ErrorMessage = "City is required.")]
    [StringLength(50, ErrorMessage = "City name cannot exceed 50 characters.")]
    public string City { get; set; }
    [Required(ErrorMessage = "Country is required.")]
    [StringLength(50, ErrorMessage = "Country name cannot exceed 50 characters.")]
    public string Country { get; set; }
    [Required(ErrorMessage = "Address is required.")]
    [StringLength(255, ErrorMessage = "Address cannot exceed 255 characters.")]
    public string Address { get; set; }
    [ForeignKey("Category")]
    [Required]
    public int Category_ID { get; set; }    
    public Category Category { get; set; }
    [ForeignKey("User")]
    public string User_ID { get; set; }
    public ApplicationUser User { get; set; }
    [Url(ErrorMessage = "Invalid URL format.")]
    public required string SignupLinkFromCreator { get; set; }
}
public class FutureDateAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is DateTime dateTime)
        {
            if (dateTime < DateTime.Now)
            {
                return new ValidationResult(ErrorMessage ?? "The date must be in the future.");
            }
        }
        return ValidationResult.Success;
    }
}
public class EndDateAfterStartDateAttribute : ValidationAttribute
{
    private readonly string _startDatePropertyName;

    public EndDateAfterStartDateAttribute(string startDatePropertyName)
    {
        _startDatePropertyName = startDatePropertyName;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var instance = validationContext.ObjectInstance;
        var startDateProperty = instance.GetType().GetProperty(_startDatePropertyName);

        if (startDateProperty != null && startDateProperty.GetValue(instance) is DateTime startDate)
        {
            if (value is DateTime endDate && endDate <= startDate)
            {
                return new ValidationResult(ErrorMessage ?? "End date must be after start date.");
            }
        }

        return ValidationResult.Success;
    }
}
