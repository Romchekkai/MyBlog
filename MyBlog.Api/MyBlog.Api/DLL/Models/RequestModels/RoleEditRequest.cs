﻿using System.ComponentModel.DataAnnotations;
using System.Data;

namespace MyBlog.Api.DLL.Models.RequestModels
{
    public class RoleEditRequest
    {
        string roleName;
        public int Id { get; private set; }
        public Guid UserId { get; set; }

        public string UserName { get; set; }
        public string RoleName
        {
            get { return roleName; }
            set
            {
                switch (value)
                {
                    case "Admin":
                        Id = 1; break;
                    case "User":
                        Id = 2; break;
                    case "Moderator":
                        Id = 3; break;
                    default: Id = 2; break;
                }
                roleName = value;
            }

        }

    }

}

