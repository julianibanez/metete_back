using MediatR;
using Newtonsoft.Json;

namespace Metete.Api.Features.GoogleMaps.Queries
{
    public class GetGeocode
    {
        public class Query : IRequest<GeocodingResponse>
        {
            public string Address { get; set; } = null!;
        }

        public class Handler : IRequestHandler<Query, GeocodingResponse>
        {
            private readonly IHttpClientFactory _httpClientFactory;
            private readonly IConfiguration _configuration;

            public Handler(IHttpClientFactory httpClientFactory, IConfiguration configuration)
            {
                _httpClientFactory = httpClientFactory;
                _configuration = configuration;
            }

            public async Task<GeocodingResponse> Handle(Query query, CancellationToken cancellationToken)
            {
                string apiKey = _configuration["GoogleMaps:ApiKey"]!;

                using var httpClient = _httpClientFactory.CreateClient("GoogleMaps");
                using var httpResponseMessage = await httpClient.GetAsync(
                    $"geocode/json?address={query.Address}&key={apiKey}", cancellationToken);

                string contentString = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<GeocodingResponse>(contentString)!;
                    return result;
                }
                else
                {
                    string error = string.IsNullOrEmpty(contentString) ? JsonConvert.SerializeObject(httpResponseMessage) : contentString;
                    throw new Exception(error);
                }
            }
        }

        public class GeocodingResponse
        {
            public Result[] results { get; set; }
            public string status { get; set; }
        }

        public class Result
        {
            public Address_Components[] address_components { get; set; }
            public string formatted_address { get; set; }
            public Geometry geometry { get; set; }
            public string place_id { get; set; }
            public Plus_Code plus_code { get; set; }
            public string[] types { get; set; }
        }

        public class Geometry
        {
            public Location location { get; set; }
            public string location_type { get; set; }
            public Viewport viewport { get; set; }
        }

        public class Location
        {
            public float lat { get; set; }
            public float lng { get; set; }
        }

        public class Viewport
        {
            public Northeast northeast { get; set; }
            public Southwest southwest { get; set; }
        }

        public class Northeast
        {
            public float lat { get; set; }
            public float lng { get; set; }
        }

        public class Southwest
        {
            public float lat { get; set; }
            public float lng { get; set; }
        }

        public class Plus_Code
        {
            public string compound_code { get; set; }
            public string global_code { get; set; }
        }

        public class Address_Components
        {
            public string long_name { get; set; }
            public string short_name { get; set; }
            public string[] types { get; set; }
        }
    }
}
