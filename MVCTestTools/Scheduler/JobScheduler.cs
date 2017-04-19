using Quartz;
using Quartz.Impl;

namespace MVCTestTools.Scheduler
{
    public class JobScheduler
    {
        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail job = JobBuilder.Create<Autotest>().Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule
                  (s =>
                     s.WithIntervalInHours(24)
                    .OnEveryDay()
                    .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(12, 30))
                  )
                .Build();

            //ITrigger trigger = TriggerBuilder.Create()
            //            .WithIdentity("trigger1", "group1")
            //            .StartNow()
            //            .WithSimpleSchedule(x => x
            //            .WithIntervalInSeconds(60)
            //            .WithRepeatCount(2))
            //        .Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}