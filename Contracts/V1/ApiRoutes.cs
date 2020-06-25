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
            public const string UnLikePost = Base + "/posts/{postId}/unLike";
            public const string GetLikes = Base + "/posts/{postId}/likes";
        }

        public static class Comments
        {
            private const string CommentsBase = Base + "/comments";
            public const string GetCommentsByPost = Base + "/posts/{postId}/comments";
            public const string CreateComment = Base + "/posts/{postId}/comments";
            public const string GetComment = CommentsBase + "/{commentId}";
            public const string UpdateComment = CommentsBase + "/{commentId}";
            public const string DeleteComment = CommentsBase + "/{commentId}";
            public const string LikeComment = CommentsBase + "/{commentId}";
            public const string GetCommentLikes = CommentsBase + "/{commentId}/likes";
            public const string UnLikeComment = CommentsBase + "/{commentId}/unLike";
            public const string ReplyComment = CommentsBase + "/{commentId}/reply";
            public const string GetCommentReplies = CommentsBase + "/{commentId}/replies";
        }

        public static class Identity
        {
            public const string Login = Base + "/identity/login";
            public const string Register = Base + "/identity/register";
            public const string Refresh = Base + "/identity/refresh";
            public const string AuthenticatedUser = Base + "/user";
        }
    }
}