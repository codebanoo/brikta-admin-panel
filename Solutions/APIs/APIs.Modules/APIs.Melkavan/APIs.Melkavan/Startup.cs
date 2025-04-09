using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIs.Melkavan
{
    public class Startup : APIs.Core.Startup
    {
        public Startup(IHostEnvironment hostingEnvironment/*,
            IConfiguration configuration*/) :
            base(hostingEnvironment/*,
                configuration*/)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            // Add CORS policy
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

      

        }

        
    }
}



#region BEFORE PWA MELKAVAN

//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.HttpsPolicy;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace APIs.Melkavan
//{
//    public class Startup : APIs.Core.Startup
//    {
//        public Startup(IHostEnvironment hostingEnvironment/*,
//            IConfiguration configuration*/) :
//            base(hostingEnvironment/*,
//                configuration*/)
//        {
//        }

//        public override void ConfigureServices(IServiceCollection services)
//        {
//            base.ConfigureServices(services);
//        }
//    }
//}
#endregion


