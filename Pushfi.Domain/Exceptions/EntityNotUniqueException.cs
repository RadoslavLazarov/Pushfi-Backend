using System;
using System.Runtime.Serialization;

namespace Pushfi.Domain.Exceptions
{
	public class EntityNotUniqueException : Exception
	{
		public EntityNotUniqueException()
		{
		}

		public EntityNotUniqueException(string message) : base(message)
		{
		}

		public EntityNotUniqueException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected EntityNotUniqueException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
