
using System.Text;
using JwtDemo.Models;
using JwtDemo.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace JwtDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Bind JWT settings from configuration
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
            builder.Services.AddSingleton<JwtService>();

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen();

            // Configure JWT authentication
            var jwtSection = builder.Configuration.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSection["Key"]!);

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSection["Issuer"],
                        ValidAudience = jwtSection["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                });

            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.MapGet("/rapidoc", () => Results.Content("""
                    <!doctype html>
                    <html>
                      <head>
                        <title>JwtDemo API</title>
                        <meta charset="utf-8" />
                        <script type="module" src="https://unpkg.com/rapidoc/dist/rapidoc-min.js"></script>
                      </head>
                      <body>
                        <rapi-doc
                          spec-url="/swagger/v1/swagger.json"
                          render-style="read"
                          theme="dark"
                          allow-authentication="true"
                          persist-auth="true">
                        </rapi-doc>
                      </body>
                    </html>
                    """, "text/html")).ExcludeFromDescription();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
