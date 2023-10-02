using Castle.Core.Resource;
using IronBarCode;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.CodeDom.Compiler;
using System.Text;
using TakeMe.Core.DTOs;
using TakeMe.Core.Entities;
using TakeMe.Core.Interfaces;
using TakeMe.Error;
using TakeMe.InferStructuer.Data;

namespace TakeMe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterDailesController : ControllerBase
    {
        private readonly IUnitOfWork work;
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public RegisterDailesController(IUnitOfWork work, ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            this.work = work;
            this.context = context;
            this.webHostEnvironment = webHostEnvironment;
        }
       /* [Authorize]*/
        [HttpGet("Get-all-Users")]

        public async Task<IActionResult> Get()
        {
            var check = await work.RegisterDaily.GetAllAsync();
            if (check is null) { return BadRequest(new BaseComonentResponse(400)); }
            return Ok(check);
        }
        [HttpPost("create-register")]
        public async Task<ActionResult> create(RegisterDailiesDTO dTO)
        {
            if (dTO is null)
            {
                return BadRequest(new BaseComonentResponse(400));
            }
            AppUsers result = await context.AppUsers.FirstOrDefaultAsync(d => d.Email == dTO.EmailAddress);

            if (result == null) { return BadRequest(new BaseComonentResponse(400, "Please Login First")); }
            var Exist = await work.RegisterDaily.IsAreadyExist(result.Id);
            if (Exist) { return BadRequest(new BaseComonentResponse(400, $"yor are Already reistred!!{result.UserName}")); }
            if (string.IsNullOrEmpty(dTO.price.ToString())) { }

            RegisterDaily register = new RegisterDaily
            {
                TimeOfRegister = dTO.TimeGo,
                NameOfstreet = dTO.NameOfstreet,
                // company name
                BusName = dTO.BusName,
                NameOfCollage = dTO.NameOfCollage,
                price = 0,
                AppUserId = result.Id

            }; await work.RegisterDaily.AddAsync(register);

            var qr = await QR(register);
            return Ok(new
            {
                Qr = qr
            });
        }
       
        private async Task<string> QR(RegisterDaily daily)
        {
            Guid guid = Guid.NewGuid();
            var check = await work.Companies.getName(daily.BusName);
            if (check is null)
            {
                return string.Empty;
            }
            string root = (guid + "@##@" + DateTime.Now.ToFileTime().ToString().Substring(9, 9) + " Not paid").ToString();
            QRCodeCheck qRCodeCheck = new QRCodeCheck
            {
                AppUserId = daily.AppUserId,
                CompanyId = check.Id,
                Name = daily.BusName,
                MyQRCode = root,
                CheckedGo = false,
                CheckedReturn = false,
            };
            await context.QRCodeChecks.AddAsync(qRCodeCheck);
            await context.SaveChangesAsync();
            return root;
        }
        [HttpPost("Export-RegisterDaylies")]

        public FileResult Export_RegisterDaylies()
        {
            var s = context.RegisterDailies.AsNoTracking().Count();
            List<object> customers = (from customer in this.context.RegisterDailies.Take(s)
                                      orderby customer.BusName
                                      select new[] {
                                        customer.BusName,
                                        customer.AppUserId,
                                        customer.NameOfCollage,
                                        customer.NameOfstreet,
                                         customer.price.ToString(),
                                        customer.TimeOfRegister.ToString()
                                 }).ToList<object>();

            //Insert the Column Names.
            customers.Insert(0, new string[6] { "BusName", "AppUserId", "NameOfCollage", "NameOfstreet", "price", "TimeOfRegister" });

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < customers.Count; i++)
            {
                string[] customer = (string[])customers[i];
                for (int j = 0; j < customer.Length; j++)
                {
                    //Append data with separator.
                    sb.Append(customer[j] + ',');
                }

                //Append new line character.
                sb.Append("\r\n");


            }
          //  DeleteRegisterDaylies();
            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "ss.csv");
        }

        /*[HttpGet]
        public void DeleteRegisterDaylies()
        {

            IEnumerable<RegisterDaily> forms = from item in  context.RegisterDailies.ToList() select item;
            foreach (var item in forms)
            {
                 work.RegisterDaily.DeleteAsync(item.Id);
            }
           
        }*/





        [HttpPost("Export-qr")]

        public FileResult Export_Qr()
        {
            var s = context.QRCodeChecks.AsNoTracking().Count();
            List<object> customers = (from customer in this.context.QRCodeChecks.Take(s)
                                      orderby customer.Id
                                      select new[] {
                                        customer.Name,
                                        customer.AppUserId,
                                        customer.CheckedReturn.ToString(),
                                        customer.CheckedGo.ToString(),
                                        customer.MyQRCode.ToString()}).ToList<object>();



            //Insert the Column Names.
            customers.Insert(0, new string[5] { "Name", "AppUserId", "CheckedReturn", "CheckedGo", "MyQRCode"});

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < customers.Count; i++)
            {
                string[] customer = (string[])customers[i];
                for (int j = 0; j < customer.Length; j++)
                {
                    //Append data with separator.
                    sb.Append(customer[j] + ',');
                }

                //Append new line character.
                sb.Append("\r\n");


            }
            DeleteRegisterQr();
            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "ss.csv");
        }




        
        private async void DeleteRegisterQr()
        {
            IEnumerable<QRCodeCheck> forms = from item in context.QRCodeChecks.ToList() select item;
            foreach (var item in forms)
            {
               await work.IQrCodeReader.DeleteAsync(item.Id);
            }
        }












        /* [Authorize]*/
        [HttpPost("read-qr-code-from-front")]

        public async Task<ActionResult> check(ReadQrCodeFromFront read)
        {
            AppUsers auth = await context.AppUsers
                .AsNoTracking().FirstOrDefaultAsync(r => r.Email == read.email);
            QRCodeCheck result = await context.QRCodeChecks.AsNoTracking()
                .FirstOrDefaultAsync(r => r.MyQRCode.Equals(read.qrcoder));
            if (result is not null)
            {
                if (result.CheckedGo is true && result.CheckedReturn is true)
                {
                    return BadRequest(new BaseComonentResponse(400, "this account is already checked"));
                }
                if (result.CheckedGo is false && result.CheckedReturn is false)
                {
                    result.CheckedGo = true;
                    context.QRCodeChecks.Update(result);
                    await context.SaveChangesAsync();
                    return Ok(new BaseComonentResponse(200, "Time go checked"));
                }
                if (result.CheckedGo is true && result.CheckedReturn is false)
                {
                    result.CheckedReturn = true;
                    context.QRCodeChecks.Update(result);
                    await context.SaveChangesAsync();
                    return Ok(new BaseComonentResponse(200, "Return checked"));
                }
            }
            return BadRequest(new BaseComonentResponse(401));
        }

    }

}
