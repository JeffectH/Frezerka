using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Frezerka.Experiment
{
    public static class ExperimentJsonSerializer
    {
        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public static string Serialize(ExperimentSessionData data)
        {
            return JsonConvert.SerializeObject(data, Settings);
        }

        public static ExperimentSessionData Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<ExperimentSessionData>(json, Settings);
        }
    }
}
