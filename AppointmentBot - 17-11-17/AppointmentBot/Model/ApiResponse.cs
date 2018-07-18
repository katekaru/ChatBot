using AppointmentBot.Enum;
using System;
using System.Collections.Generic;

namespace AppointmentBot.Model
{
    [Serializable]
    public class ApiResponse
    {
        public ActionType Action { get; set; }
        public Dictionary<string, object> Parameters { get; set; }
        public string Query { get; set; }
        public float Score { get; set; }
        public string ResponseMessage { get; set; }
        public bool IsError { get; set; }
        public string SessionId { get; set; }
        public bool IsComplete { get; set; }
    }
}