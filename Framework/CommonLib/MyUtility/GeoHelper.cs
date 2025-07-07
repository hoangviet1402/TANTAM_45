using System;

public static class GeoHelper
{
    /// <summary>
    /// Tính xem người dùng có nằm trong vùng phủ không
    /// </summary>
    /// <param name="latUser">Vĩ độ người dùng</param>
    /// <param name="lonUser">Kinh độ người dùng</param>
    /// <param name="accuracyUser">Độ chính xác của vị trí người dùng (mét)</param>
    /// <param name="latTarget">Vĩ độ điểm cần so sánh</param>
    /// <param name="lonTarget">Kinh độ điểm cần so sánh</param>
    /// <param name="radiusTarget">Bán kính vùng phủ sóng hoặc vùng an toàn của target (mét)</param>
    /// <returns>True nếu nằm trong vùng phủ, False nếu ngoài</returns>
    public static bool IsInCoverage(
        float latUser, float lonUser, int accuracyUser,
        double latTarget, double lonTarget, int radiusTarget)
    {
        double distance = CalculateDistance(latUser, lonUser, latTarget, lonTarget);
        double totalUncertainty = accuracyUser;

        return distance <= (radiusTarget + totalUncertainty);
    }

    /// <summary>
    /// Tính khoảng cách giữa 2 điểm trên trái đất (mét) theo công thức Haversine
    /// </summary>
    public static double CalculateDistance(float lat1, float lon1, double lat2, double lon2)
    {
        const double R = 6371000; // Bán kính trái đất (mét)
        double radLat1 = lat1 * Math.PI / 180.0;
        double radLat2 = lat2 * Math.PI / 180.0;
        double deltaLat = (lat2 - lat1) * Math.PI / 180.0;
        double deltaLon = (lon2 - lon1) * Math.PI / 180.0;

        double a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                   Math.Cos(radLat1) * Math.Cos(radLat2) *
                   Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);

        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        double distance = R * c;

        return distance;
    }
}
