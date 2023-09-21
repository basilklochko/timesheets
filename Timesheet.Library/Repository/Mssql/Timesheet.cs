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
    
    public partial class Timesheet
    {
        public Timesheet()
        {
            this.TimesheetDay = new HashSet<TimesheetDay>();
        }
    
        public int id { get; set; }
        public int ClientConsultantId { get; set; }
        public int Status { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public string Comment { get; set; }
        public System.DateTime CreatedDTS { get; set; }
        public System.DateTime UpdatedDTS { get; set; }
    
        public virtual ClientConsultant ClientConsultant { get; set; }
        public virtual ICollection<TimesheetDay> TimesheetDay { get; set; }
    }
}