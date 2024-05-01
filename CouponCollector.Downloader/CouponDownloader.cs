using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using static CouponsCollector.Analyzer.CouponCategory;
using static CouponsCollector.Analyzer.CouponURL;

namespace CouponCollector.Downloader;

public static class CouponDownloader
{
    // 1) Get a url to a category page
    // 2) Walk over the coupon on that page
    // 3) Append to a list of URLS
    // 4) When all coupons have been read, move onto the next category
    // 5) Repeat until we have reached the end, or otherwise

    private const string urlSuffix = @"<a\s+href=""([^""]+)""";
    
    /// <summary>
    /// Returns a list of URL
    /// </summary>
    /// <param name="uni"></param>
    /// <returns></returns>
    public static Dictionary<Category, List<Uri>> Download(University uni)
    {
        Dictionary<Category, List<Uri>> dict = new Dictionary<Category, List<Uri>>();
        var baseURI = GetWSURL();

        try
        {
            using var client = new WebClient();
            client.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:109.0) Gecko/20100101 Firefox/116.0)");
            client.Headers.Add("content-type", "application/json; charset=utf-8");
            client.Encoding = Encoding.UTF8;

            for (int i = 0; i < CategoryRepresentation.Length; i++)
            {
                string response = Regex.Unescape(
                    client.UploadString(
                        baseURI,
                        "POST",
                        FormJSON(uni, CategoryRepresentation[i])));
                Thread.Sleep((int)((Random.Shared.NextDouble() * 1000) + 3000)); // Don't get rate limited. "Outer page"
                dict.Add((Category)CategoryRepresentation[i], ExtractURIsFromResponse(response));
            }
        }
        catch (WebException ex)
        {
            Console.WriteLine("An error occurred:");
            Console.WriteLine(ex.Message);
        }

        return dict;
    }

    private static string FormJSON(University uni, int category)
    {
        Dictionary<string, string> JSONDict = new Dictionary<string, string>
        {
            { "strBusinessName", " "},
            { "strCategoryID", category.ToString()},
            { "strDisplayStart", "1"},
            { "strUniversityID", Convert.ToInt32(uni).ToString() },
        };

        return JsonSerializer.Serialize(JSONDict);
    }

    private static List<Uri> ExtractURIsFromResponse(string input)
    {
        return Regex.Matches(input, urlSuffix)
            .Select(match => match.Groups[1].Value) // Get the URL extensions from the XML response
            .Where(hrefValue => !hrefValue.StartsWith("http")) // Filter out suffixes
            .Select(url => new Uri(GetBaseURL() + url)) // Create URL targets
            .Distinct() // Remove duplicates
            .ToList();
    }
}
