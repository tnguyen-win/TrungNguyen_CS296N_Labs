using PopularGameEngines.Data;
using PopularGameEngines.Models;
using Microsoft.AspNetCore.Identity;

namespace PopularGameEngines
{
    public class SeedData
    {
        public static void Seed(AppDbContext context, IServiceProvider provider)
        {
            if (context.Messages != null && !context.Messages.Any())
            {
                var userManager = provider.GetRequiredService<UserManager<AppUser>>();
                var user1 = new AppUser { Name = "John Smith", UserName = "John" };
                var user2 = new AppUser { Name = "Jane Doe", UserName = "Jane" };
                const string SECRET_PASSWORD = "Secret!123";

                bool isSuccess = userManager.CreateAsync(user1, SECRET_PASSWORD).Result.Succeeded;
                isSuccess &= userManager.CreateAsync(user2, SECRET_PASSWORD).Result.Succeeded;

                if (isSuccess)
                {
                    var message1 = new Message
                    {
                        From = user2,
                        Title = "Test",
                        Body = "This is a test message",
                        Date = DateOnly.FromDateTime(DateTime.Now),
                        Rating = 10
                    };

                    context.Messages.Add(message1);

                    var message2 = new Message
                    {
                        From = user1,
                        Title = "Test",
                        Body = "This is another test message",
                        Date = DateOnly.FromDateTime(DateTime.Now),
                        Rating = 9
                    };

                    context.Messages.Add(message2);
                    context.SaveChanges();
                }
            }
        }
    }
}
