using CouponCollector.Analyzer;
using CouponCollector.Database;
using CouponCollector.Downloader;
using CouponsCollector.Analyzer;
using CsvHelper;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Text.RegularExpressions;
using static CouponsCollector.Analyzer.CouponCategory;
using static CouponsCollector.Analyzer.CouponURL;

namespace CoupondCollector;

/// <summary>
/// TODO - Add none category crawling!
/// </summary>
internal class Program
{
    private static readonly DatabaseManager db = new DatabaseManager();

    static void Main(string[] args)
    {
        // db.DeleteAll();
        // DownloadAnalyzeStore();

        foreach (var c in db.FindAll().Where(x => x.CouponCategory == (int) Category.PizzaWings))
        {
            Console.WriteLine($"{c.Title} : {c.CouponDescriptor.Expiry}");
        }
    }

    private static void DownloadAnalyzeStore()
    {
        // Download
        var categoryAndURLs = CouponDownloader.Download(University.ArizonaStateUniversity);

        // Analyzer
        List<Coupon> coupons = Enumerable.Empty<Coupon>().ToList();
        using var client = new WebClient();
        
        foreach (var categoryAndURL in categoryAndURLs)
        {
            foreach (var url in categoryAndURL.Value) // URL list
            {
                var c = CouponAnalyzer.Analyze(url.AbsolutePath, client.DownloadString(url), categoryAndURL.Key);
                if (c != null) coupons.Add(c); // Malformed data will be null
                Thread.Sleep((int)((Random.Shared.NextDouble() * 1000) + 3000));
            }
        }
        
        // Database 
        foreach (var item in coupons.Where(x => x != null))
        {
            db.Insert(item);
        }

        // Crashes?
        // db.InsertBulk(coupons);
    }

    private static void ExportCouponsToCsv(IEnumerable<Coupon> coupons, string outputPath)
    {
        using (var writer = new StreamWriter(outputPath))
        using (var csv = new CsvWriter(writer))
        {
            csv.WriteField("Id");
            csv.WriteField("Title");
            csv.WriteField("Address");
            csv.WriteField("PhoneNumber");
            csv.WriteField("CompanyWebsiteURL");
            csv.WriteField("CouponURL");
            csv.WriteField("CouponCategory");
            csv.WriteField("CompanyPreferedSocialURL");
            csv.WriteField("Description");
            csv.WriteField("Tags");
            csv.WriteField("CouponDescriptor.Header");
            csv.WriteField("CouponDescriptor.Description");
            csv.WriteField("CouponDescriptor.Expiry");
            csv.WriteField("CouponDescriptor.CouponCode");
            csv.NextRecord();

            foreach (var coupon in coupons)
            {
                csv.WriteField(coupon.Id);
                csv.WriteField(coupon.Title);
                csv.WriteField(coupon.Address);
                csv.WriteField(coupon.PhoneNumber);
                csv.WriteField(coupon.CompanyWebsiteURL);
                csv.WriteField(coupon.CouponURL);
                csv.WriteField((Category)coupon.CouponCategory);
                csv.WriteField(coupon.CompanyPreferedSocialURL);
                csv.WriteField(coupon.Description);
                csv.WriteField(string.Join(", ", coupon.Tags));
                csv.WriteField(coupon.CouponDescriptor.Header);
                csv.WriteField(coupon.CouponDescriptor.Description);
                csv.WriteField(coupon.CouponDescriptor.Expiry);
                csv.WriteField(coupon.CouponDescriptor.CouponCode);
                csv.NextRecord();
            }
        }
    }
}