using BusinessLayer.DTOs.Applications;
using BusinessLayer.Service.IService;
using DataAccessLayer.Entities;
using DataAccessLayer.Respository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BusinessLayer.Service
{
    public class ApplicationService : GenericService<TbApplication>, IApplicationService
    {

        private readonly IUnitOfWork _unitOfWork;

        public ApplicationService(IApplicationsRepository applicationsRepository,IUnitOfWork unitOfWork) : base(applicationsRepository)
        {
            _unitOfWork = unitOfWork;   
        }

        public async Task Add(CreateApplicationDto applicationDto)
        {

            var newApplication = new TbApplication()
            {
                ApplicantPersonId=applicationDto.ApplicantPersonId,
                AccountClassId=applicationDto.AccountClassId,
                ApplicantDate=applicationDto.ApplicantDate,
                LastStatusDate=applicationDto.LastStatusDate,
                PaidFees=applicationDto.PaidFees,
                CreatedByUserId=applicationDto.CreatedByUserId,
                ApplicationStatus=applicationDto.ApplicationStatus
            };

            await _unitOfWork.Applications.AddAsync(newApplication);
        }

        public void Cancel(TbApplication application)
        {
            _unitOfWork.Applications.UpdateStatus(application, 3);
        }

        public Task<IEnumerable<TbApplication>> GetAllAsync()
        {
            return _unitOfWork.Applications.GetAllAsync();
        }

        public void SetComplete(TbApplication application)
        {
            _unitOfWork.Applications.UpdateStatus(application, 3);
        }

  
    }
}
