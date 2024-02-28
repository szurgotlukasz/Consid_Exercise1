using System;

namespace Exercise1
{
    public interface ISystemTimeProvider
    {
        DateTimeOffset Now { get; }
    }
}