using adstra_task.Models;
using adstra_task.ViewModels;
using AutoMapper;

namespace adstra_task.AutoMapper
{
    public class AutoConfigMapper
    {
        public static IMapper CreateMapper()
        {
            var MappConfig = new MapperConfiguration(x =>
            {
                x.CreateMap<User, UserViewModel>().ForMember(x => x.UserId, z => z.MapFrom(q => q.Id)).ReverseMap();
            });


            return MappConfig.CreateMapper();

        }
    }
}
