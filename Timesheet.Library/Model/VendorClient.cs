using System.Runtime.Serialization;

namespace Timesheet.Library.Model
{
	[DataContract(Name = "VendorClient")]
	public class VendorClient : UpdatableModel, IModel
	{
        [DataMember(Name = "VendorId")]
        public int VendorId { get; set; }

        [DataMember(Name = "Client")]
		public User Client { get; set; }
	}
}
