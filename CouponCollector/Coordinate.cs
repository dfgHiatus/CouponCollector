public class Coordinate
{
    private double latitude;
    private double longitude;

    public double Latitude
    {
        get { return latitude; }
        private set { latitude = value; }
    }

    public double Longitude
    {
        get { return longitude; }
        private set { longitude = value; }
    }

    public Coordinate(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }
}
