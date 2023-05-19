using System.Text.Json.Serialization;

namespace Models
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum rpgClass
    {
        Mage = 1,

        Knight = 2, 

        Cleric = 3, 
     
    }
}