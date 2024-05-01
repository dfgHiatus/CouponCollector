using LiteDB;
using System.Text.RegularExpressions;

namespace CouponCollector.Analyzer;

public class Coupon
{
    [BsonId]
    public ObjectId Id { get; private set; }
    public string Title { get; private set; }
    public string Address { get; private set; }
    public double Distance { get; set; }
    public string PhoneNumber { get; private set; }
    public string CompanyWebsiteURL { get; private set; }
    public string CouponURL { get; private set; }
    public int CouponCategory { get; private set; }
    public string CompanyPreferedSocialURL { get; private set; }
    public string Description { get; private set; }
    public List<string> Tags { get; private set; }
    public CouponDescription CouponDescriptor { get; private set; }

    [BsonCtor]
    public Coupon(int category, string title, string address, string phoneNumber, string companyWebsiteURL, string couponLink, string companyPreferedSocialURL, string description, string tags, CouponDescription cDesc)
    {
        CouponCategory = category;
        Title = title;
        Address = address;
        PhoneNumber = phoneNumber;
        CompanyWebsiteURL = companyWebsiteURL;
        CouponURL = couponLink;
        CompanyPreferedSocialURL = companyPreferedSocialURL;
        Description = description;
        Tags = SetTags(tags);
        CouponDescriptor = cDesc;
    }

    [BsonCtor]
    public Coupon() { }

    /// <summary>
    /// Splits the string by uppercase characters
    /// </summary>
    /// <param name="html"></param>
    /// <returns></returns>
    private List<string> SetTags(string input)
    {
        if (string.IsNullOrEmpty(input)) return Enumerable.Empty<string>().ToList();

        return Regex.Split(input, @"(?<!^)(?=[A-Z])").ToList();
    }
}
