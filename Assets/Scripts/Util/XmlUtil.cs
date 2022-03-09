using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;

public class XmlUtil
{
    public static T ImportXml<T>(string path)
    {
        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (var stream = new FileStream(path, FileMode.Open))
            {
                return (T)serializer.Deserialize(stream);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Exception importing xml file: " + e);
            return default;
        }
    }
}