using System;
using MainService.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MainService.ExceptionHandling;
using MainService.UrlUnderstanding;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;

namespace MainService
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
            services.AddCors();
            // this seems to work to get the class in but... It's not the proper way to do this..
            //services.Configure<Database>(d => d.DbContext = new ShoutDbContext(new DbContextOptions<ShoutDbContext>()));
            var optionsBuilder = new DbContextOptionsBuilder<TamarakDbContext>();
            optionsBuilder.UseMySql(
                    Configuration.GetConnectionString("DefaultConnection"),
                    ServerVersion.AutoDetect(Configuration.GetConnectionString("DefaultConnection"))
            );
            
            //// --- Add services that get instantiated on each call. Controllers can optionally request these services by their type --- ////
            services.AddScoped<IDatabase, Database>(db =>
                new Database(new TamarakDbContext(optionsBuilder.Options), db.GetRequiredService<ILogger<Database>>()));

            services.AddScoped<IUrlProvider, UrlProvider>(sp =>
                new UrlProvider(sp.GetRequiredService<ILogger<UrlProvider>>()));
            
            // this line is necessary for ef tools. DO NOT use the dbContext object directly in Controllers!!!.
            // you could potentially also do this https://docs.microsoft.com/en-us/ef/core/cli/dbcontext-creation?tabs=dotnet-core-cli#from-a-design-time-factory
            services.AddDbContext<TamarakDbContext>(options =>
                {
                    options.EnableSensitiveDataLogging();
                    options.UseMySql(
                        Configuration.GetConnectionString("DefaultConnection"),
                        ServerVersion.AutoDetect(Configuration.GetConnectionString("DefaultConnection"))
                        );
                });
            services.AddControllers(options => 
                options.Filters.Add(new HttpResponseExceptionFilter())
                );
            services.AddCognitoIdentity();
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.Audience = "f7r80la7nk5or5h96es4ru6q1";
                    options.Authority = "https://cognito-idp.us-west-2.amazonaws.com/us-west-2_WSEYP2WhM";
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false, // with cognito there's no audience parameter so nothing to validate.
                    };
                });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MainService", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                //// this just tells you about different endpoints.
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MainService v1"));
            }
            
            // var config = Configuration.GetAWSLoggingConfigSection();
            // loggerFactory.AddAWSProvider(config);

            app.UseCors(builder =>
                builder
                    .WithOrigins(new[]
                    {
                        "http://localhost:3000", "https://main.d3agtx7dckui5h.amplifyapp.com",
                        "https://www.morethanthethought.com", "https://morethanthethought.com", "morethanthethought.com"
                    }) 
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
            );

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
