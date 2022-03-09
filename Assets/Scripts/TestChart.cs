using System.IO;
public class TestChart{
    public static string Load(string fileName){
        string text = File.ReadAllText(fileName);
        return text;
    }

}