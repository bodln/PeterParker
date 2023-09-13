﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PeterParker.Data.DTOs;
using PeterParker.Data.Models;
using PeterParker.Infrastructure;

namespace PeterParker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZoneController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public ZoneController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(ZoneDTO request)
        {
            ZoneDataDTO response = await unitOfWork.ZoneRepository.Add(request);
            return Ok(response);
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAll()
        {
            List<Zone> zones = await unitOfWork.ZoneRepository.GetAll(); 
            return Ok(zones);
        }
    }
}
