using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs.Applications
{
    public class CreateApplicationDto
    {

        public int ApplicantPersonId { get; set; }

        public int AccountClassId { get; set; }

        public DateTime ApplicantDate { get; set; }

        public DateTime LastStatusDate { get; set; }

        public int PaidFees { get; set; }

        public int? CreatedByUserId { get; set; }

        public byte? ApplicationStatus { get; set; }

    }
}
