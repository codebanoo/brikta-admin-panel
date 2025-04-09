using Hellang.Middleware.ProblemDetails;
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

namespace APIs.Teniaco
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
            //services.AddProblemDetails();
            base.ConfigureServices(services);
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseProblemDetails();
            base.Configure(app, env);
        }
    }
}
