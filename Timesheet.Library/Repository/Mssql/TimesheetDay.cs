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
    
    public partial class TimesheetDay
    {
        public int id { get; set; }
        public int TimesheetId { get; set; }
        public System.DateTime Day { get; set; }
        public decimal Worked { get; set; }
        public System.DateTime CreatedDTS { get; set; }
        public System.DateTime UpdatedDTS { get; set; }
    
        public virtual Timesheet Timesheet { get; set; }
    }
}
