using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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


        private int GetUserId () => int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!); 
        public async Task<ServiceResponse<List<GetCharacterDTO>>> addNewCharacter(CreateCharacterDTO addCharacter){

            var serviceResponse = new ServiceResponse<List<GetCharacterDTO>>(); 
            var character = _mapper.Map<Character>(addCharacter); 

            character.User = await _context.Users.FirstOrDefaultAsync(c => c.Id == GetUserId()); 
 
            _context.Characters.Add(character); 
            await _context.SaveChangesAsync(); 

            serviceResponse.Data = 
                await _context.Characters
                    .Where(c => c.User!.Id == GetUserId())
                    .Select(c => _mapper.Map<GetCharacterDTO>(c))
                    .ToListAsync(); 
            return serviceResponse; 
        }

        public async Task<ServiceResponse<List<GetCharacterDTO>>> GetAllCharacters(){
            
            var serviceResponse = new ServiceResponse<List<GetCharacterDTO>>(); 
            var dbCharacters = await _context.Characters
                .Include(c => c.Weapon)
                .Include(C => C.Skills)
                .Where(c => c.User!.Id == GetUserId()).ToListAsync();
            serviceResponse.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDTO>(c)).ToList(); 
            return serviceResponse; 
        }

        public async Task<ServiceResponse<GetCharacterDTO>> GetCharacterById(int id){
            
            var serviceResponse = new ServiceResponse<GetCharacterDTO>(); 
            var dbCharacters = await _context.Characters
                .Include(c => c.Weapon)
                .Include(C => C.Skills)
                .FirstOrDefaultAsync(c => c.id == id && c.User!.Id == GetUserId());        
            serviceResponse.Data = _mapper.Map<GetCharacterDTO>(dbCharacters);   
            return serviceResponse; 
        }

        public async Task<ServiceResponse<GetCharacterDTO>> UpdateCharacter(UpdateCharacterDTO updateCharacterDTO){
           
           var serviceResponse = new ServiceResponse<GetCharacterDTO>(); 
         
           try{
            var character = await _context.Characters
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.id == updateCharacterDTO.id); 
            if (character is null || character.User!.Id != GetUserId())
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
            var character = await _context.Characters.FirstOrDefaultAsync(c => c.id == id && c.User!.Id == GetUserId()); 
            if (character is null)
                    throw new Exception($"Character with id '{id}' not found."); 
            
            _context.Characters.Remove(character); 

            await _context.SaveChangesAsync(); 

            serviceResponse.Data = await _context.Characters
                .Where(c => c.User!.Id == GetUserId())    
                .Select(c => _mapper.Map<GetCharacterDTO>(c)).ToListAsync(); 

           } catch (Exception ex){
                serviceResponse.Succcess = false; 
                serviceResponse.Message = ex.Message; 
           }
            return serviceResponse; 
             
        }

        public async Task<ServiceResponse<GetCharacterDTO>> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill)
        {
            var response = new ServiceResponse<GetCharacterDTO>(); 

            try {

                var character = await _context.Characters
                    .Include(c => c.Weapon)
                    .Include(c => c.Skills)
                    .FirstOrDefaultAsync(c => c.id == newCharacterSkill.CharacterId &&
                     c.User!.Id == GetUserId());  

                if (character is null){
                    response.Succcess = false; 
                    response.Message = "Character not found"; 
                    return response; 
                }

                var skill = await _context.Skills.FirstOrDefaultAsync(s => s.Id == newCharacterSkill.SkillId); 

                if (skill is null){
                    response.Succcess = false; 
                    response.Message = "Skill not found"; 
                    return response; 
                }

                character.Skills!.Add(skill); 
                await _context.SaveChangesAsync(); 
                response.Data = _mapper.Map<GetCharacterDTO>(character); 

            } catch (Exception ex) {
                response.Succcess = false; 
                response.Message = ex.Message; 
            }
            return response; 
        }
    }
}   