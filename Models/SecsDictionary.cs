using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace EAPDashboard.Models
{
    public class SecsDictionary
    {
        public string Model { get; set; }
        public Dictionary<string, string> Events { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> Variables { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> StatusMapping { get; set; } = new Dictionary<string, string>();

        public static SecsDictionary Load(string filePath)
        {
            if (!File.Exists(filePath))
                return new SecsDictionary();

            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<SecsDictionary>(json);
        }

        public string GetEventName(string ceid)
        {
            return Events.TryGetValue(ceid, out var name) ? name : $"Unknown_CEID_{ceid}";
        }

        public string GetVariableName(string vid)
        {
            return Variables.TryGetValue(vid, out var name) ? name : $"Unknown_VID_{vid}";
        }

        public string MapStatus(string value)
        {
            return StatusMapping.TryGetValue(value, out var status) ? status : value;
        }
    }
}
