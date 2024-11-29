using AutoMapper;
using BusinessLogic.Interfaces;
using BusinessLogic.Repository;
using DataAccess.Context;
using DataAccess.Models;
using EHR.Helper;
using EHR.Profiles;
using Microsoft.EntityFrameworkCore;

namespace EHR
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IPatientRepository, PatientRepository>();
            builder.Services.AddScoped<IAvailableTimeRepositry, AvailableTimeRepository>();
            builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
            builder.Services.AddScoped<IVisitRepository, VisitRepository>();
            builder.Services.AddScoped<IMedicalPrescriptionRepository, MedicalPrescriptionRepository>();
            builder.Services.AddScoped<ILabRepository, LabRepository>();
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IAuthService,AuthService>();
            builder.Services.AddDbContext<EHRdbContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddAutoMapper(M => M.AddProfiles(new List<Profile> 
            { new DoctorProfile(), new PatientProfile(),new VisitProfile() ,new LabTestProfile(),new TimeProfile(),new StaffProfile(),new MedicationProfile()}));
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout duration
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            var app = builder.Build();
            using var Scope= app.Services.CreateScope();
            var Services= Scope.ServiceProvider;
            var LoggerFactory = Scope.ServiceProvider.GetService<ILoggerFactory>();
            try
            {
              var dbContext = Services.GetRequiredService<EHRdbContext>();
              await dbContext.Database.MigrateAsync();
            }
            catch (Exception ex) 
            {
            var logger=LoggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "Error While Applying Migrations");
            }
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession(); 

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=login}/{id?}");

            app.Run();
        }
    }
}