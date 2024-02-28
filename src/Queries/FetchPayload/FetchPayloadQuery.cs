using System;
using MediatR;

namespace Exercise1.Queries
{
    public class FetchPayloadQuery : IRequest<FetchPayloadQueryResponse>
	{
		public FetchPayloadQuery(Guid id)
		{
			Id = id;
		}

        public Guid Id { get; }
    }
}

