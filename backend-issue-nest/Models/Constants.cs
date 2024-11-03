using System.Security.Claims;

namespace backend_issue_nest.Models
{
    public static class Constants
    {
        public static readonly string[] USER_ROLE_NAME = new string[] { "Admin", "Client" };

        public static USER_ROLE Status { get; set; }

        public enum USER_ROLE
        {
            USER_ROLE_ADMIN = 0,
            USER_ROLE_CLIENT = 1,
        }

    }
}
