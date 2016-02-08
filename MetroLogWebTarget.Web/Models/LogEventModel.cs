using System;
using System.Collections.Generic;
using System.Linq;
using MetroLogWebTarget.Domain;

namespace MetroLogWebTarget.Web.Models
{
    public class LogEventModel
    {
        public int Id { get; set; }

        public int BatId { get; set; }

        public int SequenceId { get; set; }

        public string Logger { get; set; }

        public LogLevel Level { get; set; }

        public string Message { get; set; }

        //public ExceptionWrapper Exception { get; set; }

        public DateTimeOffset TimeStamp { get; set; }

    }
}