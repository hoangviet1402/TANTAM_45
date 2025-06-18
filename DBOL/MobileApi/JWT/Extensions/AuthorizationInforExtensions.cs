using System.Web;
using System.Web.Http;
using System.Net.Http;

namespace TanTamApi.JWT.Extensions
{

    public static class AuthorizationInforExtensions
    {
        /// <summary>
        /// Lấy HttpContext từ HttpRequestMessage
        /// </summary>
        public static HttpContext GetHttpContext(this HttpRequestMessage request)
        {
            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return request.Properties["MS_HttpContext"] as HttpContext;
            }
            return null;
        }

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

        // Web API Extensions - Sử dụng HttpContext từ Request
        /// <summary>
        /// Lấy CompanyId từ HttpRequestMessage (Web API), trả về 0 nếu null
        /// </summary>
        public static int GetCompanyId(this HttpRequestMessage request)
        {
            var context = request.GetHttpContext();
            return context?.Items["CompanyId"] as int? ?? 0;
        }

        /// <summary>
        /// Lấy AccountId từ HttpRequestMessage (Web API), trả về 0 nếu null
        /// </summary>
        public static int GetAccountId(this HttpRequestMessage request)
        {
            var context = request.GetHttpContext();
            return context?.Items["AccountId"] as int? ?? 0;
        }

        /// <summary>
        /// Lấy EmployeeId từ HttpRequestMessage (Web API), trả về 0 nếu null
        /// </summary>
        public static int GetEmployeeId(this HttpRequestMessage request)
        {
            var context = request.GetHttpContext();
            return context?.Items["EmployeeId"] as int? ?? 0;
        }

        /// <summary>
        /// Lấy Role từ HttpRequestMessage (Web API), trả về 0 nếu null
        /// </summary>
        public static int GetRole(this HttpRequestMessage request)
        {
            var context = request.GetHttpContext();
            return context?.Items["Role"] as int? ?? 0;
        }

        /// <summary>
        /// Lấy Username từ HttpRequestMessage (Web API), trả về "" nếu null
        /// </summary>
        public static string GetUsername(this HttpRequestMessage request)
        {
            var context = request.GetHttpContext();
            return context?.Items["Username"] as string ?? string.Empty;
        }

        /// <summary>
        /// Lấy Email từ HttpRequestMessage (Web API), trả về "" nếu null
        /// </summary>
        public static string GetEmail(this HttpRequestMessage request)
        {
            var context = request.GetHttpContext();
            return context?.Items["Email"] as string ?? string.Empty;
        }

        /// <summary>
        /// Lấy JWT ID từ HttpRequestMessage (Web API), trả về "" nếu null
        /// </summary>
        public static string GetJwtID(this HttpRequestMessage request)
        {
            var context = request.GetHttpContext();
            return context?.Items["JwtID"] as string ?? string.Empty;
        }

        /// <summary>
        /// Kiểm tra user có role được chỉ định không (Web API)
        /// </summary>
        public static bool HasRole(this HttpRequestMessage request, int role)
        {
            return request.GetRole() == role;
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