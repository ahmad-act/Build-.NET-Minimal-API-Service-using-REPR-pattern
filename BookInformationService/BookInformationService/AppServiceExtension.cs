using Microsoft.EntityFrameworkCore;
using BookInformationService.DatabaseContext;

namespace BookInformationService;

public static class AppServiceExtension
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope serviceScope = app.ApplicationServices.CreateScope();

        using SystemDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<SystemDbContext>();

        dbContext.Database.Migrate();
    }

}

