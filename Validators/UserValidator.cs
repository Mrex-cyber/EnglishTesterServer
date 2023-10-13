namespace EnglishTesterServer.Validators
{
    public class UserValidator
    {
        public static bool EmailIsValid(string email)
        {
            if (email.Length < 8 || email.Length > 25 || !email.Contains('@')) return false;
            return true;
        }
        public static bool PasswordIsValid(string password)
        {
            if (password.Length < 8) return false;
            return true;
        }
    }
}
