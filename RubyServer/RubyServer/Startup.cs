using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

using System.IO;

namespace RubyServer
{
    using RubyServer.Models;
    using RubyServer.Controllers;

    public class Startup
    {
        private static DBController dBController;

        public delegate Task RequestDelegate(HttpContext context);

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            //app.UseStaticFiles();
            //app.UseCookiePolicy();
            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Home}/{action=Index}/{id?}");
            //});

            //app.UseMiddleware<TokenMiddleware>();

            app.Map("/Add", AddNote);
            app.Map("/Put", PutNote);
            app.Map("/Notes", GetNotes);
            app.Map("/Auth", Auth);

            dBController = DBController.GetInstance();
        }

        private static void Auth(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                string result = "true";
                await context.Response.WriteAsync(result);
            });
        }

        private static void PutNote(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                using (var reader = new StreamReader(context.Request.Body))
                {
                    bool result = await dBController.UpdateNoteAsync(JsonConvert.DeserializeObject<NoteModel>(await reader.ReadToEndAsync()));

                    if (result)
                    {
                        await context.Response.WriteAsync("true");
                    }

                    await context.Response.WriteAsync("false");
                }
            });
        }

        private static void AddNote(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                using (var reader = new StreamReader(context.Request.Body))
                {
                    bool result = await dBController.AddNoteAsync(JsonConvert.DeserializeObject<NoteModel>(await reader.ReadToEndAsync()));

                    if (result)
                    {
                        await context.Response.WriteAsync("true");
                    }

                    await context.Response.WriteAsync("false");
                }
            });
        }

        private static void GetNotes(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                var obj = await dBController.GetNotesAsync();
                string json = JsonConvert.SerializeObject(obj);
                await context.Response.WriteAsync(json);
            });
        }
    }
}
