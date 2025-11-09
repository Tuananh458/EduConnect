using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using EduConnect.Data;
using EduConnect.Services;
using EduConnect.Hubs;
using EduConnect.Repositories.Interfaces;
using EduConnect.Repositories.Implementations;
using EduConnect.Services.Implementations;
using EduConnect.Services.Interfaces;
using OfficeOpenXml;
using System.ComponentModel;

namespace EduConnect
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // ✅ Cấu hình License EPPlus 7.6.0 (NonCommercial)
            OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;






            // 1️⃣ Add services
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            // ✅ Swagger hỗ trợ JWT Authorize
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Nhập token theo dạng: Bearer {your JWT token}"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        Array.Empty<string>()
                    }
                });
            });

            builder.Services.AddScoped<ILopHocRepository, LopHocRepository>();
            builder.Services.AddScoped<ILopHocService, LopHocService>();

            // Repositories
            builder.Services.AddScoped<IHocLieuRepository, HocLieuRepository>();
            builder.Services.AddScoped<ICauHoiHocLieuRepository, CauHoiHocLieuRepository>();
            builder.Services.AddScoped<IHocLieuCauHoiRepository, HocLieuCauHoiRepository>();
            builder.Services.AddScoped<IHocSinhRepository, HocSinhRepository>();
            


            // Services
            builder.Services.AddScoped<IHocLieuService, HocLieuService>();
            builder.Services.AddScoped<ICauHoiHocLieuService, CauHoiHocLieuService>();
            builder.Services.AddScoped<IHocLieuCauHoiService, HocLieuCauHoiService>();
            builder.Services.AddScoped<IHocSinhService, HocSinhService>();





            // 2️⃣ Custom services
            builder.Services.AddSignalR();

            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IEmailService, EmailService>();

            // 3️⃣ Database
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // 4️⃣ PasswordHasher
            builder.Services.AddScoped<Microsoft.AspNetCore.Identity.IPasswordHasher<object>,
                Microsoft.AspNetCore.Identity.PasswordHasher<object>>();

            // 5️⃣ Authentication (JWT + Google + Microsoft)
            var jwt = builder.Configuration.GetSection("Jwt");
            var auth = builder.Configuration.GetSection("Auth");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwt["Issuer"],

                    ValidateAudience = true,
                    ValidAudience = jwt["Audience"],

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromSeconds(30)
                };
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddGoogle(opt =>
            {
                opt.ClientId = auth["Google:ClientId"]!;
                opt.ClientSecret = auth["Google:ClientSecret"]!;
                opt.CallbackPath = "/external-login/google-callback";
            })
            .AddMicrosoftAccount(opt =>
            {
                opt.ClientId = auth["Microsoft:ClientId"]!;
                opt.ClientSecret = auth["Microsoft:ClientSecret"]!;
                opt.CallbackPath = "/external-login/microsoft-callback";
            });

            builder.Services.AddAuthorization();

            // ✅ 6️⃣ Cấu hình CORS (phải đặt TRƯỚC builder.Build())
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowClient", policy =>
                {
                    policy.WithOrigins("https://localhost:7089") // ✅ cho phép Blazor Client
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });
            builder.Services.AddControllers()
            .AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                // để nếu sau này có tên trùng vẫn ok
                c.CustomSchemaIds(t => t.FullName);
            });

            var app = builder.Build();

            // 7️⃣ Middlewares
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // ✅ Tự tạo thư mục lưu ảnh nếu chưa có
            var uploadDir = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "uploads", "avatars");
            if (!Directory.Exists(uploadDir))
            {
                Directory.CreateDirectory(uploadDir);
            }

            // ✅ Cho phép truy cập file tĩnh và thêm CORS header cho Blazor Client (7089)
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Append("Access-Control-Allow-Origin", "https://localhost:7089");
                    ctx.Context.Response.Headers.Append("Access-Control-Allow-Credentials", "true");
                }
            });

            app.UseCors("AllowClient");
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.MapHub<NotifyHub>("/hub/notify");

            app.Run();

        }
    }
}
