using System;
using System.Collections.Generic;
using System.Linq;

namespace TanTamApi.Helper
{
    public static class StringExtensions
    {
        public static List<int> ToIntList(this string csv)
        {
            if (string.IsNullOrWhiteSpace(csv))
                return new List<int>();
            return csv.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => int.TryParse(x.Trim(), out var v) ? v : 0)
                .Where(x => x > 0)
                .ToList();
        }
    }
}
