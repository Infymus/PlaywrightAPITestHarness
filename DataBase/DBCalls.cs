using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VyneSDETTakeHome.DataBase
{

   /// <summary>
   /// Stub holder for accessing a database. This could be Oracle, MSSQL, SQLITE, etc.
   /// </summary>
   public class dataBaseQuery
   {

      public DataTable dbGetSomeTable(string inParams)
      {
         DataTable d = new DataTable();
         return d;
      }

   }
}
