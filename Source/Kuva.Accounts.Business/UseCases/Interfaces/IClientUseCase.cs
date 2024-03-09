namespace Kuva.Accounts.Business.UseCases.Interfaces
{
    public interface IClientUseCase
    {
        string GetHashPasscode(string password, string email);
    }
}
