using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Services.CharacterServices
{
    public class CharacterServices : ICharacterServices
    {

        private static List<Character> characters = new List<Character>{
            new Character (),
            new Character {id = 1, Name = "Vegeta"}   
        }; 
        
        private readonly IMapper _mapper; 

        public CharacterServices(IMapper mapper){
            _mapper = mapper; 
        }

        public async Task<ServiceResponse<List<GetCharacterDTO>>> addNewCharacter(CreateCharacterDTO addCharacter){

            var serviceResponse = new ServiceResponse<List<GetCharacterDTO>>(); 
            var character = _mapper.Map<Character>(addCharacter);
            character.id = characters.Max(c => c.id) + 1;  
            characters.Add(character); 
            serviceResponse.Data = characters.Select(c => _mapper.Map<GetCharacterDTO>(c)).ToList(); 
            return serviceResponse; 
        }

        public async Task<ServiceResponse<List<GetCharacterDTO>>> GetAllCharacters(){
            
            var serviceResponse = new ServiceResponse<List<GetCharacterDTO>>(); 
            serviceResponse.Data = characters.Select(c => _mapper.Map<GetCharacterDTO>(c)).ToList(); 
            return serviceResponse; 
        }

        public async Task<ServiceResponse<GetCharacterDTO>> GetCharacterById(int id){
            
            var serviceResponse = new ServiceResponse<GetCharacterDTO>(); 
            var character = characters.FirstOrDefault(c => c.id == id);        
            serviceResponse.Data = _mapper.Map<GetCharacterDTO>(character);   
            return serviceResponse; 

            
        }

        public async Task<ServiceResponse<GetCharacterDTO>> UpdateCharacter(UpdateCharacterDTO updateCharacterDTO){
           
           var serviceResponse = new ServiceResponse<GetCharacterDTO>(); 
         
           try{
            var character = characters.FirstOrDefault(c => c.id == updateCharacterDTO.id); 
            if (character is null)
                    throw new Exception($"Character with id '{updateCharacterDTO.id}' not found."); 

            _mapper.Map(updateCharacterDTO, character); 

            character.Name = updateCharacterDTO.Name; 
            character.hitPoints = updateCharacterDTO.hitPoints; 
            character.defence = updateCharacterDTO.defence; 
            character.streng = updateCharacterDTO.streng; 
            character.intelligence = updateCharacterDTO.intelligence; 
            character.Class = updateCharacterDTO.Class;

            serviceResponse.Data = _mapper.Map<GetCharacterDTO>(character); 

           } catch (Exception ex){
                serviceResponse.Succcess = false; 
                serviceResponse.Message = ex.Message; 
           }
            return serviceResponse; 
            
        }

        public async Task<ServiceResponse<List<GetCharacterDTO>>> DeleteCharacter(int id){ 
           var serviceResponse = new ServiceResponse<List<GetCharacterDTO>>(); 
         
           try{
            var character = characters.First(c => c.id == id); 
            if (character is null)
                    throw new Exception($"Character with id '{id}' not found."); 
            
            characters.Remove(character); 

            serviceResponse.Data = characters.Select(c => _mapper.Map<GetCharacterDTO>(c)).ToList(); 

           } catch (Exception ex){
                serviceResponse.Succcess = false; 
                serviceResponse.Message = ex.Message; 
           }
            return serviceResponse; 
             
        }
    }
}