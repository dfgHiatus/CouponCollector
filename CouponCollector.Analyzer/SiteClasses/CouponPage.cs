using LiteDB;
using System.Text.RegularExpressions;

namespace CouponCollector.Analyzer;

public class CouponPage
{
    private static readonly Regex PhoneNumberCleaner = new Regex(@"[() -]", RegexOptions.Compiled);

    [BsonId]
    public ObjectId Id { get; private set; }
    public int CouponCategory { get; private set; }
    public string Title { get; private set; }
    public string Address { get; private set; }
    public string PhoneNumber { get; private set; }
    public string CompanyWebsiteURL { get; private set; }
    public string CouponURL { get; private set; }
    public Coupon Coupon { get; private set; }

    [BsonCtor]
    public CouponPage(int category, string title, string address, string phoneNumber, string companyWebsiteURL, string couponLink, Coupon coupon)
    {
        CouponCategory = category;
        Title = title;
        Address = ExtractAddress(address);
        PhoneNumber = ExtractPhoneNumberURL(phoneNumber);
        CompanyWebsiteURL = ExtractURL(companyWebsiteURL);
        CouponURL = ExtractURL(couponLink);
        Coupon = coupon;
    }

    [BsonCtor]
    public CouponPage() { }

    // Phone numbers appear in a string as follows: 3137 S. Mill Ave., Tempe <br> (480) 758 - 5864
    // Given how they will always begin after the last '(', we can start from there

    /// <summary>
    /// Utility method to extract an address from the beginning of a given string
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    internal static string ExtractAddress(string input)
    {
        if (string.IsNullOrEmpty(input)) return string.Empty;

        var startIndex = input.LastIndexOf('(');
        if (startIndex == -1) return string.Empty;

        return input.Substring(0, startIndex);
    }

    /// <summary>
    /// Utility method to extract a US phone number from the end of a given string
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    internal static string ExtractPhoneNumberURL(string input)
    {
        if (string.IsNullOrEmpty(input)) return string.Empty;

        var startIndex = input.LastIndexOf('(');
        if (startIndex == -1) return string.Empty;

        var rawPhoneNumber = input.Substring(startIndex);
        return PhoneNumberCleaner.Replace(rawPhoneNumber, string.Empty);
    }

    /// <summary>
    /// Provides a manually escaped URL for the coupon downloader.
    /// Regex and URI.EscapeDataString are not sufficient for this task.
    /// This is a dirty hack, but it works.
    /// </summary>
    /// <returns></returns>
    internal static string ExtractURL(string input)
    {
        if (string.IsNullOrEmpty(input)) return string.Empty;

        return input.
            Replace("&amp;", "&");
    }
}
