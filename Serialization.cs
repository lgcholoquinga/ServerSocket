using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Serialization
{
    public class BinarySerialization
    {
        public static byte[] Serializate(object toSerialize)
        {
            MemoryStream memory = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(memory, toSerialize);
            return memory.ToArray();
        }
        public static object Deserializate(byte[] data)
        {
            MemoryStream memory = new MemoryStream(data);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Binder = new CurrenAssemblyDeserializationBinder();
            return formatter.Deserialize(memory);
        }
    }
    public class CurrenAssemblyDeserializationBinder : SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            return Type.GetType(String.Format("{0}, {1}", typeName, Assembly.GetExecutingAssembly().FullName));
        }
    }
}