// TODO: Fix 'TEMP's

using System;

namespace Store
{
    public static class StoreItems
    {
        public enum StoreItemType
        {
            Pesticide,
            Seeds
        }

        public static int TypeToCost(StoreItemType type) => type switch
        {
            StoreItemType.Seeds => Seeds.Cost,
            StoreItemType.Pesticide => Pesticide.Cost,
            _ => 0
        };
        
        public static class Pesticide
        {
            public static string Name = "Pesticide";
            public const int Cost = 350;
            public static string Description = "TEMP";
            public static string[] Pros = {
                "Increases Crop Yields"
            };
            public static string[] Cons = {
                "TEMP"
            };
        }

        public static class Seeds
        {
            public static string Name => "Seeds";
            public const int Cost = 50;
            public static string Description => "Used to plant crops";
        }
    }
}


