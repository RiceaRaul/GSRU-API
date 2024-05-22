using System.Linq.Expressions;

namespace GSRU_API.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class HaveRoleAttribute : Attribute
    {
        public string RolePrefix { get; }
        public string ParamName { get; }

        public HaveRoleAttribute(string rolePrefix, string paramName)
        {
            RolePrefix = rolePrefix;
            ParamName = paramName;
        }
    }
}
