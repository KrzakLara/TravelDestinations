using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BE_TravelDestination.DataPart;
using BE_TravelDestinations.DataPart.Repositories;
using BE_TravelDestinations.DataPart;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Register services needed for JWT token service and user repository
        builder.Services.AddSingleton<JwtTokenService>();
        builder.Services.AddSingleton<IUserRepository, UserRepository>();

        // Register services for destinations repository and service
        builder.Services.AddSingleton<IDestinationsRepository, DestinationsRepository>();
        builder.Services.AddSingleton<IDestinationsServices, DestinationsService>();

        // Configure CORS policy to allow all origins for simplicity here
        // Remember to restrict it in a production environment
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        // Configure controllers and JSON options
        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = null; // Keep original casing
        });

        // Configure JWT Authentication
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero // Optionally reduce the default clock skew if necessary
                };
            });

        // Add authorization
        builder.Services.AddAuthorization();

        // Add Swagger if you wish to use it for API documentation
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage(); // Use the developer exception page in development mode
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseCors("AllowAll"); // Apply the CORS policy
        app.UseAuthentication(); // Ensure authentication middleware is used
        app.UseAuthorization(); // Ensure authorization middleware is used

        app.MapControllers(); // Map routes to controllers

        app.Run(); // Start handling requests
    }
}
