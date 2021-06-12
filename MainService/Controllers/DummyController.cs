using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MainService.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MainService
{
    [Route("api/[controller]")]
    public class DummyController : ControllerBase
    {

        [HttpGet]
        public Dummy Get()
        {
            _logger.LogInformation("Dummy API hit.");
            //var client = new HttpClient();
            //var response = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            //{
            //    Address = "https://demo.identityserver.io/connect/token",

            //    ClientId = "client",
            //    ClientSecret = "secret",
            //    Scope = "api1",

            //    UserName = "bob",
            //    Password = "bob"
            //});
            return new Dummy { Name = "Ain't no thang fo a chikn wang... and the dummy api was hit." };
            
        }

        private ILogger<DummyController> _logger;

        public DummyController(ILogger<DummyController> logger, IDatabase database)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("/api/dummy/fileUpload")]
        public async Task<IActionResult> FileUpload()
        {
            _logger.Log(LogLevel.Information, "FileUpload hit");

            // should only be one file here. 
            // should validate that this is an mp4...
            var files = HttpContext.Request.Form.Files;

            long size = files.Sum(f => f.Length);

            _logger.Log(LogLevel.Information, $"Number of files is {files.Count}.");

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var extension = Path.GetExtension(formFile.FileName);

                    if (extension != null) 
                    {
                        // this will throw an error if we don't delete the temp files we've created.
                        // specifically if we create 65535 files without deleteing. 
                        // TODO after successful write to S3 we must delete the tmp file 
                        var filePath = Path.GetTempFileName();

                        filePath = Path.ChangeExtension(filePath, extension);

                        _logger.Log(LogLevel.Information, $"Writing to temp file {filePath}.");

                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await formFile.CopyToAsync(stream);
                        }
                    }

                }
            }

            // Process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Ok();
        }
    }

    public class Dummy
    {
        public string Name { get; set; }
    }
}