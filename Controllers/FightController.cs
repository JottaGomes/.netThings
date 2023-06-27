using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Controllers
{
   
    [ApiController]
    [Route("[controller]")]
    public class FightController : ControllerBase 
    {
        
        private readonly IfightService _fightService; 
        public FightController(IfightService fightService){
            _fightService = fightService;    
        }



    }
}