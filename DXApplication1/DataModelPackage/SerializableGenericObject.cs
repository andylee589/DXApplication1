using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
namespace DXApplication1.DataModelPackage
{
    public sealed class SerializableGenericObject<T> : IXmlSerializable
    {
        public T Value { get; set; }
        public SerializableGenericObject()
        {

        }

        public SerializableGenericObject(T t)
        {
            this.Value = t;
        }


#region IXMLSerializable override method

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            if (!reader.HasAttributes)
            {
                throw new FormatException("expected a type attribute!");
            }
            string type = reader.GetAttribute("type");
            reader.Read();
            if (type.Equals("null"))
            {
                return;
            }
            XmlSerializer serializer = new XmlSerializer(Type.GetType(type));
            this.Value = (T)serializer.Deserialize(reader);
            reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            if (Value == null)
            {
                writer.WriteAttributeString("type", "null");
                return;
            }

            Type type = this.Value.GetType();
            XmlSerializer serializer = new XmlSerializer(type);
            writer.WriteAttributeString("type",type.AssemblyQualifiedName);
            serializer.Serialize(writer, this.Value);
        }
    }
#endregion

}
