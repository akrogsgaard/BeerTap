using System;

namespace Infrastructure
{
    public class DefaultTimeProvider : TimeProvider
    {
        private readonly static DefaultTimeProvider _instance = new DefaultTimeProvider();

        private DefaultTimeProvider() { }

        public override DateTime UtcNow
        {
            get { return DateTime.UtcNow; }
        }

        public static DefaultTimeProvider Instance
        {
            get { return DefaultTimeProvider._instance; }
        }
    }
}
