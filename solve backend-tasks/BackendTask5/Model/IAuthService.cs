namespace BackendTask5.Model
{
    public interface IAuthService
    {
        string GenerateToken(string username);
        bool validateUser(string username, string password);
    }
}
