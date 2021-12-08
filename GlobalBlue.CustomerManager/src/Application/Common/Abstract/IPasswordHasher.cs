namespace GlobalBlue.CustomerManager.Application.Common.Abstract
{
    public interface IPasswordHasher
    {
        string Hash(string password);
    }
}
