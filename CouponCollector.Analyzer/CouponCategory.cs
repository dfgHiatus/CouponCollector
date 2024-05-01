namespace CouponsCollector.Analyzer;

public static class CouponCategory
{
    public static readonly int[] CategoryRepresentation = Array.ConvertAll((int[])Enum.GetValues(typeof(Category)), Convert.ToInt32).ToArray();

    public enum Category
    {
        Superfoods = 338,
        American = 199,
        Arcade = 210,
        Asian = 78,
        Automotive = 181,
        Barber = 1,
        Beauty = 358,
        Breakfast_One = 348,
        Breakfast_Two = 81,
        Calzones = 245,
        Cannabis = 335,
        Casino = 279,
        Cookies = 331,
        Fitness = 128,
        CinammonRolls = 352,
        Health = 320,
        Horseback = 323,
        HotChicken = 339,
        RockClimbing = 114,
        InteractiveMuseum = 355,
        Italian = 89,
        KoreanSushi = 353,
        Mail = 116,
        Mediterranean = 294,
        Mexican = 90,
        Salon = 173,
        Nutrition = 7,
        PitasGreek = 93,
        PizzaWings = 94,
        PotteryPainting = 74,
        Pretzels = 329,
        SandwichesSubsSalads = 95,
        SmokeVape = 357,
        Tanning = 112,
        TattoosPiercings = 12,
        Tea = 340,
        TexMex = 354,
        VegetarianVegan = 113,
        Waxing = 14,
        None = 0,
        All_Breakfast = Breakfast_One | Breakfast_Two,
        All_Health = Fitness | Health | Nutrition,
        All_Hygiene = Waxing | Barber | Beauty | Salon,
        All_Entertainment = Casino | Horseback | RockClimbing | InteractiveMuseum | PotteryPainting | Arcade,
        All_Food =
            Superfoods |
            American |
            Asian |
            Breakfast_One |
            Breakfast_Two |
            Calzones |
            Cookies |
            CinammonRolls |
            HotChicken |
            Italian |
            KoreanSushi |
            Mediterranean |
            Mexican |
            PitasGreek |
            PizzaWings |
            Pretzels |
            SandwichesSubsSalads |
            TexMex |
            VegetarianVegan,
    }
}
