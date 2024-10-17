using FirebaseAdmin.Messaging;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Quartz;
using Metete.WorkerService.Services;
using Metete.WorkerService.Dtos;
using System.Diagnostics;

namespace Metete.WorkerService.Jobs
{
    [DisallowConcurrentExecution]
    public class SendNotificationJob : IJob
    {
        private readonly ILogger<SendNotificationJob> _logger;
        private readonly MeteteService _metetService;

        public SendNotificationJob(ILogger<SendNotificationJob> logger, MeteteService meteteService)
        {
            _logger = logger;
            _metetService = meteteService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using (_logger.BeginScope("RequestId {0}", Guid.NewGuid()))
            {
                try
                {
                    Stopwatch stopWatch = new Stopwatch();

                    stopWatch.Start();

                    await DoWorkAsync();

                    stopWatch.Stop();

                    _logger.LogInformation("Tiempo de ejecución del proceso {elapsedTime} segundos", stopWatch.Elapsed.TotalSeconds);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Something went wrong");
                }
            }
        }

        private async Task DoWorkAsync()
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;

            if (app == null)
            {
                app = FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "metete-firebase-adminsdk.json")),
                });
            }

            FirebaseMessaging fcm = FirebaseMessaging.GetMessaging(app);

            List<NotificacionDto> notificaciones = await _metetService.GetPendingPushNotificacionesAsync();

            foreach (NotificacionDto notificacion in notificaciones)
            {
                try
                {
                    Dictionary<string, string> data = new Dictionary<string, string>();

                    if (notificacion.IdEvento > 0)
                    {
                        data.Add("evento", notificacion.IdEvento.Value.ToString());                        
                    }

                    if (notificacion.IdTipoNotificacion > 0)
                    {
                        data.Add("tipoNotificacion", notificacion.IdTipoNotificacion.Value.ToString());
                    }

                    Message message = new Message()
                    {
                        Notification = new Notification
                        {
                            Title = notificacion.Titulo,
                            Body = notificacion.Mensaje
                        },
                        Token = notificacion.FcmToken,
                        Data = data.Count > 0 ? data : null
                    };

                    string response = await fcm.SendAsync(message);

                    await _metetService.MarkNotificacionAsEnviadaAsync(notificacion.Id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error sending notification {notificacion.Id}");

                    await _metetService.MarkNotificacionAsFallidaAsync(notificacion.Id);
                }
            }
        }
    }
}