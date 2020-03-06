using AutoMapper;
using Framework.Core.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Framework.Core
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            typeof(IMapperTo<>).Assembly.GetTypes()
                 .Where(i => i.GetInterfaces().Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IMapperTo<>)))
                .ToList().ForEach(item =>
                {
                    item.GetInterfaces()
                        .Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IMapperTo<>))
                        .ToList()
                        .ForEach(i =>
                        {    //这里可以支持多个IMapperTo
                            i.GetGenericArguments().ToList().ForEach(p =>
                            {
                                CreateMap(item, p);
                                CreateMap(p, item);
                            });
                        });
                });
        }
    }
}
