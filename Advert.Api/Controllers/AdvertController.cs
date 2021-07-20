using Advert.Api.Services;
using AdvertApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Advert.Api.Controllers
{
    [Route("api/v1/adverts")]
    [ApiController]
    public class AdvertController : ControllerBase
    {

        private readonly IAdvertStorageService advertStorageService;

        public AdvertController(IAdvertStorageService advertStorageService)
        {
            this.advertStorageService = advertStorageService;
        }

        [HttpPost("create")]
        [ProducesResponseType(400)]
        [ProducesResponseType(typeof(CreateAdvertResponse),201)]
        public async Task<IActionResult> Create(AdvertModel model)
        {
            string recordId = "";
            try
            {
                recordId = await this.advertStorageService.Add(model);

            }catch (KeyNotFoundException)
            {
                return new NotFoundResult();
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return StatusCode(201,new CreateAdvertResponse { Id = recordId });
        }

        [HttpPut("confirm")]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> Confirm(ConfirmAdvertModel model)
        {
            bool success;
            try
            {
                success = await this.advertStorageService.Confirm(model);

            }
            catch (KeyNotFoundException)
            {
                return new NotFoundResult();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(success);
        }
    }
}
