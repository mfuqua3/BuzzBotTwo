using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using ItemSeeder.Properties;
using ItemSeeder.Wowhead;
using Newtonsoft.Json;

namespace ItemSeeder
{
    class Program
    {
        private class Comparerer : IEqualityComparer<Item>
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
        static async Task Main(string[] args)
        {
            var json = await File.ReadAllTextAsync("Items.json");
            var objects = JsonConvert.DeserializeObject<List<Item>>(json);
            var objTrimmed = objects.Distinct(new Comparerer()).ToList();
            var newJson = JsonConvert.SerializeObject(objTrimmed, Formatting.Indented);
            await File.WriteAllTextAsync("Items.json", newJson);
            //var itemString = Resources.items;

            ////Handle any manual corrections we may need to do.
            //itemString = itemString.Replace("entry,name\r\n", "");
            //itemString = itemString.Replace("Monster - CsvItem, Lantern - Round", "Monster - CsvItem Lantern - Round");
            //itemString = itemString.Replace("Thunderfury, Blessed Blade of the Windseeker", "Thunderfury Blessed Blade of the Windseeker");
            //itemString = itemString.Replace("Zin'rokh, Destroyer of Worlds", "Zin'rokh Destroyer of Worlds");
            //itemString = itemString.Replace("\r\n", ",");

            ////Split the string on the ','
            //string[] itemsArray = itemString.Split(',');

            ////Create a dictionary to store all of the determined values.
            //List<CsvItem> itemsList = new List<CsvItem>();
            //for (int i = 0; i < itemsArray.Length; i = i + 2)
            //{
            //    //Add to the convertedItemsArray the list of 
            //    itemsList.Add(new CsvItem()
            //    {
            //        Entry = int.TryParse(itemsArray[i], out int iEntry) ? iEntry : 0,
            //        Name = (itemsArray.Length < i + 1) ? "N/a" : itemsArray[i + 1],
            //    });
            //}

            ////instantiate the WowheadClient
            //var wowheadClient = new WowheadClient();

            //var items = new List<Item>();
            //foreach (var item in itemsList)
            //{
            //    var wowheadItem = await wowheadClient.Get(item.Entry.ToString());
            //    if (wowheadItem.Item == null) continue;
            //    Console.WriteLine($"Adding {item.Name} to database");
            //    var dbItem = new Item
            //    {
            //        Id = item.Entry,
            //        Name = wowheadItem.Item.Name,
            //        Quality = wowheadItem.Item.Quality.Text,
            //        Icon = wowheadItem.Item.Icon.IconName,
            //        Class = Convert.ToInt32(wowheadItem.Item.Class.Id),
            //        Subclass = Convert.ToInt32(wowheadItem.Item.Subclass.Id)
            //    };
            //    items.Add(dbItem);
            //}

            //var jsonFile = JsonConvert.SerializeObject(items);
            //await File.WriteAllTextAsync("Items.json", jsonFile);
        }


    }
}
