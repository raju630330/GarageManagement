using GarageManagement.Server;
using GarageManagement.Server.Data;
using GarageManagement.Server.Helpers;
using GarageManagement.Server.RepoInterfaces;
using GarageManagement.Server.Reports.RepoInterfaces;
using GarageManagement.Server.Reports.Repositories;
using GarageManagement.Server.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;


namespace BrilliantMinds.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Email Service
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IJwtUserContext, JwtUserContext>();
            builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("Smtp"));
            builder.Services.AddTransient<EmailService>(); ;
            builder.Services.AddScoped<IAutoCompleteRepository, AutoCompleteRepository>();
            builder.Services.AddScoped<INewJobCardRepsoitory, NewJobCardRepository>();
            builder.Services.AddScoped<IWorkOrderReportRepository, WorkOrderReportRepository>();
            builder.Services.AddScoped<IEstimateReportRepository, EstimateReportRepository>();
            builder.Services.AddScoped<IGatePassReportRepository, GatePassReportRepository>();
            builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
            builder.Services.AddScoped<IRoleRepository, RoleRepository>();
            builder.Services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
            builder.Services.AddScoped<IIssueRepository, IssueRepository>();

            builder.Services.AddSwaggerGen();

            builder.Services.AddCors();

            // JWT
            var jwtKey = builder.Configuration["Jwt:Key"]!;
            var keyBytes = Encoding.UTF8.GetBytes(jwtKey);

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false; // dev only
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        NameClaimType = ClaimTypes.Name,       
                        RoleClaimType = ClaimTypes.Role        
                    };
                });

            builder.Services.AddAuthorization();


            var app = builder.Build();

            app.UseCors(policy => policy
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());

            

            app.UseDefaultFiles();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapStaticAssets();
            app.MapControllers();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
