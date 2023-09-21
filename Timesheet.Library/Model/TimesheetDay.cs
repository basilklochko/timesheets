using System;
using System.Runtime.Serialization;

namespace Timesheet.Library.Model
{
    [DataContract]
	public class TimesheetDay
	{
        [DataMember]
		public int RateId { get; set; }
        [DataMember]
        public DateTime Day { get; set; }
        [DataMember]
        public decimal Worked { get; set; }
	}
}
