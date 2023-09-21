using System;
using System.Runtime.Serialization;

namespace Timesheet.Library.Model
{
    [DataContract(Name = "ClientConsultant")]
    public class ClientConsultant : UpdatableModel, IModel
	{
        [DataMember(Name = "Client")]
        public User Client { get; set; }

        [DataMember(Name = "Consultant")]
        public User Consultant { get; set; }
	}
}
