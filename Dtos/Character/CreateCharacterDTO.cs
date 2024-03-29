using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dtos.Character
{
    public class CreateCharacterDTO
    {
    public int id { get; set; }
    public String Name { get; set; } = "Sangoku"; 
    public int hitPoints { get; set; } = 100; 
    public int strength { get; set; } = 10; 
    public int defence { get; set; } = 10; 
    public int intelligence { get; set; } = 10; 
    public rpgClass Class { get; set; } = rpgClass.Knight;  
    }
}