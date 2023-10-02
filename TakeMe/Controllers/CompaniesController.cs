using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TakeMe.Core.DTOs.GenericRepositriesDTO;
using TakeMe.Core.Entities;
using TakeMe.Core.Interfaces;
using TakeMe.Error;

namespace TakeMe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly IUnitOfWork work;

        public CompaniesController(IUnitOfWork work)
        {
            this.work = work;
        }
        [HttpGet("get-all-company")]
        public async Task<ActionResult> getAllCompany()
        {
            var getCompany=await work.Companies.GetAllAsync();
            if (getCompany is null)
            {
                return BadRequest(new BaseComonentResponse(400, "Bad Request"));
            }

            return Ok(getCompany);
        }
        [HttpPost("add-company")]
        public async Task<ActionResult> AddCompany(AddCompanyDTO companyDTO)
        {
            if (ModelState.IsValid)
            {
                if (companyDTO is null) { return BadRequest(new BaseComonentResponse(400, "bad request")); }
            var test=await work.Companies.CheckIshere(companyDTO.name);
            if (test)
            {
                return BadRequest(new BaseComonentResponse(400, "this already Exist..!"));
            }
            Company company = new Company
            {
                Name=companyDTO.name,
                description=companyDTO.description,
            };
                await work.Companies.AddAsync(company);
                return Ok(new BaseComonentResponse(200));
            }
            return BadRequest(new BaseComonentResponse(400, "bad request"));
        }
        
        [HttpPut("update-company-by-id/{id}")]
        public async Task<ActionResult>Update([Required]int id, UpdateCompanyDTO companyDTO)
        {
            if (ModelState.IsValid)
            {
                if (id==0  || companyDTO is null)
                {
                    return BadRequest(new BaseComonentResponse(400));
                }
                Company getId = await work.Companies.GetAsync(id);
                if (getId == null) { return BadRequest(new BaseComonentResponse(404,$"this id= {id} not found")); }
                getId.description = companyDTO.description;
                getId.Name = companyDTO.name;
                await work.Companies.UpdateAsync(id, getId);
                return Ok(new BaseComonentResponse(200));
            }
            return BadRequest(error: new BaseComonentResponse(statusCode: 400, message: "bad request"));
        }
        [HttpDelete("delete-company-by-id/")]
        public async Task<ActionResult> Delete(int id)
        {
            if (ModelState.IsValid)
            {
                if (id!=0)
                {
                    await work.Companies.DeleteAsync(id);
                    return Ok(new BaseComonentResponse(200,"Delete Done !"));
                }

            }
            return BadRequest(new BaseComonentResponse(400,"something went wrong"));
        }
    }
}
