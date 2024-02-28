using System;
namespace Exercise1
{
    public class SystemTimeProvider : ISystemTimeProvider
    {
        public DateTimeOffset Now => DateTimeOffset.Now;
    }
}

