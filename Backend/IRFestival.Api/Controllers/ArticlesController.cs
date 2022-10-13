using IRFestival.Api.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using System.Net;
using System.Runtime.InteropServices;

namespace IRFestival.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private CosmosClient _cosmosClient { get; set; }
        private Container _websiteArticlesContainer { get; set; }

        public ArticlesController(IConfiguration configuration)
        {
            _cosmosClient = new CosmosClient(configuration.GetConnectionString("CosmosConnection"));
            _websiteArticlesContainer = _cosmosClient.GetContainer("IRFestivalArticles", "WebsiteArticles");


        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Article))]
        public async Task<ActionResult> GetAsync()
        {
            var result = new List<Article>();

            var queryDefinition = _websiteArticlesContainer.GetItemLinqQueryable<Article>()
                .Where(p => p.Status == nameof(Status.Published))
                .OrderBy(p => p.Date);

            var iterator = queryDefinition.ToFeedIterator();
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                result = response.ToList();
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> CreateItemAsync()
        {
            var dummyData = new Article
            {
                Id = "855ff26c-20b3-4351-9477-0c645500a709",
                Tag = "Intro2515151",
                Title = "Some random message",
                Date = DateTime.Now,
                ImagePath = "marvingaye.jpg",
                Message = "Message21899828",
                Status = Status.Unpublished.ToString()
            };
             var a = await _websiteArticlesContainer.CreateItemAsync(dummyData);

            return Ok(dummyData);
        }
    }
}
