using AutoMapper;
using StARKS.Application.Courses.Commands;
using StARKS.Application.Courses.Commands;
using StARKS.Application.Courses.Models;
using StARKS.Application.Students.Commands;
using StARKS.Application.Students.Models;
using StARKS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StARKS.Application.Infrastructure
{
    public class OrganizationProfile : Profile
    {
        public OrganizationProfile()
        {
            CreateMap<CreateStudentCommand, Student>().ReverseMap();
            CreateMap<UpdateStudentCommand, Student>().ReverseMap();
            CreateMap<CreateCourseCommand, Course>().ReverseMap();
            CreateMap<UpdateCourseCommand, Course>().ReverseMap();
            CreateMap<CourseDto, Course>().ReverseMap();
        }
    }
}
