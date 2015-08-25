using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLogWebTarget.Domain
{
    public class LogEvent:BaseEntity
    {

        public int BatId { get; set; }

        public int SequenceId { get; set; }

        public string Logger { get; set; }

        public LogLevel Level { get; set; }

        public string Message { get; set; }

        //public ExceptionWrapper Exception { get; set; }

        public DateTime TimeStamp { get; set; }

        public int LogEnvironmentId { get; set; }
    }

}
