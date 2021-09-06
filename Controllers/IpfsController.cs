﻿using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace ErgoUtilsUploadService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IpfsController : ControllerBase
    {
        private readonly IConfiguration _config;

        public IpfsController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var client = new RestClient("https://api.nft.storage");

            using var stream = file.OpenReadStream();
            using var ms = new MemoryStream();
            stream.CopyTo(ms);

            var request = new RestRequest("upload", Method.POST);
            request.AddHeader("Authorization", $"Bearer {_config["ApiKey"]}");
            request.AddParameter(file.Name, ms.ToArray(), ParameterType.RequestBody);

            var response = await client.ExecuteAsync(request);

            return Ok(JsonSerializer.Deserialize(response.Content, typeof(object)));
        }
    }
}
