using System.Collections.Generic;
using MetroLogWebTarget.Domain;

namespace MetroLogWebTarget.Web.Models
{
    public class JsonPostWrapper
    {

        public LogEnvironment Environment { get; set; }

        public List<LogEvent> Events { get; set; }
    }
}