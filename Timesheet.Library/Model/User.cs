using System.Runtime.Serialization;

namespace Timesheet.Library.Model
{
	public enum UserType
	{
		NotSet = 0,
		SuperAdmin = 1,
		Vendor = 2,
		Client = 3,
		Consultant = 4
	}

	[DataContract(Name="User")]
	[KnownType(typeof(UpdatableModel))]
	public class User : UpdatableModel, IModel
	{
		[DataMember]
		public string Email { get; set; }
		[DataMember]
		public string Password { get; set; }
		[DataMember]
		public UserType Type { get; set; }        
		[DataMember]
		public string UserName { get; set; }
		[DataMember]
		public string Address { get; set; }
		[DataMember]
		public string Phone { get; set; }
		[DataMember]
		public string Fax { get; set; }
		[DataMember]
		public string Url { get; set; }
		[DataMember]
		public string TaxCode { get; set; }
		[DataMember]
		public string Logo { get; set; }
		[DataMember]
		public string Contact { get; set; }		
	}
}
