using CSharpPractise.Web.Jobs;
using Microsoft.AspNetCore.Mvc;
using Quartz;

namespace CSharpPractise.Web.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly ISchedulerFactory _schedulerFactory;

    public WeatherForecastController(ILogger<WeatherForecastController> logger,
        ISchedulerFactory schedulerFactory)
    {
        _logger = logger;
        _schedulerFactory = schedulerFactory;
    }

    [HttpGet]
    public IEnumerable<WeatherForecast> GetWeatherForecast()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
    
    [HttpGet]
    public async Task<bool> ScheduleJob()
    {
        var scheduler = await _schedulerFactory.GetScheduler();
        
        var jobKey = new JobKey(name: "BackgroundJob", group: "JobGroup");
        var job = JobBuilder.Create<BackgroundJob>()
            .WithIdentity(jobKey)
            .UsingJobData("ConsoleOutput", "Executing background job using JobDataMap")
            .UsingJobData("UseJobDataMapConsoleOutput", false)
            .Build();
        
        var trigger = TriggerBuilder.Create()
            .WithIdentity(name: "RepeatingTrigger", group: "TriggerGroup")
            .WithSimpleSchedule(o => o
                .WithRepeatCount(2)
                .WithIntervalInSeconds(5))
            .StartAt(DateTimeOffset.Now.AddSeconds(10))
            .Build();

        if (await scheduler.CheckExists(jobKey)){
            await scheduler.DeleteJob(jobKey);
        }
        
        await scheduler.ScheduleJob(job, trigger);
        
        return true;
    }
}