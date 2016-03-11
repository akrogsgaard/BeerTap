using System;

namespace Infrastructure
{
    public abstract class TimeProvider
    {
        static TimeProvider _current = DefaultTimeProvider.Instance;

        public static TimeProvider Current
        {
            get { return _current; }

            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                _current = value;
            }
        }

        public abstract DateTime UtcNow { get; }

        public DateTime SqlMin
        {
            get { return new DateTime(1753, 1, 1); }
        }


        public static void ResetToDefault()
        {
            _current = DefaultTimeProvider.Instance;
        }
    }
}