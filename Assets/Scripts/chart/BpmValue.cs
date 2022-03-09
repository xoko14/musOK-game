public class BpmValue{
    public float Value {get; set;}
    public float Position {get; set;}

    public BpmValue(float v, int b, int pos)
    {
        Value = v;
        Position = b + pos/1920f;
    }
    
}