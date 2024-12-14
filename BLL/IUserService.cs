namespace BLL
{
    public interface IUserService
    {
        bool AuthenticateUser(string username, string password, out string role);
        int GetUserId(string username); 
    }
}
