using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dtos.Fighter;

namespace Services.FightService
{
    public class FightServices : IfightService
    {
        private readonly DataContext _context; 
        public FightServices(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto request)
        {
            var response = new ServiceResponse<AttackResultDto>(); 

            try {

                var attacker = await _context.Characters
                    .Include(c => c.Weapon)
                    .FirstOrDefaultAsync(c => c.id == request.AttackerId); 
                
                var opponent = await _context.Characters
                    .FirstOrDefaultAsync(c => c.id == request.OponentId); 

                if (attacker is null || opponent is null || attacker.Weapon is null){
                    throw new Exception("Someting fishy is going on here ... haha "); 
                }

                int damage = attacker.Weapon.Damage + (new Random().Next(attacker.strength)); 
                damage -= new Random().Next(opponent.Defeats); 

                if (damage > 0){
                    opponent.hitPoints -= damage; 
                }

                if (opponent.hitPoints <= 0){
                    response.Message = $"{opponent.Name} has been defeated!"; 
                }

                response.Data = new AttackResultDto {
                    Attacker = attacker.Name, 
                    Opponent = opponent.Name, 
                    AttackerHP = attacker.hitPoints, 
                    OponnetHP = opponent.hitPoints, 
                    Damage = damage
                }; 

                await _context.SaveChangesAsync(); 
            } catch (Exception ex) {
                response.Succcess = false; 
                response.Message = ex.Message; 
            }

            return response; 
        }
    }
}