using System.Net.Mime;
using System.Reflection;
using Kuva.Accounts.Service.Models;
using Microsoft.AspNetCore.Mvc;
using FileVersionInfo = System.Diagnostics.FileVersionInfo;

namespace Kuva.Accounts.Service.Controllers
{
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Route("api/v1/[Controller]")]
    public class VersionController : BaseController
    {
        [HttpGet]
        public object Get()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var fileInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            return new VersionModel
            {
                Version = fileInfo.FileVersion,
                Description = fileInfo.ProductName
            };
        }
    }
}