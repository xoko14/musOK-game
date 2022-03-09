using System.Xml;
using System.Xml.Serialization;

[XmlRoot(ElementName = "chart")]
public class SongInfo
{
	[XmlElement(ElementName = "title")]
	public string title;

	[XmlElement(ElementName = "artist")]
	public string artist;

	[XmlElement(ElementName = "easy")]
	public ChartFile easy;

	[XmlElement(ElementName = "normal")]
	public ChartFile normal;

	[XmlElement(ElementName = "hard")]
	public ChartFile hard;

	[XmlElement(ElementName = "music")]
	public ChartFile music;

	[XmlElement(ElementName = "jacket")]
	public ChartFile jacket;
}

public class ChartFile{
	[XmlAttribute(AttributeName = "file")]
	public string file;
}

public class ChartDifficulty: ChartFile{

}