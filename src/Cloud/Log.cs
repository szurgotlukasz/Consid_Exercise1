using System;

namespace Exercise1.Cloud
{
    public record Log
    {
		public Log(Status status, DateTimeOffset timeStamp)
		{
			this.Status = status;
			this.Timestamp = timeStamp;
		}

        public Status Status { get; }
        public DateTimeOffset Timestamp { get; }

        public override string ToString()
        {
            return $"{Status} - Attempt at {Timestamp.ToString("hh:mm:ss")}";
		}
    }

	public enum Status
	{
		Failure,
		Success
	}
}

