using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BusinessLayer.Service;
using DataAccessLayer.Respository.IRepository;
using BusinessLayer.Service.IService;
using DataAccessLayer.Entities;
using BusinessLayer.DTOs.People;
using Microsoft.AspNetCore.Authorization;

namespace ApiBank.Controllers.People
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {

        private readonly IPersonService _personService;
        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {

                var lstPeople = await _personService.GetAllAsync();
                return Ok(new { success = true, data = lstPeople });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An Error Occurred while retriving Person." });
            }
        }


        [HttpGet("GetByID/{personID}", Name = "GetPersonByID")]
        public async Task<IActionResult> GetPersonByID(int personID)
        {

            try
            {

                if (personID < 0)
                {
                    return BadRequest($"Not Accepted ID: {personID}");
                }

                var Student = await _personService.GetAsync(p => p.PersonId == personID, "NationalityCountry");

                if (Student != null)
                {

                    ReterivePersonDto reterivePersondto = new ReterivePersonDto()
                    {
                        Id = Student.PersonId,
                        NationalNo = Student.NationalNo,
                        FullName = Student.FirstName + ' ' + Student.SecondName + ' ' + Student.ThirdName + ' ' + Student.LastName,
                        Address = Student.Address,
                        DateOfBirth = Student.DateOfBirth,
                        Email = Student.Email,
                        Gendor = (Student.Gendor == 0) ? "Male" : "Female",
                        ImagePath = Student.ImagePath,
                        Phone = Student.Phone,
                        CountryName = Student.NationalityCountry.CountryName

                    };
                    return Ok(new { success = true, data = reterivePersondto });
                }

                else
                {
                    return NotFound($"Student with ID {personID} Not Found.");
                }
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An Error Occurred while retriving Person." });
            }

        }


        [HttpGet("IsExist{personID:int}", Name = "IsPersonExist")]
        public async Task<IActionResult> IsPersonExist(int personID)
        {

            if (personID < 0)
            {
                return BadRequest($"Not Accepted ID: {personID}");
            }

            if (_personService.IsPersonExist(personID))
            {
                return Ok(new { success = true, data = "Person is exist" });
            }

            return NotFound($"Person with ID :{personID} is not found.");

        }


        [HttpPost(Name = "AddPerson")]
        public async Task<IActionResult> AddPerson([FromForm] CreatePersonDto dto)
        {

            if (dto == null)
            {
                return BadRequest("Invalid Person data.");
            }

            await _personService.AddPerson(dto);
            await _personService.SaveChanges();
            return Ok(dto);
        }


        [HttpPut(Name = "UpdatePerson")]
        public async Task<IActionResult> UpdatePerson(int personID, UpdatePersonDto dto)
        {

            if (personID < 0)
                return BadRequest($"Not Accepted ID: {personID}");

            if (!_personService.IsPersonExist(personID))
                NotFound("Person is not Found exist..");

            await _personService.Update(personID, dto);
            await _personService.SaveChanges();

            return Ok(dto);
        }


        [HttpDelete("Delete{personID}", Name = "DeletePerson")]
        public async Task<IActionResult> DeletePerson(int personID)
        {

            if (personID < 0)
                return BadRequest($"Not Accepted ID {personID}");

            var person = await _personService.GetAsync(p => p.PersonId == personID);

            if (person == null)
                return NotFound($"Student with ID {personID} not found..");


            _personService.Remove(person);
            await _personService.SaveChanges();

            return Ok($"Student with ID {personID} has been deleted.");
        }


    }
}

