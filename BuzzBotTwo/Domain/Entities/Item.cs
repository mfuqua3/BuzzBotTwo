namespace BuzzBotTwo.Domain.Entities
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public int QualityValue { get; set; }
        public string Quality { get; set; }
        public int Class { get; set; }
        public int? Subclass { get; set; }
        public int ItemLevel { get; set; }
        public int InventorySlot { get; set; }

        public override string ToString()
        {
            return $"{Id}: {Name}";
        }
    }
}