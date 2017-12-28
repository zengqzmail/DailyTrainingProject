using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using System.Data.SqlServerCe;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Data.EntityClient;

namespace FunctionalTasksDataManagement
{
    public class DataDemo
    {
        tasksDataContainer context;
        //
        public DataDemo()
        {
            Console.WriteLine("mic check 1 2 1 2"); //more later....
            context = new tasksDataContainer();
            var taskDataList = context.TaskDatas;
            int howManyRows = taskDataList.Count();
            Console.WriteLine("# rows: " + howManyRows.ToString());
        }
    }
}
