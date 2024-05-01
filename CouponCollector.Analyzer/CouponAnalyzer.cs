using CouponCollector.Analyzer;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace CouponsCollector.Analyzer;

public static class CouponAnalyzer
{
    /// <summary>
    /// Given a Coupon document, return its object form
    /// </summary>
    /// <param name="html"></param>
    /// <returns></returns>
    public static Coupon Analyze(string uri, string html, CouponCategory.Category category = CouponCategory.Category.None)
    {
        var document = new HtmlDocument();
        document.LoadHtml(html);

        var tagsHTML = document.DocumentNode.Descendants("ul")
            .Where(node => node.GetAttributeValue("class", "").Contains("LegendWidth"))
            .FirstOrDefault()?
            .InnerText;

        string descTitle = string.Empty;
        string descTitleInnerText = string.Empty;
        try
        {
            var descHTML = document.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "").Contains("CouponContainer colorCoupons DiningCoupon"))
                .FirstOrDefault();

            descTitleInnerText = descHTML?.InnerText!;
            descTitle = descHTML?
                .Descendants("h1")
                .FirstOrDefault()!
                .Descendants("a")
                .FirstOrDefault()?
                .InnerText!;
        }
        catch (Exception ex) { Console.WriteLine(ex.Message); }

        return document.DocumentNode.Descendants("div")
            .Where(node => node.GetAttributeValue("class", "").Contains("AdvertiseListing"))
            .Select(node => // Convert HTML to classes
            {
                return new Coupon
                (
                    category: Convert.ToInt32(category),      
                    title: node.Descendants("h2").FirstOrDefault()?.InnerText!,
                    address: ExtractAddress(node.Descendants("p").FirstOrDefault()?.InnerText!)!,
                    phoneNumber: ExtractPhoneNumberURL(node.Descendants("p").FirstOrDefault()?.InnerText!),
                    companyWebsiteURL: node.Descendants("a").FirstOrDefault()?.GetAttributeValue("href", "")!,
                    couponLink: uri,
                    companyPreferedSocialURL: node.Descendants("p").Skip(2).FirstOrDefault()?.InnerText!,
                    description: node.Descendants("p").LastOrDefault()?.InnerText!,
                    tags: tagsHTML!,
                    cDesc: new CouponDescription
                    (
                        header: descTitle,
                        description: descTitleInnerText!,
                        expiry: descTitleInnerText!,
                        couponCode: descTitleInnerText!
                    )
                );
            })
            .Where(rawCoupon => // Filter null entries
            {
                return !(string.IsNullOrEmpty(rawCoupon.CompanyPreferedSocialURL) ||
                         string.IsNullOrEmpty(rawCoupon.Description) ||
                         string.IsNullOrEmpty(rawCoupon.CouponDescriptor.Header) ||
                         string.IsNullOrEmpty(rawCoupon.CouponDescriptor.Description) ||
                         string.IsNullOrEmpty(rawCoupon.CouponDescriptor.CouponCode)) &&
                         rawCoupon.Tags.Count != 0;
            })
            .FirstOrDefault()!; // As this will always be at most one element
    }

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
        return Regex.Replace(rawPhoneNumber, @"[() -]", string.Empty);
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
