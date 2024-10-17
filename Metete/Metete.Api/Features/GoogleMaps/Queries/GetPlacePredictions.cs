using MediatR;
using Newtonsoft.Json;

namespace Metete.Api.Features.GoogleMaps.Queries
{
    public class GetPlacePredictions
    {
        public class Query : IRequest<PlaceAutoCompleteResponse>
        {
            public string Input { get; set; } = null!;
        }

        public class Handler : IRequestHandler<Query, PlaceAutoCompleteResponse>
        {
            private readonly IHttpClientFactory _httpClientFactory;
            private readonly IConfiguration _configuration;

            public Handler(IHttpClientFactory httpClientFactory, IConfiguration configuration)
            {
                _httpClientFactory = httpClientFactory;
                _configuration = configuration;
            }

            public async Task<PlaceAutoCompleteResponse> Handle(Query query, CancellationToken cancellationToken)
            {
                string apiKey = _configuration["GoogleMaps:ApiKey"]!;

                using var httpClient = _httpClientFactory.CreateClient("GoogleMaps");
                using var httpResponseMessage = await httpClient.GetAsync(
                    $"place/autocomplete/json?input={query.Input}&key={apiKey}", cancellationToken);

                string contentString = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<PlaceAutoCompleteResponse>(contentString)!;
                    return result;
                }
                else
                {
                    string error = string.IsNullOrEmpty(contentString) ? JsonConvert.SerializeObject(httpResponseMessage) : contentString;
                    throw new Exception(error);
                }
            }
        }

        public class PlaceAutoCompleteResponse
        {
            public Prediction[] predictions { get; set; }
            public string status { get; set; }
        }

        public class Prediction
        {
            public string description { get; set; }
            public Matched_Substrings[] matched_substrings { get; set; }
            public string place_id { get; set; }
            public string reference { get; set; }
            public Structured_Formatting structured_formatting { get; set; }
            public Term[] terms { get; set; }
            public string[] types { get; set; }
        }

        public class Structured_Formatting
        {
            public string main_text { get; set; }
            public Main_Text_Matched_Substrings[] main_text_matched_substrings { get; set; }
            public string secondary_text { get; set; }
            public Secondary_Text_Matched_Substrings[] secondary_text_matched_substrings { get; set; }
        }

        public class Main_Text_Matched_Substrings
        {
            public int length { get; set; }
            public int offset { get; set; }
        }

        public class Secondary_Text_Matched_Substrings
        {
            public int length { get; set; }
            public int offset { get; set; }
        }

        public class Matched_Substrings
        {
            public int length { get; set; }
            public int offset { get; set; }
        }

        public class Term
        {
            public int offset { get; set; }
            public string value { get; set; }
        }
    }
}
