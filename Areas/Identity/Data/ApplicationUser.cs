using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Login.Areas.Identity.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    // Common fields
    public string FullName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty; // "Student" or "Teacher"

    // Student-specific fields
    public string RollNo { get; set; } = string.Empty; // Renamed from StudentID
    public int? Class { get; set; }  // Nullable int for Class
    public char? Section { get; set; } // Nullable char for Section
    public DateTime? AdmissionDate { get; set; } // Nullable DateTime for AdmissionDate

    // Teacher-specific fields
    public string EmployeeNo { get; set; } = string.Empty; // Renamed from TeacherID
    public string Specification { get; set; } = string.Empty;
    public DateTime? JoiningDate { get; set; } // Nullable DateTime for JoiningDate

    public bool IsApproved { get; set; } = false; // Default to false
    //public bool IsActive { get; set; } = true;  // Default value is /*true*/


}

