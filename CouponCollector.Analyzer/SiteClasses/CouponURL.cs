namespace CouponsCollector.Analyzer;

public static class CouponURL
{
    private const string _baseURL = "https://m.studentinsider.com/";

    private const string _targetURL = "https://m.studentinsider.com/Coupon.aspx?AttribSetID=2&key=coupon&UniversityID={0}";

    private const string _targetWS = "https://m.studentinsider.com/wbsStudentInsider.asmx/GetGuideListingByCouponWS";

    public static Uri GetBaseURL()
    {
        return new Uri(_baseURL);
    }

    public static Uri GetWSURL()
    {
        return new Uri(_targetWS);
    }

    /// <summary>
    /// The target URL for the coupon downloader.
    /// </summary>
    /// <param name="university">The university to target</param>
    /// <returns></returns>
    public static Uri GetTargetURL(University university)
    {
        return new Uri(string.Format(_targetURL, (int)university));
    }

    /// <summary>
    /// Returns the content of the WS responsbile for serving page info
    /// </summary>
    /// <returns></returns>
    public static string GetPageByIndex(int pageIndex)
    {
        return string.Empty;
    }

    public enum University
    {
        UniversityOfArizona = 1,
        ArizonaStateUniversity = 2,
        TexasAMUniversity = 4,
        NorthernArizonaUniversity = 5,
    }
}
