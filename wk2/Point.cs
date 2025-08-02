public class Point
{
    // Two fields
    private double x, y;
    public double X { get { return x; } set { x = value; } }
    public double Y { get { return y; } set { y = value; } }

    // Constructor 
    public Point(double x, double y)
    {
        this.X = x;
        this.Y = y;
    }

    // Separate get and set methods for some reason
    public double GetX()
    {
        return X;
    }

    public double GetY()
    {
        return Y;
    }

    // Set method
    public void Set(double x, double y)
    {
        this.X = x;
        this.Y = y;
    }

    // Distance method
    public double DistanceTo(Point pt)
    {
        return Math.Sqrt(Math.Pow((pt.X - this.X), 2) + Math.Pow((pt.Y - this.Y), 2));
    }
}

