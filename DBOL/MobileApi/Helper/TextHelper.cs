using System.Linq;

namespace TanTamApi.Helper
{

    public static class TextHelper
    {
        // Thêm các hàm xử lý chuỗi tại đây
        // Ví dụ:
        public static bool IsNullOrWhiteSpace(string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }
        /// <summary>
        /// Loại bỏ dấu tiếng Việt và thay thế khoảng trắng bằng ký tự cho trước
        /// </summary>
        public static string NormalizeText(string input, string separator = "_")
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;
            // Loại bỏ dấu tiếng Việt
            string text = input.Normalize(System.Text.NormalizationForm.FormD);
            var chars = text.Where(c => System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c) != System.Globalization.UnicodeCategory.NonSpacingMark).ToArray();
            text = new string(chars).Normalize(System.Text.NormalizationForm.FormC);
            // Thay thế các ký tự không phải chữ/số bằng separator
            text = System.Text.RegularExpressions.Regex.Replace(text, "[^a-zA-Z0-9]+", separator);
            // Loại bỏ separator ở đầu/cuối và thay thế nhiều separator liên tiếp thành 1
            text = System.Text.RegularExpressions.Regex.Replace(text, $"{separator}+", separator).Trim(separator.ToCharArray());
            return text;
        }
        // Bạn có thể bổ sung các hàm khác tùy ý
    }

}