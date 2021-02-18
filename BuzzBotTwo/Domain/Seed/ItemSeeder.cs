using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using BuzzBotTwo.Domain.Entities;
using BuzzBotTwo.Properties;
using Newtonsoft.Json;

namespace BuzzBotTwo.Domain.Seed
{
    public class ItemSeeder:IDataSeeder<Item>
    {
        public IEnumerable<Item> Data => LoadFromResources();

        public IEnumerable<Item> LoadFromResources()
        {
            var json = Encoding.UTF8.GetString(Resources.Items);
            var items = JsonConvert.DeserializeObject<List<Item>>(json);
            return items.Distinct(new Comparer());
        }

        private class Comparer : IEqualityComparer<Item>
        {
            public bool Equals(Item x, Item y)
            {
                return x.Id == y.Id;
            }

            public int GetHashCode(Item obj)
            {
                return obj.Id.GetHashCode();
            }
        }


    }
}