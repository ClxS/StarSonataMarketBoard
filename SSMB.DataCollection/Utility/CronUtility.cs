namespace SSMB.DataCollection.Utility
{
    using System;

    public class CronUtility
    {
        private static Random Rand;

        static CronUtility()
        {
            Rand = new Random();
        }

        public static string GetRandom12HourCron()
        {
            var minute = Rand.Next(0, 60);
            var hour = Rand.Next(0, 24);
            var hour2 = (hour + 12) % 24;
            return $"{minute} {hour},{hour2} * * *";
        }
    }
}
