using Metete.WorkerService.Dtos;
using Newtonsoft.Json;
using System.Text;

namespace Metete.WorkerService.Services
{
    public class MeteteService
    {
        private readonly ILogger<MeteteService> _logger;
        private readonly System.Net.Http.IHttpClientFactory _httpClientFactory;

        public MeteteService(ILogger<MeteteService> logger,
           IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<NotificacionDto>> GetPendingPushNotificacionesAsync()
        {
            using (var client = _httpClientFactory.CreateClient("Metete"))
            {
                using (HttpResponseMessage response = await client.GetAsync("notificaciones/pending-push"))
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        List<NotificacionDto> notificaciones = JsonConvert.DeserializeObject<List<NotificacionDto>>(responseBody)!;

                        return notificaciones;
                    }
                    else
                    {
                        string error = string.IsNullOrEmpty(responseBody) ? JsonConvert.SerializeObject(response) : responseBody;
                        throw new Exception(error);
                    }
                }
            }
        }

        public async Task MarkNotificacionAsEnviadaAsync(int id)
        {
            using (var client = _httpClientFactory.CreateClient("Metete"))
            {
                using (HttpResponseMessage response = await client.PutAsync($"notificaciones/{id}/mark-enviada", null))
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        string error = string.IsNullOrEmpty(responseBody) ? JsonConvert.SerializeObject(response) : responseBody;
                        throw new Exception(error);
                    }
                }
            }
        }

        public async Task MarkNotificacionAsFallidaAsync(int id)
        {
            using (var client = _httpClientFactory.CreateClient("Metete"))
            {
                using (HttpResponseMessage response = await client.PutAsync($"notificaciones/{id}/mark-fallida", null))
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        string error = string.IsNullOrEmpty(responseBody) ? JsonConvert.SerializeObject(response) : responseBody;
                        throw new Exception(error);
                    }
                }
            }
        }
    }
}
