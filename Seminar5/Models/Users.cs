using System;
namespace Seminar5.Models
{
	public class User
	{
		public User()
		{
		}

        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Messages> ToMassages { get; set; }
        public virtual ICollection<Messages> FromMassages { get; set; }

    }
}

