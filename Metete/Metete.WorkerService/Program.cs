using Metete.WorkerService.Jobs;
using Metete.WorkerService.Services;
using Quartz;
using Quartz.AspNetCore;
using Serilog;

Environment.CurrentDirectory = AppContext.BaseDirectory;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory();

            //// Just use the name of your job that you created in the Jobs folder.
            var jobKey = new JobKey("SendNotificationJob");
            q.AddJob<SendNotificationJob>(opts => opts.WithIdentity(jobKey));

            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("SendNotificationJob-trigger")               
                .WithCronSchedule(hostContext.Configuration["Schedule:CronExpression"]!)
            );
        });

        // ASP.NET Core hosting
        services.AddQuartzServer(options =>
        {
            // when shutting down we want jobs to complete gracefully
            options.WaitForJobsToComplete = true;
        });        

        // HttpClient
        services.AddHttpClient("Metete", configureClient =>
        {
            string baseUrl = hostContext.Configuration["Apis:MeteteBaseUrl"]!;
            baseUrl += baseUrl.EndsWith("/") ? string.Empty : "/";

            configureClient.BaseAddress = new Uri(baseUrl);
        });

        // Services
        services.AddSingleton<MeteteService>();
    })
    .UseSerilog((hostContext, loggerConfiguration) => loggerConfiguration
        .ReadFrom.Configuration(hostContext.Configuration))
    .Build();

host.Run();
