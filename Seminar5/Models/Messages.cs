﻿using System;
namespace Seminar5.Models
{
	public class Messages
	{
		public Messages()
		{
		}

		public int Id{ get; set; }
		public string Text { get; set; }
		public bool Received { get; set; }
        public int? ToUserId { get; set; }
        public int? FromUserId { get; set; }
		public virtual User? ToUser { get; set; }
        public virtual User? FromUser { get; set; }
    }
}

