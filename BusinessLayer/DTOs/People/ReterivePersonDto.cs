using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs.People
{
    public class ReterivePersonDto
    {
        public int Id { get; set; }
        public string NationalNo { get; set; } = null!;
        public string FullName {  get; set; } = null!;
        public DateTime DateOfBirth { get; set; }

        public string Gendor { get; set; }
        
        public string Address { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string? Email { get; set; }

        public string? ImagePath { get; set; }

       public string CountryName {  get; set; } = null!;

    }
}
