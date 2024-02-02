using AutoMapper;
using CollegeApp.Data;
using CollegeApp.Models;

namespace CollegeApp.Configurations
{
    public class AUtoMapperConfig : Profile
    {
        public AUtoMapperConfig()
        {
            CreateMap<Student, StudentDto>().ReverseMap();
            CreateMap<Department, DepartmentDto>().ReverseMap();    
        }
    }
}
