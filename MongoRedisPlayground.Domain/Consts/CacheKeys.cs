namespace MongoRedisPlayground.Domain.Consts;

public static class CacheKeys
{
    public static class Heroes
    {
        public const string All = "heroes";

        public static string ById(string id)
        {
            return $"hero:{id}";
        }
    }

    public static class HeroAttributes
    {
        public const string All = "heroAttributes";

        public static string ById(string id)
        {
            return $"heroAttribute:{id}";
        }
    }
}