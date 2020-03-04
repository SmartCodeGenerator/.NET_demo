using AutoMapper;
using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Domain.Interfaces;
using BlackCaviarBank.Infrastructure.Business;
using BlackCaviarBank.Infrastructure.Business.Resources.Mappings;
using BlackCaviarBank.Infrastructure.Business.Resources.ServiceOptions;
using BlackCaviarBank.Infrastructure.Data;
using BlackCaviarBank.Infrastructure.Data.Repositories.Extensions;
using BlackCaviarBank.Infrastructure.Data.UnitOfWork;
using BlackCaviarBank.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.IO;

namespace BlackCaviarBank
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
            services.AddScoped<IRepository<Account>, AccountRepository>();
            services.AddScoped<IRepository<Card>, CardRepository>();
            services.AddScoped<IRepository<Notification>, NotificatiobRepository>();
            services.AddScoped<IRepository<Service>, ServiceRepository>();
            services.AddScoped<IRepository<Transaction>, TransactionRepository>();
            services.AddScoped<IRepository<UserProfile>, UserProfileRepository>();

            services.AddSingleton<UnitOfWork>();

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAdministrationService, AdministrationService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ICardService, CardService>();
            services.AddScoped<INotificationService, BankingNotificationService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IGeneratorService, NumberGeneratorService>();
            services.AddScoped<IRegistrationService, RegistrationService>();
            services.AddScoped<IRolesManagementService, RolesManagementService>();
            services.AddScoped<ISubscriptionService, SubscriptionService>();

            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<UserProfile, IdentityRole>(opts =>
            {
                opts.Password.RequiredLength = 7;
                opts.Password.RequireDigit = true;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
            })
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = true;
                        options.TokenValidationParameters = new TokenValidationParameters()
                        {
                            ValidateIssuer = true,
                            ValidIssuer = AuthOptions.ISSUER,

                            ValidateAudience = true,
                            ValidAudience = AuthOptions.AUDIENCE,

                            ValidateLifetime = true,

                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey()
                        };
                    }
                );

            services.AddControllers()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v2", new OpenApiInfo { Title = "BlackCaviarBank API", Version = "v2" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "MyStaticFiles")),
                RequestPath = "/StaticFiles"
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Black Caviar V1");
            });

            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin();
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
            });

            app.UseHttpsRedirection();

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
