using System.Linq;
using System.Threading.Tasks;
using Kuva.Accounts.Entities;
using Kuva.Accounts.Repository.Data.Interfaces;
using Utilities.General.XunitPriorityAttributes;

namespace Kuva.Accounts.Tests.Repository
{
    public class UserLevelDataTest : BaseRepositoryTest
    {
        private readonly IUserLevelData _userLevelData;
        
        public UserLevelDataTest(IUserLevelData userLevelData)
        {
            _userLevelData = userLevelData;
        }

        [Fact, TestPriority(1)]
        public async Task GetDataTest()
        {
            var userLevels = await _userLevelData.GetDataAsync(1, 5);
            Assert.NotNull(userLevels);
            
            var userLevelEntities = userLevels as UserLevelEntity[] ?? userLevels.ToArray();
            Assert.True(userLevelEntities.Any());
            
            var userLevel = userLevelEntities.First();
            var actual = await _userLevelData.GetDataByPrimaryKeyAsync(userLevel);
            Assert.Null(actual);
        }
    }
}