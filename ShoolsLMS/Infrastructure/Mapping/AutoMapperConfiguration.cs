using ShoolsLMS.Models.Data;
using ShoolsLMS.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoolsLMS
{
    public class AutoMapperConfiguration
    {
        public static void RegisterMaps()
        {
            AutoMapper.Mapper.Initialize(config =>
            {
                config.CreateMap<Grade, GradeViewModel>();
                config.CreateMap<GradeViewModel, Grade>();

                config.CreateMap<Subject, SubjectVM>();
                config.CreateMap<SubjectVM, Subject>();

                config.CreateMap<Subject, SeeTeacherVM>()
                    .ForMember(c => c.TeacherUserName,
                        opt => opt.MapFrom(x => x.User.UserName));
            });
        }
    }
}