using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dtos.Fighter;
using Microsoft.AspNetCore.Mvc;

namespace Controllers
{
   
    [ApiController]
    [Route("[controller]")]
    public class FightController : ControllerBase 
    {
        
        private readonly IfightService _fightService; 
        public FightController(IfightService fightService){
            _fightService = fightService;    
        }

        [HttpPost("weapon")]
        public async Task<ActionResult<ServiceResponse<AttackResultDto>>> WeaponAttack (WeaponAttackDto request){

            return Ok(await _fightService.WeaponAttack(request)); 
        }

         [HttpPost("skill")]
        public async Task<ActionResult<ServiceResponse<AttackResultDto>>> SkillAttack (SkillAtackDto request){

            return Ok(await _fightService.SkillAttack(request)); 
        }

           [HttpPost]
        public async Task<ActionResult<ServiceResponse<FightResultDto>>> Fight (FightRequestDto request){

            return Ok(await _fightService.Fight(request)); 
        } 
    }
}