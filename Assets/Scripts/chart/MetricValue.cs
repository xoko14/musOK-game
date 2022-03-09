public class MetricValue{
    public int Numerator {get; set;}
    public int Denominator {get; set;}
    
    public float Position {get; set;}

    public MetricValue(int n, int d, int b, int p)
    {
        Numerator = n;
        Denominator = d;
        Position = b + p/1920f;
    }
}