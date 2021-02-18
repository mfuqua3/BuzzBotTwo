using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuzzBotTwo.Configuration;
using BuzzBotTwo.Discord;
using BuzzBotTwo.Discord.Services;
using BuzzBotTwo.Domain;
using BuzzBotTwo.Factories;
using BuzzBotTwo.Repository;
using Discord;
using Discord.WebSocket;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BuzzBotTwo
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<DiscordConfiguration>(_configuration.GetSection("Discord"));
            services.AddDbContext<BotContext>(opt => opt.UseSqlite(_configuration.GetConnectionString("DefaultConnection")));
            services.AddDiscordComponents()
                .AddRepositories()
                .AddTransient<ISoftResRaidTemplateFactory, SoftResRaidTemplateFactory>()
                .AddTransient<ITemplateConfigurationService, TemplateConfigurationService>()
                .AddScoped<IUserDataService, UserDataService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment _)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<BotContext>();
                context.Database.Migrate();
            }
            StartClient(app.ApplicationServices);
        }

        private async void StartClient(IServiceProvider services)
        {
            var client = services.GetService<DiscordSocketClient>();
            await services.GetRequiredService<CommandHandlingService>().InitializeAsync(services);

            await client.LoginAsync(TokenType.Bot, _configuration["Discord:Token"]);
            await client.StartAsync();
            await Task.Delay(-1);

        }
    }
}
