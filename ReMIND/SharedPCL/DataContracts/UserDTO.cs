using SharedPCL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedPCL.DataContracts
{
    public class UserDTO
    {
        public string FullName { get; set; }
        public int ID { get; set; }
        public UserStatus Status { get; set; }
    }
}
