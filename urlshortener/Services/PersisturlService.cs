using System;
using System.Collections.Generic;
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
        Task<string> GetShortUrl(Url longUrl);
        Task<int> SaveUrl(PersistUrl persistUrl, CancellationToken cancellationToken);
    }
    public class PersisturlService : IPersisturlService
    {
        PersistUrl _persistUrl;
        private IConfiguration _configuration;
        private CreateUpdateUrlRecord createUpdateUrlRecord;
        private CancellationToken source;
        public PersisturlService(IConfiguration configuration)
        {
            _persistUrl = new PersistUrl();
            _configuration = configuration;
            createUpdateUrlRecord = new CreateUpdateUrlRecord(_configuration);
            source = new CancellationToken();
        }
        public async Task<string> GetShortUrl(Url url)
        {
            string longUrl = url.LongUrl;
            string hash = CreateMD5(longUrl);
            var base62Converter = new Base62Converter();
            var encoded = base62Converter.Encode(hash);
            _persistUrl.LongUrl = longUrl;
            _persistUrl.ShortUlr = encoded;
            var response = await SaveUrl(_persistUrl, source);
            if(response == -1)
            {
                longUrl = longUrl + System.DateTime.UtcNow.ToLongDateString();
                hash = CreateMD5(longUrl);
                base62Converter = new Base62Converter();
                encoded = base62Converter.Encode(hash);
                _persistUrl.LongUrl = longUrl;
                _persistUrl.ShortUlr = encoded;
                response = await SaveUrl(_persistUrl, source);
            }
            return encoded;
        }
        public async Task<int> SaveUrl(PersistUrl persistUrl, CancellationToken source)
        {
            _persistUrl.CreatedBy = "Shorten URL Service";
            _persistUrl.CreatedOn = DateTimeOffset.Now;
            _persistUrl.Identifier = System.Guid.NewGuid().ToString();
            var responseCode = await createUpdateUrlRecord.HandleRecord(_persistUrl, source);
            return responseCode;
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
