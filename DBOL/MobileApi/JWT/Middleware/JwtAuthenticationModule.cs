using System.Web;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using TanTamApi.JWT.Helper;

namespace TanTamApi.JWT.Middleware
{
    public class JwtAuthenticationModule : IHttpModule
    {
        private JwtTokenHelper _jwtTokenHelper;

        public void Init(HttpApplication context)
        {
            _jwtTokenHelper = new JwtTokenHelper(new AppSettingsConfiguration());
            
            context.BeginRequest += OnBeginRequest;
        }

        private void OnBeginRequest(object sender, System.EventArgs e)
        {
            var application = (HttpApplication)sender;
            var context = application.Context;
            
            // Skip authentication for OPTIONS requests (CORS)
            if (context.Request.HttpMethod == "OPTIONS")
                return;

            // Skip authentication for specific paths if needed
            if (ShouldSkipAuthentication(context.Request.Path))
                return;

            var token = context.Request.Headers["Authorization"]?.Split(' ').Last();
            if (!string.IsNullOrEmpty(token))
            {
                _jwtTokenHelper.AttachUserToContext(context, token);
            }
        }

        private bool ShouldSkipAuthentication(string path)
        {
            // Add paths that should skip authentication
            var skipPaths = new[] { "/api/auth/login", "/api/auth/register" };
            return skipPaths.Any(p => path.StartsWith(p, System.StringComparison.OrdinalIgnoreCase));
        }

        public void Dispose()
        {
            // Clean up if needed
        }
    }
} 