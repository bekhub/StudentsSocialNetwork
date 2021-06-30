using AutoMapper;
using Core.Entities;
using Core.ObisApiModels;

namespace Api.Endpoints.StudentAccount
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Student, Response.PersonalInformation>(MemberList.None)
                .ForMember(x => x.Fullname,
                    expression => expression.MapFrom(x => $"{x.Firstname} {x.Lastname}"))
                .ForMember(x => x.Email,
                    expression => expression.MapFrom(x => x.User.Email))
                .ForMember(x => x.Username, 
                    expression => expression.MapFrom(x => x.User.UserName))
                .ForMember(x => x.ProfilePictureUrl, 
                    expression => expression.MapFrom(x => x.User.ProfilePictureUrl))
                .ForMember(x => x.Birthdate,
                    expression => expression.MapFrom(x => $"{x.BirthDate:dd.MM.yyyy}"))
                .ForMember(x => x.Department,
                    expression => expression.MapFrom(x => x.Department.Name))
                .ForMember(x => x.Faculty, 
                    expression => expression.MapFrom(x => x.Department.Institute.Name));

            CreateMap<StudentCourse, Response.LessonsAndMarks>(MemberList.None)
                .ForMember(x => x.Code,
                    expression => expression.MapFrom(x => x.Course.Code))
                .ForMember(x => x.Name,
                    expression => expression.MapFrom(x => x.Course.Name))
                .ForMember(x => x.AverageMark,
                    expression => expression.MapFrom(x => x.AverageAssessment));

            CreateMap<Assessment, Response.Mark>(MemberList.None)
                .ForMember(x => x.Name,
                    expression => expression.MapFrom(x => x.Type));

            CreateMap<StudentCourse, StudentTakenLessons.Response>(MemberList.None)
                .ForMember(x => x.Code,
                    expression => expression.MapFrom(x => x.Course.Code))
                .ForMember(x => x.Name,
                    expression => expression.MapFrom(x => x.Course.Name))
                .ForMember(x => x.Credit,
                    expression => expression.MapFrom(x => x.Course.Credits))
                .ForMember(x => x.Theory,
                    expression => expression.MapFrom(x => x.Course.Theory))
                .ForMember(x => x.Practice,
                    expression => expression.MapFrom(x => x.Course.Practice))
                .ForMember(x => x.TheoryAbsent,
                    expression => expression.MapFrom(x => x.TheoryAbsent))
                .ForMember(x => x.PracticeAbsent,
                    expression => expression.MapFrom(x => x.PracticeAbsent));
        }
    }
}
