using Kuva.Accounts.Business.Validators;
using Xunit;

namespace Kuva.Accounts.Tests.Business.ValidatorsTest
{
    public class EmailValidatorTest
    {
        [Theory]
        [InlineData("teste@email.com")]
        [InlineData("a@b.com")]
        [InlineData("a@really.long.stinkin.domain.name")]
        public void ValidateIsTrueTest(string email)
        {
            Assert.True(EmailValidator.Shared.IsTrue(email));
        }
        
        [Theory]
        [InlineData("teste")]
        [InlineData("a")]
        [InlineData("really.long.stinkin.domain.name")]
        public void ValidateIsFalseTest(string email)
        {
            Assert.False(EmailValidator.Shared.IsTrue(email));
        }
    }
}