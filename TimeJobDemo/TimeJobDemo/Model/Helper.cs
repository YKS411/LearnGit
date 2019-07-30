using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeJobDemo.Model
{
    public class Helper
    {
        private static Dictionary<string, Shop> shops = new Dictionary<string, Shop>();

        public static void SetShopsList(List<Shop> shopList)
        {
            foreach(var item in shopList)
            {
                if (!shops.ContainsKey(item.Name))
                {
                    shops.Add(item.Name, item);
                }
            }
        }

        public static List<Shop> GetShopList()
        {
            var dataList = new List<Shop>();
            if (shops.Count > 0)
            {
                foreach(var item in shops)
                {
                    dataList.Add(item.Value);
                }
            }
            return dataList;
        }


    }
}
