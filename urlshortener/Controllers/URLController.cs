using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using urlshortener.Handlers;
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
        private ExceptionHandler _exceptionHandler;
        private Microsoft.Extensions.Primitives.StringValues apikey;
        public urlController(ILogger<urlController> logger
            , IPersisturlService persisturlService
            , IConfiguration configuration)
        {
            _logger = logger;
            _persisturlService = persisturlService;
            _configuration = configuration;
            _exceptionHandler = new ExceptionHandler();
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Url url)
        {
            try
            {
                
                var userIpAddress = this.Request.Headers["X-Forwarded-For"].ToString().Split(',').FirstOrDefault();
                this.Request.Headers.TryGetValue("apikey", out apikey);
                var shortUrl = await _persisturlService.GetShortUrl(url);
                return Ok(shortUrl);
            }
            catch(Exception ex)
            {
                _exceptionHandler.ErrorCode = "1000";
                _exceptionHandler.ErrorMessage = ex.Message;
                return BadRequest(_exceptionHandler);
            }
            
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(DateTime.Now.Ticks.ToString());
        }
    }
}
