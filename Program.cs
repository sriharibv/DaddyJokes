using DaddyJokes.Constants;
using DaddyJokes.Data;
using DaddyJokes.Service;
using Microsoft.Extensions.Caching.Memory;

namespace DaddyJokes
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();

            //Injecting memory cache that we use it in the DaddyJokesService
            builder.Services.AddMemoryCache();

            builder.Services.AddSingleton(sp =>
            {
                var memorycache = sp.GetRequiredService<IMemoryCache>();
                var configuration = sp.GetService<IConfiguration>();
                var config = configuration.GetSection(DaddyJokeConstants.ConfigConstants.DaddyJokesConfig).Get<DaddyJokesConfig>();
                return new DaddyJokesService(memorycache, config.ApiUrl);
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

            app.Run();
        }
    }
}