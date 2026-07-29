using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Protocol.Drivers.Secs
{
    public class SecsDictionary
    {
        public string Model { get; set; } = string.Empty;
        public Dictionary<string, string> Events { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> Variables { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> StatusMapping { get; set; } = new Dictionary<string, string>();

        public static SecsDictionary Load(string filePath)
        {
            if (!File.Exists(filePath))
                return new SecsDictionary();

            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<SecsDictionary>(json) ?? new SecsDictionary();
        }

        public string GetEventName(string ceid)
        {
            if (string.IsNullOrWhiteSpace(ceid)) return ceid ?? string.Empty;
            string trimmed = ceid.Trim();
            if (Events.TryGetValue(trimmed, out var name) && !string.IsNullOrEmpty(name))
                return name;
            return trimmed;
        }

        public string GetVariableName(string vid)
        {
            if (string.IsNullOrWhiteSpace(vid)) return vid ?? string.Empty;
            string trimmed = vid.Trim();
            if (Variables.TryGetValue(trimmed, out var name) && !string.IsNullOrEmpty(name))
                return name;
            return trimmed;
        }

        public string MapStatus(string value)
        {
            return StatusMapping.TryGetValue(value, out var status) ? status : value;
        }
    }
}
