using System.Xml.Serialization;

namespace ItemSeeder.Wowhead
{
    public class Icon
    {
        [XmlAttribute(AttributeName = "displayId")]
        public int DisplayId { get; set; }
        [XmlText]
        public string IconName { get; set; }
    }
}