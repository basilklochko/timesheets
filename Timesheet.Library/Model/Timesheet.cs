using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Timesheet.Library.Model
{
	public enum TimesheetStatus
	{
		Pending = 0,
		Submitted = 1,
		Rejected = 2,
		Approved = 3,
		Processed = 4
	}

	[DataContract(Name="Timesheet")]
	public class Timesheet : UpdatableModel, IModel
	{
        [DataMember]
        public int ClientConsultantId { get; set; }

        [DataMember]
		public TimesheetStatus Status { get; set; }
        [DataMember]
        public string TimesheetStatus { get; set; }
        [DataMember]
		public DateTime StartDate { get; set; }
        [DataMember]
		public DateTime EndDate { get; set; }

        [DataMember]
        public User Vendor { get; set; }
        [DataMember]
        public User Client { get; set; }
        [DataMember]
        public User Consultant { get; set; }

        [DataMember]
        public decimal Worked { get; set; }

        [DataMember]
        public string Comment { get; set; }

        [DataMember]
        public List<TimesheetDay> Days { get; set; }
	}
}
