public class HaversineDistanceCalculator
{
    public static double CalculateHaversineDistance(Coordinate coord1, Coordinate coord2)
    {
        // Radius of the Earth in kilometers
        const double earthRadius = 6371;

        // Convert latitude and longitude from degrees to radians
        double lat1 = ToRadians(coord1.Latitude);
        double lon1 = ToRadians(coord1.Longitude);
        double lat2 = ToRadians(coord2.Latitude);
        double lon2 = ToRadians(coord2.Longitude);

        // Calculate the Haversine distance
        double distance = CalculateHaversineDistanceInKilometers(lat1, lon1, lat2, lon2, earthRadius);

        return distance;
    }

    private static double CalculateHaversineDistanceInKilometers(double lat1, double lon1, double lat2, double lon2, double radius)
    {
        double dlon = lon2 - lon1;
        double dlat = lat2 - lat1;
        double a = Math.Pow(Math.Sin(dlat / 2), 2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Pow(Math.Sin(dlon / 2), 2);
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        double distance = radius * c;
        return distance;
    }

    private static double ToRadians(double degrees)
    {
        return degrees * Math.PI / 180;
    }
}