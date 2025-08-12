using System;

namespace BussinessObject.Helper
{
    /// <summary>
    /// Helper class cho việc xử lý properties
    /// </summary>
    public static class PropertyHelper
    {
        /// <summary>
        /// Lấy giá trị property từ object một cách an toàn
        /// </summary>
        /// <param name="obj">Object cần lấy property</param>
        /// <param name="propertyName">Tên property</param>
        /// <returns>Giá trị property hoặc default value</returns>
        public static T GetPropertyValue<T>(object obj, string propertyName)
        {
            try
            {
                if (obj == null)
                    return default(T);

                var property = obj.GetType().GetProperty(propertyName);
                if (property != null)
                {
                    var value = property.GetValue(obj);
                    if (value != null && value is T)
                    {
                        return (T)value;
                    }
                }
                return default(T);
            }
            catch
            {
                return default(T);
            }
        }
    }
} 