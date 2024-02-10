using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;

namespace AdvWorks
{
    public class AdvWorks
    {
        [FunctionName("AdvWorks")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req,
            ILogger log)
        {            
            log.LogInformation("C# HTTP trigger function processed a request.");
          
            var sqlConnection = Environment.GetEnvironmentVariable("conString");
                
            List<Product> productList=new List<Product>();

            using (SqlConnection conn = new SqlConnection(sqlConnection))
            {                
                SqlCommand sqlCmdSupply = new SqlCommand();
                SqlDataReader readerSupply;
                sqlCmdSupply.CommandText = "select ProductId,Name,ListPrice from saleslt.Product";
                sqlCmdSupply.Connection = conn;
                conn.Open();
                readerSupply = await sqlCmdSupply.ExecuteReaderAsync();
                while(readerSupply.Read())
                {
                    productList.Add(
                        new Product() {
                            Id=(int)readerSupply["ProductId"],
                            ProductName=(string)readerSupply["Name"],
                            Price=(decimal)readerSupply["ListPrice"]
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
