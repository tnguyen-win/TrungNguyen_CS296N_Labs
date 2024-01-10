using PopularGameEngines.Data;
using PopularGameEngines.Models;

namespace PopularGameEngines {
    public class SeedData {
        public static void Seed(AppDbContext context) {
            if (!context.Messages.Any()) {
                var user1 = new AppUser { Name = "Brian" };
                var user2 = new AppUser { Name = "Amanda" };

                context.Users.Add(user1);
                context.Users.Add(user2);
                context.SaveChanges();

                var message1 = new Message {
                    From = user2,
                    Body = "This is a test message",
                    Date = DateOnly.FromDateTime(DateTime.Now),
                    Rating = 10
                };

                context.Messages.Add(message1);

                var message2 = new Message {
                    From = user1,
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
