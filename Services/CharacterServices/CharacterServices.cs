using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Services.CharacterServices
{
    public class CharacterServices : ICharacterServices
    {
        
        private readonly IMapper _mapper; 
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CharacterServices(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor){

            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper; 
        }

        public async Task<ServiceResponse<List<GetCharacterDTO>>> addNewCharacter(CreateCharacterDTO addCharacter){

            var serviceResponse = new ServiceResponse<List<GetCharacterDTO>>(); 
            var character = _mapper.Map<Character>(addCharacter); 

            _context.Characters.Add(character); 
            await _context.SaveChangesAsync(); 

            serviceResponse.Data = 
                await _context.Characters.Select(c => _mapper.Map<GetCharacterDTO>(c)).ToListAsync(); 
            return serviceResponse; 
        }

        public async Task<ServiceResponse<List<GetCharacterDTO>>> GetAllCharacters(int userId){
            
            var serviceResponse = new ServiceResponse<List<GetCharacterDTO>>(); 
            var dbCharacters = await _context.Characters.Where(c => c.User!.Id == userId).ToListAsync();
            serviceResponse.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDTO>(c)).ToList(); 
            return serviceResponse; 
        }

        public async Task<ServiceResponse<GetCharacterDTO>> GetCharacterById(int id){
            
            var serviceResponse = new ServiceResponse<GetCharacterDTO>(); 
            var dbCharacters = await _context.Characters.FirstOrDefaultAsync(c => c.id == id);        
            serviceResponse.Data = _mapper.Map<GetCharacterDTO>(dbCharacters);   
            return serviceResponse; 

            
        }

        public async Task<ServiceResponse<GetCharacterDTO>> UpdateCharacter(UpdateCharacterDTO updateCharacterDTO){
           
           var serviceResponse = new ServiceResponse<GetCharacterDTO>(); 
         
           try{
            var character = await _context.Characters.FirstOrDefaultAsync(c => c.id == updateCharacterDTO.id); 
            if (character is null)
                    throw new Exception($"Character with id '{updateCharacterDTO.id}' not found."); 

            character.Name = updateCharacterDTO.Name; 
            character.hitPoints = updateCharacterDTO.hitPoints; 
            character.defence = updateCharacterDTO.defence; 
            character.strength = updateCharacterDTO.strength; 
            character.intelligence = updateCharacterDTO.intelligence; 
            character.Class = updateCharacterDTO.Class;

            await _context.SaveChangesAsync(); 
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
            var character = await _context.Characters.FirstOrDefaultAsync(c => c.id == id); 
            if (character is null)
                    throw new Exception($"Character with id '{id}' not found."); 
            
            _context.Characters.Remove(character); 

            await _context.SaveChangesAsync(); 

            serviceResponse.Data = await _context.Characters.Select(c => _mapper.Map<GetCharacterDTO>(c)).ToListAsync(); 

           } catch (Exception ex){
                serviceResponse.Succcess = false; 
                serviceResponse.Message = ex.Message; 
           }
            return serviceResponse; 
             
        }
    }
}