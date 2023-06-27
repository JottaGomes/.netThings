
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Controllers
{   

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase {
    
        private ICharacterServices _characterService { get; }

    public CharacterController (ICharacterServices characterService){
            _characterService = characterService;
       
    }

        [HttpGet("GetAll")]  
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDTO>>>> Get(){
            
            return Ok(await _characterService.GetAllCharacters()); 
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDTO>>> GetSingle(int id){
            return Ok(await _characterService.GetCharacterById(id)); 
        }

        [HttpPost("addCharacters")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDTO>>>> AddCharacter(CreateCharacterDTO newCharacter){
            
            return Ok(await _characterService.addNewCharacter(newCharacter)); 
        }

        [HttpPut("updateCharacter")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDTO>>>> UpdateCharacter(UpdateCharacterDTO updateCharacterDTO){
            
            var response = await _characterService.UpdateCharacter(updateCharacterDTO); 
            if (response.Data is null){
                return NotFound (response); 
            }
            return Ok(response); 
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDTO>>> DeleteCharacter(int id){
            var response = await _characterService.DeleteCharacter(id); 
            if (response.Data is null){
                return NotFound (response); 
            }
            return Ok(response);  
        }

        [HttpPost("Skill")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDTO>>> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill){
            
            return Ok(await _characterService.AddCharacterSkill(newCharacterSkill)); 
        }

    }
}