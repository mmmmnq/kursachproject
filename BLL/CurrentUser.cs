namespace BLL
{
    public static class CurrentUser
    {
        public static int UserId { get; private set; }
        public static string Username { get; private set; }
        public static string Role { get; private set; }

        public static bool IsAuthenticated { get; private set; } = false;

        // Метод для установки данных о текущем пользователе
        public static void SetUser(int userId, string username, string role)
        {
            UserId = userId;
            Username = username;
            Role = role;
            IsAuthenticated = true;
        }

        // Метод для проверки, авторизован ли пользователь
        public static bool IsUserLoggedIn()
        {
            return IsAuthenticated;
        }

        // Метод для очистки данных о пользователе
        public static void Logout()
        {
            UserId = 0;
            Username = null;
            Role = null;
            IsAuthenticated = false;
        }
    }
}
