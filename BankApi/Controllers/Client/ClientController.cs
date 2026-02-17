using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiBank.Controllers.Client
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {

        private readonly IReadOnlyService<TbClient> _readOnlyService;

        public ClientController(IReadOnlyService<TbClient> readOnlyService)
        {
            _readOnlyService = readOnlyService;
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var lstUser = await _readOnlyService.GetAllAsync();

                return Ok(new { success = true, data = lstUser });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An Error Occurred while retriving client." });
            }
        }



    }
}
