using LiteDB;

namespace CouponCollector.Analyzer;

public class CouponDescription
{
    [BsonId]
    public ObjectId Id { get; private set; }
    public string Header { get; private set; }
    public string Description { get; private set; }
    public string Expiry { get; private set; }
    public string CouponCode { get; private set; }

    [BsonCtor]
    public CouponDescription(string header, string description, string expiry, string couponCode)
    {
        Header = header;
        Description = ExtractDescription(description);
        Expiry = ExtractExpiry(expiry);
        CouponCode = ExtractCouponCode(couponCode);
    }

    [BsonCtor]
    public CouponDescription() { }

    /* "Input" looks like this
     * 
     * $2 OFF ANY PURCHASE OF $10 OR MORESAVE
     $2 OFF ANY PURCHASE OF $10 OR MORE. Not valid with any other offer. 
    In-store purchases only (not valid for online orders). 1 coupon per 
    pers... Expires 12/31/2023.Coupon Code : ASU2239. Berry Divine Acai Bowls960 N. Scottsdale Rd.Tempe, AZ 85281(480) 967-0942
     */

    /// <summary>
    /// Utility method to extract the description from the given string
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private string ExtractDescription(string input)
    {
        if (string.IsNullOrEmpty(input)) return string.Empty;

        var endIndex = input.IndexOf("Expires");
        return endIndex == -1 ? input : input.Substring(0, endIndex); 
    }

    /// <summary>
    /// Utility method to extract the expiry from the given string
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private string ExtractExpiry(string input)
    {
        if (string.IsNullOrEmpty(input)) return string.Empty;

        var flag = "Expires ";
        var startIndex = input.IndexOf(flag);
        if (startIndex == -1) return input;

        var offsetIndex = startIndex + flag.Length;
        var endIndex = input.IndexOf(".", startIndex);
        if (endIndex == -1) return input;

        return input.Substring(offsetIndex, endIndex - offsetIndex);
    }

    /// <summary>
    /// Utility method to extract the coupon code from the given string
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private string ExtractCouponCode(string input)
    {
        if (string.IsNullOrEmpty(input)) return string.Empty;

        var flag = "Coupon Code : ";
        var startIndex = input.IndexOf(flag);
        if (startIndex == -1) return input;

        var offsetIndex = startIndex + flag.Length;
        var endIndex = input.IndexOf(".", startIndex);
        if (endIndex == -1) return input;

        return input.Substring(offsetIndex, endIndex - offsetIndex);
    }

    private static bool IsDateTimeValid(string input, out DateTime parsedTime)
    {
        return DateTime.TryParse(input, out parsedTime);
    }
}
