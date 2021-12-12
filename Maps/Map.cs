using AutoMapper;
using ModelsForAPI.Models;

namespace EmployeesAPI.Maps
{
    public class Map : Profile
    {
        public Map()
        {
            CreateMap<Department, Department>();
            CreateMap<Employee, Employee>();
            CreateMap<Gender, Gender>();            
        }
    }
}
