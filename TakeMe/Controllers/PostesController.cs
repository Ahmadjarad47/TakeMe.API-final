using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TakeMe.Core.DTOs;
using TakeMe.Core.Entities;
using TakeMe.Core.Interfaces;
using TakeMe.Error;

namespace TakeMe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostesController : ControllerBase
    {
        private readonly IUnitOfWork work;

        public PostesController(IUnitOfWork work)
        {
            this.work = work;

        }
        [HttpGet("get-all-postes")]

        public async Task<ActionResult> get(postesDTO postesDTO)
        {
            var check = await work.Companies.getId(postesDTO.name);
            if (check == null) { return BadRequest(error:new BaseComonentResponse(400,$"this id= {postesDTO.name} was not found")); }
            
            return Ok(check);
        }
        [HttpPost("add-posts")]
        public async Task<ActionResult> Add(AddPostDTO dTO)
        {
            if (dTO is  null)
            {
                return BadRequest(new BaseComonentResponse(400, "this object is null"));
            }
            Postes postes = new Postes
            {Timetogo=dTO.TimeRegister,
            companyId=dTO.companyId,
            phoneNumber=dTO.phoneNumber,
            Returntime = dTO.TimeRerturn,
            street = dTO.street,
            
            };
             await work.Postes.AddAsync(postes);
            return Ok(new BaseComonentResponse(200, "Done !"));
        }
        [HttpDelete("Delete-item-by-id/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            if (id==0)
            {
                return BadRequest(new BaseComonentResponse(400, "this object is null"));
            }
            await work.Postes.DeleteAsync(id);
            return Ok(new BaseComonentResponse(200, "Item is Deleted"));
        }
    }
    public record postesDTO(int name);
   
}
