using System.Runtime.Serialization;

namespace Timesheet.Library.Model
{
	[DataContract(Name = "VendorConsultant")]
    public class VendorConsultant : UpdatableModel, IModel
	{
        [DataMember(Name = "VendorId")]
        public int VendorId { get; set; }

        [DataMember(Name = "Consultant")]
        public User Consultant { get; set; }
	}
}
