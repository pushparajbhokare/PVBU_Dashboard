using App.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnetWebService.Core.DB
{
    public static class DBConnection
    {
        public static string GetConnectionString()
        {
            runTimeConfiguration configManager = new runTimeConfiguration();

            return string.Concat("Server=", configManager.getParticularConfig("qdas_value_db", "dataSource"),
             ";Database=", configManager.getParticularConfig("qdas_value_db", "dbName"),
             ";User ID=", configManager.getParticularConfig("qdas_value_db", "userID"),
             ";Password=", configManager.getParticularConfig("qdas_value_db", "password"));
        }
    }
}
