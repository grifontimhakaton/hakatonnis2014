using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedPCL.DataContracts
{
    public class SubjectDTO
    {
        public string SubjectName { get; set; }
        public int SubjectID { get; set; }
        public bool UserSelected { get; set; }
    }
}
