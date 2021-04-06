using System;
using System.Data.SqlClient;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Extensions;

namespace VirtualTableTestingPlugins
{
    public class CreatePlugin : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.Get<IPluginExecutionContext>();
            if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
            {
                Entity entity = (Entity)context.InputParameters["Target"];
                Guid id = Guid.NewGuid();
                //change the table name below to the source table name you have created 
                string cmdString = "INSERT INTO VETicket (TicketID,Name,Severity) VALUES (@TicketID, @Name, @Severity)";
                SqlConnection connection = Connection.GetConnection();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = cmdString;
                    command.Parameters.AddWithValue("@TicketID", id);
                    command.Parameters.AddWithValue("@Name", entity["cat_name"]);
                    command.Parameters.AddWithValue("@Severity", entity["cat_severity"]);
                    connection.Open();
                    try
                    {
                        var numRecords = command.ExecuteNonQuery();
                        Console.WriteLine("inserted {0} records", numRecords);
                    }
                    finally
                    {
                        connection.Close();
                    }
                    // other codes. 
                }
                context.OutputParameters["id"] = id;
            }
        }
    }
}
