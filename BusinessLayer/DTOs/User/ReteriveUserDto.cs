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
        public string FirstName {  get; set; }
        public string LastName {  get; set; }
        public DateTime DateOfBirth { get; set; }
        public string countryname {  get; set; }


    }
}
