using CouponCollector.Analyzer;
using LiteDB;
using System.Linq.Expressions;

namespace CouponCollector.Database;

public class DatabaseManager
{
    private static DatabaseManager _singleton;
    private readonly ILiteCollection<Coupon> _cp;
    private static readonly string _dbPath = Path.Combine("Database", "Coupons.db");

    /// <summary>
    /// The number of unique CouponPages in the database
    /// </summary>
    public int Count => _cp.Count();

    public DatabaseManager()
    {
        if (_singleton != null) 
            throw new InvalidOperationException("Cannot create more than one instance of the DatabaseManager");

        if (!Directory.Exists("Database")) 
            Directory.CreateDirectory("Database");
        if (!File.Exists(_dbPath)) 
            File.Create(_dbPath);

        // Get a collection (or create, if doesn't exist)
        var _db = new LiteDatabase($@"Filename={_dbPath}; Connection=shared");
        _cp = _db.GetCollection<Coupon>("couponPages");
        _singleton = this;
    }

    /// <summary>
    /// Inserts a CouponPage into the database
    /// </summary>
    /// <param name="cp"></param>
    /// <returns></returns>
    public BsonValue Insert(Coupon cp)
    {
        return _cp.Insert(cp);
    }

    /// <summary>
    /// Inserts a CouponPage into the database
    /// </summary>
    /// <param name="cp"></param>
    /// <returns></returns>
    public BsonValue InsertBulk(IEnumerable<Coupon> cp)
    {
        return _cp.InsertBulk(cp);
    }

    /// <summary>
    /// Finds a CouponPage by its BsonValue id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Coupon FindById(BsonValue id)
    {
        return _cp.FindById(id);
    }

    /// <summary>
    /// Returns all CouponPages that match the predicate
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public IEnumerable<Coupon> Find(Expression<Func<Coupon, bool>> predicate)
    {
        return _cp.Find(predicate);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Coupon> FindAll()
    {
        return _cp.FindAll();
    }

    /// <summary>
    /// Updates a CouponPage entry in the database
    /// </summary>
    /// <param name="cp"></param>
    /// <returns></returns>
    public bool Update(Coupon cp)
    {
        return _cp.Update(cp);
    }

    /// <summary>
    /// Deletes a CouponPage by its BsonValue id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool Delete(BsonValue id)
    {
        return _cp.Delete(id);
    }

    /// <summary>
    /// Deletes all CouponPages in this database. Careful!
    /// </summary>
    /// <returns></returns>
    public int DeleteAll()
    {
        return _cp.DeleteAll();
    }
}