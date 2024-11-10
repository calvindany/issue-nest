using System.Security.Claims;

namespace backend_issue_nest.Models
{
    public static class Constants
    {
        public static readonly string[] USER_ROLE_NAME = new string[] { "Admin", "Client" };

        public enum USER_ROLE
        {
            USER_ROLE_ADMIN = 1,
            USER_ROLE_CLIENT = 2,
        }

        public static int GetUserRoleIndex(string roleName)
        {
            return Array.IndexOf(USER_ROLE_NAME, roleName);
        }

        public static readonly string[] TICKETS_STATUS_NAME = new string[] { "Open", "In Progress", "Resolved" };

        public static int GetTicketIndex(string ticketStatus)
        {
            return Array.IndexOf(TICKETS_STATUS_NAME, ticketStatus);
        }

        public enum TICKET_STATUS
        {
            OPEN = 1,
            IN_PROGRESS = 2,
            RESOLVE = 3
        }

    }
}
