using System.Linq.Expressions;

namespace GSRU_API.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class HaveRoleAttribute(string rolePrefix, string paramName) : Attribute
    {
        public string RolePrefix { get; } = rolePrefix;
        public string ParamName { get; } = paramName;
    }
}
