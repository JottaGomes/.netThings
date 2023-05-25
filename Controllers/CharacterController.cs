
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase {
        private static List<Character> characters = new List<Character>{
            new Character (),
            new Character {id = 1, Name = "Vegeta"}   
        }; 

        [HttpGet("GetAll")]  
        public ActionResult<List<Character>> Get(){
            return Ok(characters); 
        }

        [HttpGet("{id}")]
        public ActionResult<Character> GetSingle(int id){
            return Ok(characters.FirstOrDefault(c => c.id == id)); 
        }

        [HttpPost("addCharacters")]
        public ActionResult<List<Character>> AddCharacter(Character newCharacter){
            characters.Add(newCharacter); 
            return Ok(characters); 
        }
    }
}