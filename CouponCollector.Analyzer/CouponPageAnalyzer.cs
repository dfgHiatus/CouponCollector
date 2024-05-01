using CouponCollector.Analyzer;
using HtmlAgilityPack;
using System.Net;
using static CouponsCollector.Analyzer.CouponCategory;

namespace CouponsCollector.Analyzer;

public static class CouponPageAnalyzer
{
    /// <summary>
    /// Given a CouponPage document, return its object form
    /// </summary>
    /// <param name="html"></param>
    /// <returns></returns>
    public static IEnumerable<CouponPage> Analyze(string html, Category category = Category.None)
    {
        var document = new HtmlDocument();
        document.LoadHtml(html);

        return document.DocumentNode.Descendants("div")
            .Where(node => node.GetAttributeValue("class", "").Contains("AdvertiseListing"))
            .Select(node => // Convert HTML to classes
            {
                // When downloading the Coupon HTML, don't get rate limited!
                using var client = new WebClient();
                var cURL = CouponPage.ExtractURL(node.Descendants("a").FirstOrDefault()?.GetAttributeValue("href", "")!);
                var coupon = new CouponPage
                (
                    category: Convert.ToInt32(category),
                    title: node.Descendants("h2").FirstOrDefault()?.Descendants("a").FirstOrDefault()?.InnerText!,
                    address: node.Descendants("p").FirstOrDefault()?.InnerText!,
                    phoneNumber: node.Descendants("p").Take(1).FirstOrDefault()?.InnerText!,
                    companyWebsiteURL: node.Descendants("a").Skip(1).FirstOrDefault()?.GetAttributeValue("href", "")!,
                    couponLink: cURL,
                    coupon: CouponAnalyzer.Analyze(cURL, client.DownloadString(cURL), category)
                );
                Thread.Sleep((int)((Random.Shared.NextDouble() * 1000) + 3000)); // Don't get rate limited. "Inner page"
                return coupon;
            })
            .Where(coupon => // Filter null entries
            {
                return !(string.IsNullOrEmpty(coupon.Title) ||
                         string.IsNullOrEmpty(coupon.Address) ||
                         string.IsNullOrEmpty(coupon.CouponURL) ||
                         string.IsNullOrEmpty(coupon.CompanyWebsiteURL) ||
                         string.IsNullOrEmpty(coupon.PhoneNumber));
            })
            .DistinctBy(coupon => // Filter unique entries (by coupon id)
            {
                return coupon.CouponURL;
            });
    }
}
