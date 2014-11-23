using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedPCL.DataContracts
{
    public class GenericDTO<T>
    {
        public T Response { get; set; }
        public string Status { get; set; }
    }
}
