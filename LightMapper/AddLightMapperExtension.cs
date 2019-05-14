using LightMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AddLightMapperExtension
    {
        public static void AddLightMapper(this IServiceCollection services)
        {
            services.AddSingleton<IMapper,Mapper>();
        }
    }
}
