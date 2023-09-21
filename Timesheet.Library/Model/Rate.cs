using System;

namespace Timesheet.Library.Model
{
	public enum RateType
	{
		Other,
		C2C,
		W2,
		C1099
	}

	[Serializable]
	public class Rate : UpdatableModel, IModel
	{
		public int ClientConsultantId { get; set; }
		public RateType VendorRateType { get; set; }
		public double VendorRate { get; set; }
		public RateType ConsultantRateType { get; set; }
		public double ConsultantRate { get; set; }
	}
}
