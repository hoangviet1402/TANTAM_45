using System.Web;

namespace TanTamApi.JWT.Extensions
{

    public static class AuthorizationInforExtensions
    {
        /// <summary>
        /// Lấy EmployeeId từ context, trả về 0 nếu null
        /// </summary>
        public static int GetEmployeeId(this HttpContext context)
        {
            return context.Items["EmployeeId"] as int? ?? 0;
        }

        public static int GetAccountId(this HttpContext context)
        {
            return context.Items["AccountId"] as int? ?? 0;
        }


        public static string GetJwtID(this HttpContext context)
        {
            var jti = "";// context.User.FindFirst("jti")?.Value;
            if (string.IsNullOrEmpty(jti))
            {
                jti = context.Items["JwtID"] as string ?? string.Empty;
            }
            return jti;
        }

        /// <summary>
        /// Lấy CompanyId từ context, trả về 0 nếu null
        /// </summary>
        public static int GetCompanyId(this HttpContext context)
        {
            return context.Items["CompanyId"] as int? ?? 0;
        }

        /// <summary>
        /// Lấy Role từ context, trả về 0 nếu null
        /// </summary>
        public static int GetRole(this HttpContext context)
        {
            return context.Items["Role"] as int? ?? 0;
        }

        /// <summary>
        /// Lấy Username từ context, trả về "" nếu null
        /// </summary>
        public static string GetUsername(this HttpContext context)
        {
            return context.Items["Username"] as string ?? string.Empty;
        }

        /// <summary>
        /// Lấy Email từ context, trả về "" nếu null
        /// </summary>
        public static string GetEmail(this HttpContext context)
        {
            return context.Items["Email"] as string ?? string.Empty;
        }

        /// <summary>
        /// Kiểm tra user có role được chỉ định không
        /// </summary>
        public static bool HasRole(this HttpContext context, int role)
        {
            return context.GetRole() == role;
        }

        ///// <summary>
        ///// Kiểm tra user có phải là SystemAdmin không
        ///// </summary>
        //public static bool IsSystemAdmin(this HttpContext context)
        //{
        //    return context.HasRole((int)UserRole.SystemAdmin);
        //}

        ///// <summary>
        ///// Kiểm tra user có phải là Employee không
        ///// </summary>
        //public static bool IsEmployee(this HttpContext context)
        //{
        //    return context.HasRole((int)UserRole.Employees);
        //}
    }
}