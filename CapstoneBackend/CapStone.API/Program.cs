using CapStone.Application.Repositories;
using CapStone.Application.Services;
using CapStone.API.Middleware;
using CapStone.Infrastructure.Data;
using CapStone.Infrastructure.Repositories;
using CapStone.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using CapStone.Application.Configuration;
using CapStone.Infrastructure.Services.Underwriting;
using System.Text;

namespace CapStone.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<AccidentDbContext>(opt =>
                opt.UseSqlServer(builder.Configuration.GetConnectionString("capStoneDB")));

            builder.Configuration.AddJsonFile("underwriting.json", optional: false, reloadOnChange: true);
            builder.Services.Configure<UnderwritingSettings>(builder.Configuration.GetSection("Underwriting"));

            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IJwtService, JwtService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<IAgentService, AgentService>();
            builder.Services.AddScoped<IClaimService, ClaimService>();
            builder.Services.AddScoped<IPolicyService, PolicyService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            
            // Risk Evaluators
            builder.Services.AddScoped<IRiskEvaluator, AgeEvaluator>();
            builder.Services.AddScoped<IRiskEvaluator, OccupationEvaluator>();
            builder.Services.AddScoped<IRiskEvaluator, HabitEvaluator>();
            builder.Services.AddScoped<IRiskEvaluator, MedicalEvaluator>();

            builder.Services.AddScoped<IUnderwritingService, UnderwritingService>();
            builder.Services.AddScoped<IAssignmentService, AssignmentService>();
            builder.Services.AddHttpClient();
            builder.Services.AddScoped<IChatBotService, ChatBotService>();

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
                });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AngularPolicy", policy =>
                {
                    policy.WithOrigins("http://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            var jwtKey = builder.Configuration["Jwt:Key"];
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey ?? "")),
                        RoleClaimType = System.Security.Claims.ClaimTypes.Role
                    };
                });

            var app = builder.Build();

 
            using (var scope = app.Services.CreateScope())
            {
                AccidentDbSeedData.SeedAsync(scope.ServiceProvider)
                    .GetAwaiter()
                    .GetResult();
            }

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseCors("AngularPolicy");
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}