using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace urlshortener.Handlers
{
    public class CreateUpdateUrlRecord
    {
        private string _connectionString = string.Empty;
        private readonly IConfiguration _configuration;
        public CreateUpdateUrlRecord(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = Microsoft.Extensions.Configuration.ConfigurationExtensions.GetConnectionString(_configuration, "urlservicedb");
        }
        public DataTable GetUrlTable()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("LongUrl", typeof(string));
            dataTable.Columns.Add("ShortUrl", typeof(string));
            dataTable.Columns.Add("Identifier", typeof(string));
            dataTable.Columns.Add("CreatedOn", typeof(DateTimeOffset));
            dataTable.Columns.Add("CreatedBy", typeof(string));
            return dataTable;
        }
        public async Task<int> HandleRecord(PersistUrl request, CancellationToken cancellationToken)
        {
            try
            {
                DataTable dataTable = GetUrlTable();
                int responseCode = Int32.MaxValue;
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand("sp_InsertUrl", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 60;
                        command.Parameters.Add(new SqlParameter
                        {
                            ParameterName = "@LongUrl",
                            Direction = ParameterDirection.Input,
                            SqlDbType = SqlDbType.NVarChar,
                            Value = request.LongUrl
                        });
                        command.Parameters.Add(new SqlParameter
                        {
                            ParameterName = "@ShortUrl",
                            Direction = ParameterDirection.Input,
                            SqlDbType = SqlDbType.NVarChar,
                            Value = request.ShortUlr
                        });
                        command.Parameters.Add(new SqlParameter
                        {
                            ParameterName = "@Identifier",
                            Direction = ParameterDirection.Input,
                            SqlDbType = SqlDbType.NVarChar,
                            Value = request.Identifier
                        });
                        command.Parameters.Add(new SqlParameter
                        {
                            ParameterName = "@CreatedOn",
                            Direction = ParameterDirection.Input,
                            SqlDbType = SqlDbType.DateTime,
                            Value = System.DateTime.Now
                        });
                        command.Parameters.Add(new SqlParameter
                        {
                            ParameterName = "@CreatedBy",
                            Direction = ParameterDirection.Input,
                            SqlDbType = SqlDbType.NVarChar,
                            Value = request.CreatedBy
                        });
                        //command.Parameters.Add(new SqlParameter
                        //{
                        //    ParameterName = "@ReturnCode",
                        //    Direction = ParameterDirection.ReturnValue,
                        //    SqlDbType = SqlDbType.Int,
                        //    Value = -1000
                        //});
                        var returnParameter = command.Parameters.Add("@ReturnCode", SqlDbType.Int);
                        returnParameter.Direction = ParameterDirection.ReturnValue;
                        var reader = command.ExecuteReader();
                        responseCode = Int32.Parse(returnParameter.Value.ToString());
                    }
                }
                return responseCode;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
