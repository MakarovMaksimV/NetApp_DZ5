using System;
using System.Text.Json;
using Seminar5.Models;

namespace Seminar5
{
    public enum Command
    {
        Register,
        Message,
        Confirmation
    }

    public class MessagesUDP
	{
        public Command Command { get; set; }
        public int? Id { get; set; }
        public string FromName { get; set; }
        public string ToName { get; set; }
        public string Text { get; set; }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }

        public static MessagesUDP FromJson(string json)
        {
            return JsonSerializer.Deserialize<MessagesUDP>(json);
        }




    }
}

