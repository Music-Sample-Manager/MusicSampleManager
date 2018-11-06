using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Data.SqlClient;

namespace ContributorWebsite.Backend
{
    public static class CreatePackage
    {
        [FunctionName(nameof(CreatePackage))]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            string packageName = req.Query["packageName"];
            string packageDescription = req.Query["packageDescription"];
            int authorId = int.Parse(req.Query["authorId"]);

            // * Store content in a Blob - make note of its ID
            // * Add metadata to relational data store, including the Blob ID
            // Data:
            // * Blob content
            // * Package name
            // * Package revision


            if (packageName == null)
            {
                return new BadRequestObjectResult("Please pass a packageName in the query string");
            }
            if (packageDescription == null)
            {
                return new BadRequestObjectResult("Please pass a packageDescription in the query string");
            }
            if (authorId <= 0)
            {
                return new BadRequestObjectResult("Please pass an authorId in the query string");
            }

            // Get the connection string from app settings and use it to create a connection.
            var str = Environment.GetEnvironmentVariable("DatabaseConnectionString");
            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                var text = "INSERT Packages (Identifier, Description, AuthorId) " +
                           "VALUES (@packageName, @packageDescription, @authorId)";

                using (SqlCommand cmd = new SqlCommand(text, conn))
                {
                    cmd.Parameters.AddWithValue("@packageName", packageName);
                    cmd.Parameters.AddWithValue("@packageDescription", packageDescription);
                    cmd.Parameters.AddWithValue("@authorId", authorId);

                    // Execute the command and log the # rows affected.
                    var rows = cmd.ExecuteNonQuery();
                    log.Info($"{rows} rows were updated");
                }
            }

            return (ActionResult)new OkObjectResult($"Hello, {packageName} - {packageDescription}");
        }
    }
}