using System.Collections.Generic;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AdvWorks
{
    public static class AdvWorks
    {
        [Function("AdvWorks")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("AdvWorks");
            logger.LogInformation("C# HTTP trigger function processed a request.");
          
            var sqlConnection = Environment.GetEnvironmentVariable("conString");
                
            List<Product> productList=new List<Product>();

            using (SqlConnection conn = new SqlConnection(sqlConnection))
            {

                

                SqlCommand sqlCmdSupply = new SqlCommand();
                SqlDataReader readerSupply;
                sqlCmdSupply.CommandText = "select ProductId,Name,ListPrice from saleslt.Product";
                sqlCmdSupply.Connection = conn;
                conn.Open();
                readerSupply = sqlCmdSupply.ExecuteReader();
                while(readerSupply.Read())
                {
                    productList.Add(
                        new Product() {
                            Id=(int)readerSupply["ProductId"],
                            ProductName=(string)readerSupply["Name"],
                            Price=(double)readerSupply["ListPrice"]
                        }
                    );
                }
                readerSupply.Close();
                conn.Close();
            }



            var response = new ObjectResult(productList);

            return response;
        }
    }
}
