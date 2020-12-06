using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Leav_management.Models
{
    public class LeaveTypeVM
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(1, 60, ErrorMessage = "Pleas enter a valid number of dayes")]
        [Display(Name = "Default Number Of Dayes")]
        public int DefaultDays { get; set; }
        [Display(Name = "Created")]
        public DateTime? DateCreated { get; set; }
    }

}
