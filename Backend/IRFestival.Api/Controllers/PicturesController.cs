using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs;
using IRFestival.Api.Common;
using IRFestival.Api.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Web;

namespace IRFestival.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PicturesController : ControllerBase
    {
        private BlobUtility BlobUtility { get; }
        private readonly IConfiguration _configuration;

        public PicturesController(BlobUtility blobUtility, IConfiguration configuration)
        {
            BlobUtility = blobUtility;
            _configuration = configuration;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type= typeof(string[]))]
        public async Task<ActionResult> GetAllPictureUrls()
        {
            var container = BlobUtility.GetThumbsContainer();
            var result = container.GetBlobs()
                .Select(blob => BlobUtility.GetSasUri(container, blob.Name))
                .ToArray();
            return Ok(result);

        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK, Type= typeof(AppSettingsOptions))]
        public async Task<ActionResult> PostPicture(IFormFile file)
        {
            BlobContainerClient container = BlobUtility.GetThumbsContainer();
            var filename = $"{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}{HttpUtility.UrlPathEncode(file.FileName.Replace(" ", ""))}";
            await container.UploadBlobAsync(filename, file.OpenReadStream());
            
            await using (var client = new ServiceBusClient(_configuration.GetConnectionString("ServiceBusSenderConnection")))
            {
                ServiceBusSender sender = client.CreateSender(_configuration.GetValue<string>("QueueNameMails"));
                ServiceBusMessage message = new ServiceBusMessage($"The picture {filename} was uploaded! Send a fictional mail to me@you.us");

                await sender.SendMessageAsync(message);
            }

            return Ok();
        }
    }
}
