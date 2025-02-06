using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs.Applications
{
    public class ReteriveApplicationDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AccountClassName { get; set; }
        public DateTime ApplicationDate { get; set; }
        public DateTime LastAplicationDate { get; set; }
        public double PaidFees { get; set; }
        public int CreatedByUserID { get; set; }
        public byte? ApplicationStatus { get; set; }

    }
}
