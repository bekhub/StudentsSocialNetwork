using System.ComponentModel.DataAnnotations;

namespace Core.Enums
{
    public enum Weekday
    {
        [Display(Name = "Monday")]
        Monday = 1,
        [Display(Name = "Tuesday")]
        Tuesday,
        [Display(Name = "Wednesday")]
        Wednesday,
        [Display(Name = "Thursday")]
        Thursday,
        [Display(Name = "Friday")]
        Friday,
        [Display(Name = "Saturday")]
        Saturday,
        [Display(Name = "Sunday")]
        Sunday,
    }
}
