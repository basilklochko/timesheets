//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Timesheet.Library.Repository.Mssql
{
    using System;
    using System.Collections.Generic;
    
    public partial class vwTimesheetConsultant
    {
        public int id { get; set; }
        public int ClientConsultantId { get; set; }
        public int Status { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public string Comment { get; set; }
        public System.DateTime CreatedDTS { get; set; }
        public System.DateTime UpdatedDTS { get; set; }
        public Nullable<decimal> Worked { get; set; }
        public int VendorId { get; set; }
        public System.DateTime VendorCreatedDTS { get; set; }
        public System.DateTime VendorUpdatedDTS { get; set; }
        public string VendorEmail { get; set; }
        public string VendorPassword { get; set; }
        public int VendorType { get; set; }
        public string VendorUserName { get; set; }
        public string VendorAddress { get; set; }
        public string VendorPhone { get; set; }
        public string VendorFax { get; set; }
        public string VendorUrl { get; set; }
        public string VendorTaxCode { get; set; }
        public string VendorLogo { get; set; }
        public string VendorContact { get; set; }
        public int ClientId { get; set; }
        public System.DateTime ClientCreatedDTS { get; set; }
        public System.DateTime ClientUpdatedDTS { get; set; }
        public string ClientEmail { get; set; }
        public string ClientPassword { get; set; }
        public int ClientType { get; set; }
        public string ClientUserName { get; set; }
        public string ClientAddress { get; set; }
        public string ClientPhone { get; set; }
        public string ClientFax { get; set; }
        public string ClientUrl { get; set; }
        public string ClientTaxCode { get; set; }
        public string ClientLogo { get; set; }
        public string ClientContact { get; set; }
        public int ConsultantId { get; set; }
        public System.DateTime ConsultantCreatedDTS { get; set; }
        public System.DateTime ConsultantUpdatedDTS { get; set; }
        public string ConsultantEmail { get; set; }
        public string ConsultantPassword { get; set; }
        public int ConsultantType { get; set; }
        public string ConsultantUserName { get; set; }
        public string ConsultantAddress { get; set; }
        public string ConsultantPhone { get; set; }
        public string ConsultantFax { get; set; }
        public string ConsultantUrl { get; set; }
        public string ConsultantTaxCode { get; set; }
        public string ConsultantLogo { get; set; }
        public string ConsultantContact { get; set; }
    }
}
