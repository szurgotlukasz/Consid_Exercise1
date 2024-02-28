using System;
using MediatR;

namespace Exercise1.Queries.ListAllLogs
{
    public class ListAllLogsQuery : IRequest<ListAllLogsResponse>
	{
		public ListAllLogsQuery(DateTimeOffset from, DateTimeOffset to)
		{
			From = from;
			To = to;
		}

        public DateTimeOffset From { get; }
        public DateTimeOffset To { get; }
    }
}

