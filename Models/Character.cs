using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models{

    
    public class Character{
     
    public int id { get; set; }
    public String Name { get; set; } = "Sangoku"; 
    public int hitPoints { get; set; } = 100; 
    public int strength { get; set; } = 10; 
    public int defence { get; set; } = 10; 
    public int intelligence { get; set; } = 10; 
    public rpgClass Class { get; set; } = rpgClass.Knight;  
    public User? User { get; set; }
    public Weapon? Weapon { get; set; }
    public List<Skill>? Skills { get; set; }
    public int Victories { get; set; }
    public int Fights { get; set; }
    public int Defeats { get; set; }

    }
  
    
}