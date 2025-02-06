using BusinessLayer.DTOs.Applications;
using BusinessLayer.Service;
using BusinessLayer.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiBank.Controllers.Applications
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {

        private readonly IApplicationService _applicationService;

        public ApplicationController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {

            try
            {

                var lstApplications = _applicationService.GetAllAsync();
                return Ok(new { success = true, data = lstApplications });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An Error Occurred while retriving Person." });

            }
        }

        [HttpGet("GetByID{applicationId}", Name = "GetApplicationByID")]
        public async Task<IActionResult> GetUserByID(int applicationId)
        {

            try
            {

                if (applicationId < 0)
                {
                    return BadRequest($"Not Accepted ID: {applicationId}");
                }

                var application = await _applicationService.GetAsync(a => a.ApplicationId == applicationId, "AccountClass,ApplicantPerson,CreatedByUser");

                if (application != null)
                {
                    ReteriveApplicationDto reteriveApplication = new ReteriveApplicationDto()
                    {
                        Id = applicationId,
                        AccountClassName = application.AccountClass.AccountName,
                        ApplicationDate=application.ApplicantDate,
                        ApplicationStatus= application.ApplicationStatus,
                        CreatedByUserID = application.CreatedByUser.UserId,
                        FirstName=application.ApplicantPerson.FirstName,
                        LastName=application.ApplicantPerson.LastName,
                        LastAplicationDate = application.ApplicantDate,
                        PaidFees = application.PaidFees,
                    };

                    return Ok(new { success = true, data = reteriveApplication });
                }

                else
                {
                    return NotFound($"Application with ID {applicationId} Not Found.");
                }
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An Error Occurred while retriving application." });
            }

        }


        [HttpPost(Name = "AddApplication")]
        public async Task<IActionResult> Add(CreateApplicationDto dto)
        {

            if (dto == null)
            {
                return BadRequest("Invalid Application data.");
            }
            
            await _applicationService.Add(dto);
            await _applicationService.SaveChanges();
            return Ok(dto);
        }






    }
}
