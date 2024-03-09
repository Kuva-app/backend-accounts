namespace Kuva.Accounts.Business.Validators
{
    public interface IValidator
    {
        bool IsTrue<T>(T value);
    }
}