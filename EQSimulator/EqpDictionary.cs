using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace EQSimulator
{
    public class EqpDictionary
    {
        public string Model { get; set; }
        public Dictionary<string, string> Events { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> Variables { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> ManagementItems { get; set; } = new Dictionary<string, string>();

        public string GetManagementItemValue(string inputCode)
        {
            if (string.IsNullOrWhiteSpace(inputCode) || ManagementItems == null) return null;
            string trimmed = inputCode.Trim();
            foreach (var kvp in ManagementItems)
            {
                if (kvp.Key.Equals(trimmed, StringComparison.OrdinalIgnoreCase))
                    return kvp.Value;
            }
            return null;
        }

        public static EqpDictionary Load(string eqId)
        {
            try 
            {
                string dictPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Dictionaries", $"{eqId}.json");
                if (File.Exists(dictPath))
                {
                    string json = File.ReadAllText(dictPath);
                    return JsonConvert.DeserializeObject<EqpDictionary>(json);
                }
            } catch { }
            return new EqpDictionary();
        }

        public string GetCeid(string eventName)
        {
            foreach (var kvp in Events)
            {
                if (kvp.Value.Equals(eventName, StringComparison.OrdinalIgnoreCase)) return kvp.Key;
            }
            return eventName;
        }

        public string GetVid(string varName)
        {
            foreach (var kvp in Variables)
            {
                if (kvp.Value.Equals(varName, StringComparison.OrdinalIgnoreCase)) return kvp.Key;
            }
            return varName;
        }
        
        public string GetVarName(string vid)
        {
            return Variables.TryGetValue(vid, out var name) ? name : vid;
        }
    }
}
