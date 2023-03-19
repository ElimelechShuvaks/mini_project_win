using System.Xml.Linq;
using System.Xml.Serialization;

namespace Dal;

public static class XmlTools
{
    /// <summary>
    /// help function to save lists into a xml file with a given path by Xml Serializer.
    /// </summary>
    public static void SaveListToXMLSerializer<T>(List<T?> list, string path) where T : struct
    {
		try
		{
            using FileStream file = new(path, FileMode.Create, FileAccess.Write, FileShare.None);
            XmlSerializer serializer = new(typeof(List<T?>));

            serializer.Serialize(file, list);
        }
        catch (Exception ex)
		{
            throw new Exception($"fail to create xml file: {path}", ex);
        }
    }

    /// <summary>
    /// help function to load xml file with a given path into a list by Xml Serializer. 
    /// </summary>
    public static List<T?> LoadListFromXMLSerializer<T>(string path) where T : struct
    {
        try
        {
            if (!File.Exists(path))
                return new List<T?>();

            using FileStream file = new FileStream(path, FileMode.Open);
            XmlSerializer serializer = new(typeof(List<T?>));

            return serializer.Deserialize(file) as List<T?> ?? new List<T?>();
        }
        catch (Exception ex)
        {
            throw new Exception($"fail to load xml file: {path}", ex);
        }
    }

    /// <summary>
    /// help function to save XElemelnt into a xml file with a given path.
    /// </summary>
    public static void SaveXElementToXelFile(XElement root, string path)
    {
        try
        {
            root.Save(path);
        }
        catch (Exception ex)
        {
            throw new Exception($"fail to create xml file: {path}", ex);
        }
    }

    /// <summary>
    /// help function to load xml file with a given path into an XElement, or creat an XElement if he dousen't exsit in path.
    /// </summary>
    public static XElement LoadListFromXElement(string path, string? content = null)
    {
        try
        {
            if (File.Exists(path))
                return XElement.Load(path);

            XElement root = new XElement(content ?? throw new Exception("cann't creat a new empty XElement"));
            root.Save(path);
            return root;
        }
        catch (Exception ex)
        {
            throw new Exception($"fail to load xml file: {path}", ex);
        }
    }

    public static int GetConfigNumber(string path, string elementName)
    {
        int configNumber;
        XElement element = LoadListFromXElement(path);

        configNumber = Convert.ToInt32(element.Element(elementName)!.Value) + 1;
        element.Element(elementName)!.Value = configNumber.ToString();

        SaveXElementToXelFile(element, path);

        return configNumber;
    }
}