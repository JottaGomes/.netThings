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

        public async Task<ServiceResponse<FightResultDto>> Fight(FightRequestDto request)
        {
           var response = new ServiceResponse<FightResultDto>{
            Data = new FightResultDto()
           }; 

           try {

            var characters = await _context.Characters
                .Include(c => c.Weapon)
                .Include(c => c.Skills)
                .Where(c => request.CharactersIds.Contains(c.id))
                .ToListAsync(); 


                bool defeated = false; 

                while (!defeated){

                    foreach(var attacker in characters){
                        var opponents = characters.Where(c => c.id != attacker.id).ToList();
                        var opponent = opponents[new Random().Next(opponents.Count)]; 

                        int damage = 0; 
                        string attackUsed = string.Empty; 

                        bool useWeapon = new Random().Next(2) == 0;

                        if (useWeapon && attacker.Weapon is not null){
                            
                            attackUsed = attacker.Weapon.Name; 
                            damage = DoWeaponAttack(attacker, opponent); 

                        } else if (!useWeapon && attacker.Skills is not null){

                            var skill = attacker.Skills[new Random().Next(attacker.Skills.Count)]; 
                            attackUsed = skill.Name; 
                            damage = DoSkillAttack(attacker, opponent, skill); 

                        } else {
                            response.Data.Log
                                .Add($"{attacker.Name} was not able to attack!");
                            
                            continue; 
                        }
                    response.Data.Log   
                        .Add($"{attacker.Name} attacks {opponent.Name} using {attackUsed} with {(damage >= 0 ? damage : 0)} damage.");

                    if (opponent.hitPoints <= 0){
                        defeated = true; 
                        attacker.Victories ++; 
                        opponent.Defeats ++;
                        response.Data.Log.Add($"{opponent.Name} has been defeated!"); 
                        response.Data.Log.Add($"{attacker.Name} wins with {attacker.hitPoints} hp left."); 
                        break; 
                    } 
                    }
                }

                characters.ForEach(c => {
                    c.Fights++; 
                    c.hitPoints = 100; 

                });
                await _context.SaveChangesAsync();  
           }catch (Exception ex){
            response.Succcess = false; 
            response.Message = ex.Message; 
          
           };
           return response; 
        }

        public async Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAtackDto request)
        {
            var response = new ServiceResponse<AttackResultDto>(); 

            try
            {

                var attacker = await _context.Characters
                    .Include(c => c.Skills)
                    .FirstOrDefaultAsync(c => c.id == request.AttackerId);

                var opponent = await _context.Characters
                    .FirstOrDefaultAsync(c => c.id == request.OpponentId);

                if (attacker is null || opponent is null || attacker.Skills is null)
                {
                    throw new Exception("Someting fishy is going on here ... haha ");
                }

                var skill = attacker.Skills.FirstOrDefault(c => c.Id == request.SkillId);

                if (skill is null)
                {
                    response.Succcess = false;
                    response.Message = $"{attacker.Name} doesn't know that skill";
                    return response;
                }

                int damage = DoSkillAttack(attacker, opponent, skill);

                if (opponent.hitPoints <= 0)
                {
                    response.Message = $"{opponent.Name} has been defeated!";
                }

                response.Data = new AttackResultDto
                {
                    Attacker = attacker.Name,
                    Opponent = opponent.Name,
                    AttackerHP = attacker.hitPoints,
                    OponnetHP = opponent.hitPoints,
                    Damage = damage
                };

                await _context.SaveChangesAsync();
            }
            catch (Exception ex) {
                response.Succcess = false; 
                response.Message = ex.Message; 
            }

            return response; 
        }

        private static int DoSkillAttack(Character attacker, Character opponent, Skill skill)
        {
            int damage = skill.Damage + (new Random().Next(attacker.intelligence));
            damage -= new Random().Next(opponent.Defeats);

            if (damage > 0)
            {
                opponent.hitPoints -= damage;
            }

            return damage;
        }

        public async Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto request)
        {
            var response = new ServiceResponse<AttackResultDto>(); 

            try
            {

                var attacker = await _context.Characters
                    .Include(c => c.Weapon)
                    .FirstOrDefaultAsync(c => c.id == request.AttackerId);

                var opponent = await _context.Characters
                    .FirstOrDefaultAsync(c => c.id == request.OponentId);

                if (attacker is null || opponent is null || attacker.Weapon is null)
                {
                    throw new Exception("Someting fishy is going on here ... haha ");
                }

                int damage = DoWeaponAttack(attacker, opponent);

                if (opponent.hitPoints <= 0)
                {
                    response.Message = $"{opponent.Name} has been defeated!";
                }

                response.Data = new AttackResultDto
                {
                    Attacker = attacker.Name,
                    Opponent = opponent.Name,
                    AttackerHP = attacker.hitPoints,
                    OponnetHP = opponent.hitPoints,
                    Damage = damage
                };

                await _context.SaveChangesAsync();
            }
            catch (Exception ex) {
                response.Succcess = false; 
                response.Message = ex.Message; 
            }

            return response; 
        }

        private static int DoWeaponAttack(Character attacker, Character opponent)
        {
            if (attacker.Weapon is null){
                throw new Exception("Attacker has no weapon!");
            }
            int damage = attacker.Weapon.Damage + (new Random().Next(attacker.strength));
            damage -= new Random().Next(opponent.Defeats);

            if (damage > 0)
            {
                opponent.hitPoints -= damage;
            }

            return damage;
        }
    }
}