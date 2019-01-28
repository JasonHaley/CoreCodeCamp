﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace CoreCodeCamp.Services
{
    public class ImageStorageService : IImageStorageService
    {
        private readonly IConfiguration _config;

        public ImageStorageService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<string> StoreImage(string filename, byte[] image)
        {
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            var accessToken = await azureServiceTokenProvider.GetAccessTokenAsync("https://storage.azure.com/");
            var tokenCredential = new TokenCredential(accessToken);
            var creds = new StorageCredentials(tokenCredential);


            var url = string.Concat(_config["BlobService-StorageUrl"], filename);
            //var acct = _config["BlobService-Account"];
            //var key = _config["BlobService-Key"];

            //var creds = new StorageCredentials(acct, key);
            var blob = new CloudBlockBlob(new Uri(url), creds);

            bool shouldUpload = true;
            if (await blob.ExistsAsync())
            {
                await blob.FetchAttributesAsync();
                if (blob.Properties.Length == image.Length)
                {
                    shouldUpload = false;
                }
            }

            if (shouldUpload) await blob.UploadFromByteArrayAsync(image, 0, image.Length);


            return url;
        }
    }
}
