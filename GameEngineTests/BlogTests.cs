using PopularGameEngines.Controllers;
using PopularGameEngines.Data;
using PopularGameEngines.Models;

namespace GameEngineTests {
    public class GameEngineTests {
        [Fact]
        public void ForumPostTest() {
            var repo = new FakeBlogRepository();
            var controller = new BlogController(repo, null!);
            var model = new Message() {
                From = new AppUser { Name = "Tester", },
                Body = "This is a test"
            };

            controller.Post(model);

            Assert.True(model.MessageId > 0);
            Assert.Equal(model.Date.ToShortDateString(), DateTime.Now.ToShortDateString());
            Assert.InRange(model.Rating, 1, 10);
        }
    }
}
