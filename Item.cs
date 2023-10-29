using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

public abstract class Item
{
    public string Name { get; set; }
    public decimal Price { get; set; }

    public Item(string name)
    {
        Name = name;
    }

    public abstract decimal CalculatePrice();

}
    public class Pizza : Item
    {   
        public string Size { get; set; }
        public string Type { get; set; }
        public Pizza(string name, string size, string type, decimal price) : base(name)
        {
            Size = size;
            Type = type;
            Price = price;
        }

        public override decimal CalculatePrice()
        {
            decimal price = 0;

            switch (Size)
            {
                case "petite":
                    price += 8;
                    break;
                case "moyenne":
                    price += 10;
                    break;
                case "grande":
                    price += 12;
                    break;
            }

            switch (Type)
            {
                case "margarita":
                    price += 2;
                    break;
                case "végétarienne":
                    price += 3;
                    break;
                case "reine":
                    price += 4;
                    break;
            }

            return price;
        }

    }

    public class Drink : Item
    {   
        public int Volume { get; set; }
        public string Type { get; set; }
        public Drink(string name, int volume, string type, decimal price) : base(name)
        {
            Volume = volume;
            Type = type;
            Price = price;
        }

        public override decimal CalculatePrice()
        {
            decimal price = 0;

            switch (Type)
            {
                case "cola":
                    price += 2;
                    break;
                case "jus d'orange":
                    price += 2.5M;
                    break;
            }

            return price * Volume;
        }
    }

public class ItemConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(Item);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        JObject jObject = JObject.Load(reader);
        if (jObject["Name"].Value<string>() == "pizza")
        {
            return jObject.ToObject<Pizza>();
        }
        else if (jObject["Name"].Value<string>() == "boisson")
        {
            return jObject.ToObject<Drink>();
        }
        throw new InvalidOperationException("ItemType non reconnu");
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}
