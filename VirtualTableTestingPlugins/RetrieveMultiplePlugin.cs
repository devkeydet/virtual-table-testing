using System;
using System.Data.SqlClient;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Extensions;

namespace VirtualTableTestingPlugins
{
    public class RetrieveMultiplePlugin : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.Get<IPluginExecutionContext>();
            EntityCollection collection = new EntityCollection();
            //change the table name below to the source table name you have created  
            string cmdString = "SELECT TicketID, Severity, Name FROM VETicket";
            SqlConnection connection = Connection.GetConnection();
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = cmdString;
                connection.Open();
                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Entity e = new Entity("cat_ticket");
                            e.Attributes.Add("cat_ticketid", reader.GetGuid(0));
                            e.Attributes.Add("cat_severity", reader.GetInt32(1));
                            e.Attributes.Add("cat_name", reader.GetString(2));
                            collection.Entities.Add(e);
                        }
                    }
                }
                finally
                {
                    connection.Close();
                }
                context.OutputParameters["BusinessEntityCollection"] = collection;
            }
        }
    }
}
