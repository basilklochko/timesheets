using System;
using System.Runtime.Serialization;

namespace Timesheet.Library.Model
{
    [DataContract]
	public abstract class UpdatableModel
	{
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public DateTime CreatedDTS { get; set; }
        [DataMember]
        public DateTime UpdatedDTS { get; set; }
	}
}
