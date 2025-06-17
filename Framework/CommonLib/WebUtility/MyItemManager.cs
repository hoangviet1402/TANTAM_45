using System.Web;

namespace WebUtility
{
    public class MyItemManager
    {
        public static void Set(string name, string value)
        {
            try
            {
                if (HttpContext.Current == null)
                    return;

                if (HttpContext.Current.Items == null)
                    return;

                HttpContext.Current.Items[name] = value;
            }
            catch
            {
            }
        }

        public static string Get(string name, string defaultValue = "")
        {
            try
            {
                if (HttpContext.Current == null)
                    return defaultValue;

                if (HttpContext.Current.Items == null)
                    return defaultValue;

                return string.Format("{0}", HttpContext.Current.Items[name]);
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}