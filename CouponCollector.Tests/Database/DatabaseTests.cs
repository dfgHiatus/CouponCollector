using CouponCollector.Analyzer;
using CouponCollector.Database;
using System.Text;
using static CouponsCollector.Analyzer.CouponCategory;

namespace CouponCollector.Tests.Database;

internal class DatabaseTests
{
    internal static void TestDatabase()
    {
        DatabaseManager db = new DatabaseManager();

        try
        {
            Console.WriteLine();
            Console.WriteLine("BEGIN DATABASE TESTS");
            Console.WriteLine();

            for (int i = 0; i < 50; i++)
                db.Insert(MakeRandomCoupon());

            foreach (var item in db.Find(x => x.Title.Contains('a')))
                Console.WriteLine(item.Title);

            db = new DatabaseManager();
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine("Singleton logic in place");
            Console.WriteLine(e.Message);
        }
        finally
        {
            db.DeleteAll();
            Console.WriteLine();
            Console.WriteLine("END DATABASE TESTS");
            Console.WriteLine();
        }
    }
    
    //internal static CouponPage MakeRandomCouponPage()
    //{
    //    return new CouponPage(
    //        cateogry: GenerateRandomCategory(),
    //        title: GenerateRandomString(10),
    //        address: GenerateRandomString(20),
    //        phoneNumber: GenerateRandomPhoneNumber(),
    //        companyWebsiteURL: GenerateRandomString(20),
    //        couponLink: GenerateRandomString(20),
    //        coupon: MakeRandomCoupon()
    //     );
    //}

    internal static Coupon MakeRandomCoupon()
    {
        return new Coupon
        (
            category: GenerateRandomCategory(),
            title: GenerateRandomString(10),
            address: GenerateRandomString(20),
            phoneNumber: GenerateRandomPhoneNumber(),
            companyWebsiteURL: GenerateRandomString(20),
            couponLink: GenerateRandomString(20),
            companyPreferedSocialURL: GenerateRandomString(20),
            description: GenerateRandomString(50),
            tags: GenerateRandomString(50),
            cDesc: MakeRandomCouponDescriptor()
        );
    }

    internal static CouponDescription MakeRandomCouponDescriptor()
    {
        return new CouponDescription
        (
            header: GenerateRandomString(10),
            description: GenerateRandomString(10),
            expiry: GenerateRandomString(10),
            couponCode: GenerateRandomString(8)
        );
    }

    private const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ .?!,;:";
    private static List<string> GenerateRandomStrings(int amount, int length)
    {
        var list = new List<string>();
        for (int i = 0; i < amount; i++)
        {
            list.Add(GenerateRandomString(length));
        }
        return list;
    }
    private static string GenerateRandomString(int length)
    {
        StringBuilder stringBuilder = new StringBuilder(length);

        for (int i = 0; i < length; i++)
        {
            int randomIndex = Random.Shared.Next(chars.Length);
            stringBuilder.Append(chars[randomIndex]);
        }

        return stringBuilder.ToString();
    }
    private static string GenerateRandomPhoneNumber()
    {
        StringBuilder stringBuilder = new StringBuilder();

        for (int i = 0; i < 10; i++)
        {
            stringBuilder.Append(Random.Shared.Next(0, 10));
        }

        return stringBuilder.ToString();
    }
    private static DateTime GenerateRandomDateTime()
    {
        DateTime minDate = new DateTime(2000, 1, 1);
        DateTime maxDate = DateTime.Now;
        int range = (maxDate - minDate).Days;
        return minDate.AddDays(Random.Shared.Next(range)).AddHours(Random.Shared.Next(24)).AddMinutes(Random.Shared.Next(60)).AddSeconds(Random.Shared.Next(60));
    }
    private static int GenerateRandomCategory()
    {
        int index = Random.Shared.Next(0, CategoryRepresentation.Length);
        return CategoryRepresentation[index];
    }
}
