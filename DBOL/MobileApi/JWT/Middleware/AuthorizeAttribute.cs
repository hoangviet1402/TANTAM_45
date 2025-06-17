using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TanTamApi.JWT.Middleware
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        private readonly int[] _roles;

        public AuthorizeAttribute(params int[] roles)
        {
            _roles = roles;
        }
        //Lưu ý:
        //[Authorize] ở controller level là cấu hình mặc định cho tất cả actions 
        //Có thể override bằng cách thêm[Authorize] với tham số khác ở action level vd [Authorize((int)UserRole.SystemAdmin, (int)UserRole.BranchManager)] or [Authorize(1,2)] or [Authorize()]
        //Có thể sử dụng[AllowAnonymous] để cho phép truy cập công khai
        //Authorization ở action level sẽ override authorization ở controller level
        // **** Request -> Authentication Middleware -> Authorization Pipeline -> OnAuthorization -> Controller Action
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var context = filterContext.HttpContext;
            var accountId = TryConvertToInt(context.Items["AccountId"]);
            var companyId = TryConvertToInt(context.Items["CompanyId"]);
            var userRole = TryConvertToInt(context.Items["Role"]);

            if (accountId <= 0 || companyId <= 0 || userRole <= 0)
            {
                filterContext.Result = new HttpStatusCodeResult(401, "Unauthorized");
                return;
            }

            if (_roles != null && _roles.Any() && !_roles.Contains(userRole))
            {
                filterContext.Result = new HttpStatusCodeResult(403, "Forbidden");
                return;
            }
        }

        private int TryConvertToInt(object value)
        {
            if (value == null)
                return 0;

            return int.TryParse(value.ToString(), out int result) ? result : 0;
        }
    }
}