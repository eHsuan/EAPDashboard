using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EAPGateway.Models
{
    public class GatewayConfig
    {
        public int Port { get; set; } = 5000;
        public string PrimaryDb { get; set; } = "";
        public string SecondaryDb { get; set; } = "";
    }

    public class BindRequest
    {
        public string EqpId { get; set; } = "";
        public string CassetteId { get; set; } = "";
        public string WorkOrder { get; set; } = "";
        public List<string> Panels { get; set; } = new List<string>();
    }

    public class UnbindRequest
    {
        public string EqpId { get; set; } = "";
        public string CassetteId { get; set; } = "";
    }

    public class QueryResponse
    {
        public string WorkOrder { get; set; } = "";
        public List<string> Panels { get; set; } = new List<string>();
    }
}
