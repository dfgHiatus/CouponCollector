using CouponsCollector.Analyzer;

namespace CouponCollector.Tests;

internal static class AnalysisTests
{
    internal static void TestCoupon()
    {
        Console.WriteLine();
        Console.WriteLine("BEGIN COUPON TESTS");
        Console.WriteLine();

        var coupondHTML = Path.Combine(Environment.CurrentDirectory, "Site", "Coupon.htm");
        string html = File.ReadAllText(coupondHTML);
        var coupon = CouponAnalyzer.Analyze(string.Empty, html);

        Console.WriteLine("CouponPage.Title: " + coupon.Title);
        Console.WriteLine("CouponPage.Address: " + coupon.Address);
        Console.WriteLine("CouponPage.Coupon Link: " + coupon.CouponURL);
        Console.WriteLine("CouponPage.Phone Number: " + coupon.PhoneNumber);
        Console.WriteLine("CouponPage.Website Link: " + coupon.CompanyWebsiteURL);
        Console.WriteLine("CouponPage.Coupon.PreferedSocialURL: " + coupon.CompanyPreferedSocialURL);
        Console.WriteLine("CouponPage.Coupon.Description: " + coupon.Description);
        Console.WriteLine("CouponPage.Coupon.Header: " + coupon.CouponDescriptor.Header);
        Console.WriteLine("CouponPage.Coupon.Description: " + coupon.CouponDescriptor.Description);
        Console.WriteLine("CouponPage.Coupon.Expiry: " + coupon.CouponDescriptor.Expiry);
        Console.WriteLine("CouponPage.Coupon.CouponCode: " + coupon.CouponDescriptor.CouponCode);
        Console.WriteLine();

        Console.WriteLine();
        Console.WriteLine("END COUPON TESTS");
        Console.WriteLine();
    }
}
