using BusinessLayer.DTOs.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs.User
{
    public class ReteriveUserDto 
    {
        public int userId {  get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }
        public string FullName {  get; set; }
        public DateTime DateOfBirth { get; set; }
        public string CountryName {  get; set; }


    }
}
