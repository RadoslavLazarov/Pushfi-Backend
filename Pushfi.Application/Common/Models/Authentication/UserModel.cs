using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pushfi.Application.Common.Models.Authentication
{
    public class UserModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public bool IsDeleted { get; set; }
    }
}
