using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace EnglishTesterServer.Auth
{
    public class AuthOptions
    {
        public const string ISSUER = "EnglishTesterServer";
        public const string AUDIENCE = "EnglishLearner";
        private const string KEY = "tester5erv3rtaskfortesting";
        public static SymmetricSecurityKey GetSecurityKey() => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));   
    }
}
