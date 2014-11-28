using System.Collections.Generic;

namespace MetroLogWebTarget.Web.Models
{
    public class LoggingEnvironment
    {
        public Dictionary<string, object> Values { get; private set; }

        public LoggingEnvironment()
        {
            this.Values = new Dictionary<string, object>();
        }
    }
}