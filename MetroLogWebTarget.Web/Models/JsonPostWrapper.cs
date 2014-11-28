using Newtonsoft.Json;
using WebTargetSample.Json;
using WebTargetSample.Model;

namespace MetroLogWebTarget.Web.Models
{
    public class JsonPostWrapper
    {
        [JsonConverter(typeof(LoggingEnvironmentConverter))]
        public LoggingEnvironment Environment { get; set; }

        public LogEventInfo[] Events { get; set; }

        public JsonPostWrapper()
        {
        }

        public static JsonPostWrapper FromJson(string json)
        {
            return JsonConvert.DeserializeObject<JsonPostWrapper>(json);
        }
    }
}