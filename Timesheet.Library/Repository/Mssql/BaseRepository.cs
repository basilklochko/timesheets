using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timesheet.Library.Repository.Mssql
{
    public class BaseRepository
    {
        protected static timesheetsEntities entities;

        public BaseRepository()
        {
            if (entities == null)
            {
                entities = new timesheetsEntities();                
            }
        }
    }
}
