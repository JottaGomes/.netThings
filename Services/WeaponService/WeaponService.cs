using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dtos.Weapon;

namespace Services.WeaponService
{
    public class WeaponService : IWeaponService
    {
        private readonly DataContext _context; 

        private readonly IHttpContextAccessor _accessor; 

        private readonly IMapper _mapper; 

        public WeaponService(DataContext context, IHttpContextAccessor accessor, IMapper mapper){
            _context = context;
            _accessor = accessor; 
            _mapper = mapper; 
        }

        public async Task<ServiceResponse<GetCharacterDTO>> AddWeapon(AddWeaponDto newWeapon)
        {
            var response = new ServiceResponse<GetCharacterDTO>(); 
            try {

                var character = await _context.Characters
                    .FirstOrDefaultAsync(c => c.id == newWeapon.CharacterId && 
                    c.User!.Id == int.Parse(_accessor.HttpContext!
                        .User
                        .FindFirstValue(ClaimTypes.NameIdentifier)!)); 
    	        if (character is null){
                    response.Succcess = false; 
                    response.Message = "Character not found"; 
                    return response; 
                }

                var weapon = new Weapon{
                    Name = newWeapon.Name,
                    Damage = newWeapon.Damage,
                    Character = character  

                };

                _context.Weapons.Add(weapon); 
                await _context.SaveChangesAsync(); 

                response.Data = _mapper.Map<GetCharacterDTO>(character); 

            } catch (Exception ex){
                response.Succcess = false; 
                response.Message = ex.Message; 
            }

            return response; 
        }
    }
}