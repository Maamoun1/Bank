using DataAccessLayer.Entities;
using DataAccessLayer.Respository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Respository.IRepository;
using BusinessLayer.Service.IService;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Respository;
using Microsoft.EntityFrameworkCore.Diagnostics;
using BusinessLayer.DTOs.People;
using Microsoft.AspNetCore.Http;


namespace BusinessLayer.Service
{
    public class PersonService : GenericService<TbPerson>,IPersonService
    {

        private readonly IUnitOfWork _unitOfWork;
        public PersonService(IPersonRepostitory personrepository,IUnitOfWork unitOfWork) : base(personrepository) 
        {
            _unitOfWork = unitOfWork;
        }

        public bool IsPersonExist(int PersonID)
        {
             
            return (_unitOfWork.Person.IsPersonExist(PersonID));
        }
        
        public async Task Update(int personID, UpdatePersonDto dto)
        {

            var personFromDb = await _unitOfWork.Person.GetAsync(p => p.PersonId == personID);

            if (personFromDb != null)
            {
                personFromDb.FirstName = dto.FirstName;
                personFromDb.SecondName = dto.SecondName;
                personFromDb.ThirdName = dto.ThirdName;
                personFromDb.LastName = dto.LastName;
                personFromDb.DateOfBirth = dto.DateOfBirth;
                personFromDb.Email = dto.Email;
                personFromDb.NationalityCountryId = dto.NationalityCountryId;
                personFromDb.Gendor = dto.Gendor;
                personFromDb.NationalNo = dto.NationalNo;
                personFromDb.Phone = dto.Phone;
                personFromDb.ImagePath = dto.ImagePath;
                personFromDb.Address = dto.Address;
                    
                _unitOfWork.Person.Update(personFromDb); 
            } 
        }

        public  async Task AddPerson(CreatePersonDto dto)
        {

            var newperson = new TbPerson
            {
                FirstName = dto.FirstName,
                SecondName = dto.SecondName,               
                ThirdName=dto.ThirdName,
                LastName=dto.LastName,
                DateOfBirth=dto.DateOfBirth,
                Email=dto.Email,
                NationalityCountryId=dto.NationalityCountryId,
                Gendor=dto.Gendor,
                NationalNo=dto.NationalNo,
                Phone=dto.Phone,
                ImagePath= await ReturnPictureName(dto.ImageFile),
                Address=dto.Address,
                
            };
            await _unitOfWork.Person.AddAsync(newperson);
        }

        public async Task<string> ReturnPictureName(IFormFile imageFile)
        {
            // Check if no file is uploaded
            if (imageFile == null || imageFile.Length == 0)
                return ("No file uploaded.");

            // Directory where files will be uploaded
            var uploadDirectory = @"C:\MyUploads";

            // Generate a unique filename
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(uploadDirectory, fileName);

            // Ensure the uploads directory exists, create if it doesn't
            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }

            // Save the file to the server
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return fileName;
        }
    }




}
