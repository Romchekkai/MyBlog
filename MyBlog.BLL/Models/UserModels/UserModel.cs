using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.BLL.Models.UserModels
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? Surname { get; set; }
        public string? Login { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public DateTime? DateOfTheBirth { get; set; }
        public string? Image { get; set; }
        public string? Status { get; set; }
        public string? About { get; set; }
        public string? PhoneNumber { get; set; }

        public string GetFullName()
        {
            return FirstName + " " + Surname;
        }


    }
    /* public class UserRoleModel
     {
         public int Id { get; set; }
         public string Name { get; set; }
     }*/
}
