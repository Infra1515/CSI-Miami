using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using CSI_Miami.Infrastructure.Providers.Contracts;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CSI_Miami.Infrastructure.Providers
{
    public class JsonFileExporter : IExporterProvider
    {
        private readonly IConfiguration configuration;
        private static readonly Object obj = new Object();

        public JsonFileExporter(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string ExportDataAsJson(string rawSqlCommand)
        {
            var tableData = GetData(rawSqlCommand);
            return this.DataTableToJsonWithJsonNet(tableData);
        }

        private string DataTableToJsonWithJsonNet(DataTable table)
        {
            var jsonString = JsonConvert.SerializeObject(table);
            return jsonString;
        }

        private DataTable GetData(string rawSqlCommand)
        {
            DataTable dt = new DataTable();

            lock (obj)
            {
                using (SqlConnection con = new SqlConnection(configuration
                                                            .GetConnectionString("DefaultConnection")))
                {
                    using (SqlCommand cmd = new SqlCommand(rawSqlCommand, con))
                    {
                        con.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                        Dictionary<string, object> row;
                        foreach (DataRow dr in dt.Rows)
                        {
                            row = new Dictionary<string, object>();
                            foreach (DataColumn col in dt.Columns)
                            {
                                row.Add(col.ColumnName, dr[col]);
                            }
                            rows.Add(row);
                        }
                    }
                }
            }

            return dt;
        }


        public void WriteDataAsJson(string text)
        {
            lock (obj)
            {

                using (StreamWriter writer =
                    new StreamWriter("movies.json", true, Encoding.UTF8))
                {
                    writer.Write(text);
                }
            }

        }
    }
}

