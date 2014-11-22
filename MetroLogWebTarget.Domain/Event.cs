using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLogWebTarget.Domain
{
    public class Event
    {
        public int Id { get; set; }

        public int BatId { get; set; }

        public int SequenceId { get; set; }


        public string Logger { get; set; }


        public string Level { get; set; }

        public string Message { get; set; }

        public string Exception { get; set; }

        public DateTime TimeStamp { get; set; }

    }
}
