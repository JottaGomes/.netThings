using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dtos.Fighter;

namespace Services.FightService
{
    public interface IfightService
    {
        
        Task<ServiceResponse<AttackResultDto>> WeaponAttack (WeaponAttackDto request); 

         Task<ServiceResponse<AttackResultDto>> SkillAttack (SkillAtackDto request); 

        Task<ServiceResponse<FightResultDto>> Fight (FightRequestDto request); 
    }
}