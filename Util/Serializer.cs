using System.IO;
using System.Runtime.Serialization;

namespace Nova_Engine.Util;

public class Serializer
{
    public static void Serialize(object obj, string path)
    {
        var serializer = new DataContractSerializer(obj.GetType());
        using (FileStream fileStream = new FileStream(path, FileMode.Create))
        {
            serializer.WriteObject(fileStream, obj);
        }
    }

    public static T? Deserialize<T>(string path)
    {
        var serializer = new DataContractSerializer(typeof(T));
        using (FileStream fileStream = new FileStream(path, FileMode.Open))
        {
            var instance = serializer.ReadObject(fileStream);
            if (instance != null && instance is T)
                return (T) instance;
        }
        return default;
    }
}