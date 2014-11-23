using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedPCL.DataContracts
{
    public class EventDTO
    {
        public int ID { get; set; }
        public string EventText { get; set; }
        public string EventTitle { get; set; }
        public string EventDescription { get; set; }
        public DateTime EventStartUTC { get; set; }
        public DateTime EventEndtUTC { get; set; }
        public TimeSpan Duration { get { return EventEndtUTC - EventStartUTC; } }
        public string Subject { get; set; }
    }
}