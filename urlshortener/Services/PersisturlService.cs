using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Base62;
using Microsoft.Extensions.Configuration;
using urlshortener.Handlers;

namespace urlshortener.Services
{
    public interface IPersisturlService
    {
        Task<Response> GetShortUrl(Url longUrl);
        Task<string> GetLongUrl(string shortUrl);
        Task<Response> SaveUrl(PersistUrl persistUrl, CancellationToken cancellationToken);
    }
    public class PersisturlService : IPersisturlService
    {
        PersistUrl _persistUrl;
        private IConfiguration _configuration;
        private CreateUpdateUrlRecord createUpdateUrlRecord;
        private Response postResponse;
        private string baseHost;
        private CancellationToken source;
        public PersisturlService(IConfiguration configuration)
        {
            _persistUrl = new PersistUrl();
            _configuration = configuration;
            createUpdateUrlRecord = new CreateUpdateUrlRecord(_configuration);
            postResponse = new Response();
            source = new CancellationToken();
            baseHost = configuration["basehost"];
        }

        public async Task<string> GetLongUrl(string shortUrl)
        {
            shortUrl = baseHost + shortUrl;
            var response = await createUpdateUrlRecord.GetLongUrlRecord(shortUrl, source);
            return response;
        }

        public async Task<Response> GetShortUrl(Url url)
        {
            string longUrl = url.LongUrl;
            string hash = CreateMD5(longUrl);
            var base62Converter = new Base62Converter();
            var encoded = base62Converter.Encode(hash);
            _persistUrl.LongUrl = longUrl;
            _persistUrl.ShortUlr = baseHost + encoded.Substring(0,7);
            postResponse = await SaveUrl(_persistUrl, source);

            return postResponse;
        }
        public async Task<Response> SaveUrl(PersistUrl persistUrl, CancellationToken source)
        {
            _persistUrl.CreatedBy = "Shorten URL Service";
            _persistUrl.CreatedOn = DateTimeOffset.Now;
            _persistUrl.Identifier = System.Guid.NewGuid().ToString();
            postResponse = await createUpdateUrlRecord.HandleRecord(_persistUrl, source);
            return postResponse;
        }


        private string CreateMD5(string input)
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
        }
    }

}
