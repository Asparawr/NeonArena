using System.IO;
using System.Xml;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Scripting;
using System.Collections.Generic;
using System.Text;

public static class Serializer
{
    public static int key = 53276982;
    public static string Serialize<T>(this T obj)
    {
        var serializer = new DataContractSerializer(obj.GetType());
        using (var writer = new StringWriter())
        using (var stm = new XmlTextWriter(writer))
        {
            serializer.WriteObject(stm, obj);
            return EncryptDecrypt(writer.ToString());
        }
    }
    public static T Deserialize<T>(this string serialized)
    {
        serialized = EncryptDecrypt(serialized);
        var serializer = new DataContractSerializer(typeof(T));
        using (var reader = new StringReader(serialized))
        using (var stm = new XmlTextReader(reader))
        {
            return (T)serializer.ReadObject(stm);
        }
    }

    public static string EncryptDecrypt(string textToEncrypt)
    {
        StringBuilder inSb = new StringBuilder(textToEncrypt);
        StringBuilder outSb = new StringBuilder(textToEncrypt.Length);
        char c;
        for (int i = 0; i < textToEncrypt.Length; i++)
        {
            c = inSb[i];
            c = (char)(c ^ key);
            outSb.Append(c);
        }
        return outSb.ToString();
    }
}
