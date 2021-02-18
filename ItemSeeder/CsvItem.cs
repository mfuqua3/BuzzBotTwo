using FileHelpers;

namespace ItemSeeder
{
    [DelimitedRecord(",")]
    public class CsvItem
    {
        public int Entry { get; set; }
        public string Name { get; set; }
    }
}