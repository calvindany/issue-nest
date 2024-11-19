using backend_issue_nest.Models;

namespace backend_issue_nest.Controllers.Helper
{
    public class ValidationHelper
    {
        private readonly Type _enumType;

        public ValidationHelper(Type enumType)
        {
            _enumType = enumType;   
        }

        public bool EnumValidation(object value)
        {
            if (value == null || !Enum.IsDefined(_enumType, value)) 
            {
                return false;
            }

            return true;
        }
    }
}
