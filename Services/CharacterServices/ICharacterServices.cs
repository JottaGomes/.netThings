using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Services.CharacterServices
{
    public interface ICharacterServices
    {
    Task<ServiceResponse<List<GetCharacterDTO>>> GetAllCharacters(); 
    Task<ServiceResponse<GetCharacterDTO>> GetCharacterById (int id); 
    Task<ServiceResponse<List<GetCharacterDTO>>> addNewCharacter(CreateCharacterDTO addCharacter); 
    Task<ServiceResponse<GetCharacterDTO>> UpdateCharacter(UpdateCharacterDTO updateCharacterDTO); 
    Task<ServiceResponse<List<GetCharacterDTO>>> DeleteCharacter(int id); 
    Task<ServiceResponse<GetCharacterDTO>> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill); 
    }
}