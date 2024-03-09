using System.Threading.Tasks;
using Kuva.Accounts.Service.Controllers;
using Kuva.Accounts.Service.Models;
using Kuva.Accounts.Tests.Fixtures;
using Newtonsoft.Json;
using Xunit;

namespace Kuva.Accounts.Tests.Controllers
{
    public class VersionControllerTest : BaseControllerTest
    {
        
        [Fact]
        public void ControllerGetTest()
        {
            var controller = new VersionController();
            var model = controller.Get();
            Assert.NotNull(model);
            Assert.IsType<VersionModel>(model);
            var actual = model as VersionModel;
            var expected = VersionFixture.GetVersionDefault();
            Assert.Equal(expected.Description, actual?.Description);
            Assert.Equal(expected.Version, actual?.Version);
        }

        internal async Task GetTestSample()
        {
            var response = await Client.GetAsync("/api/version");
            var expected = VersionFixture.GetVersionDefault();
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<VersionModel>(result);
            Assert.Equal(expected.Description, model.Description);
            Assert.Equal(expected.Version, model.Version);
        }
    }
}