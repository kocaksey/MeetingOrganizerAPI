using MeetingOrganizer.Repositories.Concrete;
using MeetingOrganizer.Repository.Abstract;

namespace MeetingOrganizer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // Repository ve di�er servisleri DI'a ekleyin
            builder.Services.AddScoped<IMeetingRepository, MeetingRepository>();

            // Veritaban� ba�lant� zincirini almak i�in konfig�rasyonu kullan�n
            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

            builder.Services.AddControllers();

            // Swagger ve API belgeleri
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // CORS yap�land�rmas�
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder => builder.AllowAnyOrigin()
                                      .AllowAnyMethod()
                                      .AllowAnyHeader());
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // CORS Middleware'ini ekleyin
            app.UseCors("AllowAll");

            app.UseAuthorization();

            app.UseStaticFiles();

            app.MapControllers();

            app.Run();
        }
    }
}
