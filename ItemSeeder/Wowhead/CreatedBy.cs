using System.Xml.Serialization;

namespace ItemSeeder.Wowhead
{
    [XmlRoot(ElementName = "createdBy")]
    public class CreatedBy
    {
        [XmlElement(ElementName = "spell")]
        public Spell Spell { get; set; }
    }
}