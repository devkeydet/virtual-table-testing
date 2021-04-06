using System;
using System.Data.SqlClient;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Extensions;

namespace VirtualTableTestingPlugins
{
    public class RetrievePlugin : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.Get<IPluginExecutionContext>();
            Guid id = Guid.Empty;
            if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is EntityReference)
            {
                EntityReference entityRef = (EntityReference)context.InputParameters["Target"];
                Entity e = new Entity("cat_ticket");
                //change the table name below to the source table name you have created  
                string cmdString = "SELECT TicketID, Severity, Name FROM VETicket WHERE TicketID=@TicketID";
                SqlConnection connection = Connection.GetConnection();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = cmdString;
                    command.Parameters.AddWithValue("@TicketID", entityRef.Id);
                    connection.Open();
                    try
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                e.Attributes.Add("cat_ticketid", reader.GetGuid(0));
                                e.Attributes.Add("cat_severity", reader.GetInt32(1));
                                e.Attributes.Add("cat_name", reader.GetString(2));
                            }
                        }
                    }
                    finally
                    {
                        connection.Close();
                    }
                    // other codes. 
                }
                context.OutputParameters["BusinessEntity"] = e;
            }
        }
    }
}
