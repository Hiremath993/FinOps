using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Finops.DTO;
using Finops.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Finops.Controllers
{
    // [Route("[controller]")]
    // public class EmailController : Controller
    // {
    //     private readonly ILogger<EmailController> _logger;

    //     public EmailController(ILogger<EmailController> logger)
    //     {
    //         _logger = logger;
    //     }

    //     public IActionResult Index()
    //     {
    //         return View();
    //     }

    //     [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    //     public IActionResult Error()
    //     {
    //         return View("Error!");
    //     }
    // }

    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {

        private readonly IEmailRepository emailRepository;

        public EmailController(IEmailRepository emailRepository)
        {
            this.emailRepository = emailRepository;
        }

        [Route("api/[controller]")]
        [HttpPost]
        public IActionResult SendEmail(EmailDTO email)
        {
            try
            {
                emailRepository.SendEmail(email);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest("something went wrong while sending email" + e);
            }
        }
    }
}