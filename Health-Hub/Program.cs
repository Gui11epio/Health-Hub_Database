using Health_Hub.Application.Mapping;
using Health_Hub.Application.Services;
using Health_Hub.Domain.IRepositories;
using System.Text.Json.Serialization;
using Health_Hub.Infrastructure.Context;
using Health_Hub.Infrastructure.Repositories;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Asp.Versioning;
using Microsoft.Extensions.DependencyInjection;
using Health_Hub.Domain.Interfaces;
using Serilog;
using Oracle.ManagedDataAccess.Client;

namespace Health_Hub
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {

                c.EnableAnnotations();

            });


            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<OracleConnection>(sp =>
            {
                var config = sp.GetRequiredService<IConfiguration>();
                var connectionString = config.GetConnectionString("DefaultConnection");
                return new OracleConnection(connectionString);
            });

            builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            builder.Services.AddScoped<UsuarioService>();

            builder.Services.AddScoped<IQuestionarioRepository, QuestionarioRepository>();
            builder.Services.AddScoped<QuestionarioService>();

            builder.Services.AddAuthorization();

            builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Services.AddControllers()
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });



            var app = builder.Build();


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();


            app.Run();
        }
    }
}
