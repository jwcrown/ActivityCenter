using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace BeltExam1.Models
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class DateInTheFutureAttribute : ValidationAttribute
    {
        // Vaildation for future date
        protected override ValidationResult IsValid(object value, ValidationContext validationcontext)
        {
            string futureDate = Convert.ToDateTime(value).ToString("yyyy-MM-dd");
            DateTime validate = DateTime.ParseExact(futureDate, "yyyy-MM-dd",null);
            if (futureDate != null)
            {
                if (validate < DateTime.Now.Date)
                {
                    return new ValidationResult(ErrorMessageString);
                }
                else{
                    return ValidationResult.Success;
                }
            }
            return new ValidationResult(ErrorMessageString);
            
        }
    }
    public class Dashboard
    {
        public List<Activity> Activities {get;set;}
        public User User {get;set;}
        public List<User> Users {get;set;}
    }

    public class Showdata
    {
        public Activity Activity {get;set;}
        public User User {get;set;}
        public List<User> Users {get;set;}
    }
    
    public class ActivityView
    {
        [Required]
        [MinLength(2)]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Title can only contain letters")]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public string Time { get; set; }

        [Required]
        [DateInTheFuture(ErrorMessage = "Date of activity must be in the future")]
        [DataType(DataType.Date)]
        public string Date { get; set; }

        [Required]
        [Display(Name = "Duration (hours)")]
        public decimal Duration { get; set; }

        [Required]
        [MinLength(10)]
        public string Description { get; set; }
    }
}