﻿using CollegeApp.Validations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace CollegeApp.Models
{
    public class StudentDto
    {
        public int Id { get; set; }
        public string StudentName { get; set; }
        public string Email { get; set; }   
        public string Address { get; set; }
        public DateTime DOB { get; set; }

    }
}
