﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PeterParker.Data.DTOs;
using PeterParker.Infrastructure;

namespace PeterParker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParkingSpaceController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public ParkingSpaceController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllByGeoJSON")]
        public IActionResult GetAllByGeoJSON(string request)
        {
            var result = unitOfWork.ParkingSpaceRepository.GetAllByGeoJSON(request);
            if (result != null)
            {
                return Ok(result);
            }

            return BadRequest("Something went wrong");
        }
    }
}