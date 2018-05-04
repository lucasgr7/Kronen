using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Kronen.web.Services;
using Kronen.web.Services.Contract;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kronen
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSession();
            services.AddScoped<IGameRoomService, GameRoomService>();
            services.AddMvc()
            .AddSessionStateTempDataProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

            
            app.UseStaticFiles();
            app.UseWebSockets();
            app.UseSession(); 
            app.Use(async (context, next) =>{
            if (context.Request.Path == "/ws")
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    await ChatService.Echo(context, webSocket);
                }
                else
                {
                    context.Response.StatusCode = 400;
                }
            }
            else
            {
                await next();
            }

        });
            app.Use(async (context, next) =>{
            if (context.Request.Path.StartsWithSegments("/ws/room"))
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    long id = long.Parse(context.Request.Path.ToString().Replace("/ws/room/", ""));
                    WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    await ChatService.EchoRoom(context, webSocket, id);
                }
                else
                {
                    context.Response.StatusCode = 400;
                }
            }
            else
            {
                await next();
            }

        });

            app.UseMvc();
            app.UseDeveloperExceptionPage();
            app.UseMvcWithDefaultRoute();
        }

    }
}
