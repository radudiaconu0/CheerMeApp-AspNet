using System.Dynamic;
using System.Xml.Linq;

namespace CheerMeApp.Contracts.V1
{
    public static class ApiRoutes
    {
        private const string Root = "api";

        private const string Version = "v1";

        private const string Base = Root + "/" + Version;

        public static class Posts
        {
            public const string GetAll = Base + "/posts";
            public const string Create = Base + "/posts";
            public const string Get = Base + "/posts/{postId}";
            public const string Update = Base + "/posts/{postId}";
            public const string Delete = Base + "/posts/{postId}";
            public const string LikePost = Base + "/posts/{postId}/like";
            public const string UnLikePost = Base + "/posts/{postId}/unlike";
            public const string GetLikes = Base + "/posts/{postId}/likes";
        }

        public static class Identity
        {
            public const string Login = Base + "/identity/login";
            public const string Register = Base + "/identity/register";
            public const string Refresh = Base + "/identity/refresh";
        }
    }
}