namespace MongoRedisPlayground.Domain.Consts;

public static class SystemConsts
{
    public static class App
    {
        public const string NameApi = "MongoRedisPlayground.API";
        public const string NameApp = "MongoRedisPlayground";
    }

    public static class Time
    {
        public const int OneSecond = 1;
        public const int OneMinute = 60;
        public const int TenMinutes = 600;
        public const int OneHour = 3600;
        public const int HalfDay = 43200;
        public const int OneDay = 86400;
        public const int OneWeek = 604800;
        public const int OneMonth = 2629800;
        public const int OneYear = 31536000;

        public const int PlanDurationDaysFree = 14;
        public const int PlanDurationDays = 30;
    }
}