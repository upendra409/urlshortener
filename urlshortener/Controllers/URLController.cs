using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using urlshortener.Services;

namespace urlshortener.Controllers
{
    [ApiController]
    [Route("urlservice/v1/[controller]")]
    public class urlController : ControllerBase
    {
        private readonly ILogger<urlController> _logger;
        private readonly IPersisturlService _persisturlService;
        private readonly IConfiguration _configuration;
        public urlController(ILogger<urlController> logger
            , IPersisturlService persisturlService
            , IConfiguration configuration)
        {
            _logger = logger;
            _persisturlService = persisturlService;
            _configuration = configuration;
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Url url)
        {
            var shortUrl = _persisturlService.GetShortUrl(url);
            return Ok(shortUrl);
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok();
        }
/*        private string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }*/
    }
}
