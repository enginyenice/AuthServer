﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MiniApp2.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetInvoices()
        {
            // Token bilgisinden veri alma işlemleri örnek
            var userName = HttpContext.User.Identity.Name;
            var userIdClaim = User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);

            //Bu controllerda yapılacak herhangi bir işlem .......
            return Ok($"userName: {userName} / userId: {userIdClaim.Value}");
        }
    }
}
