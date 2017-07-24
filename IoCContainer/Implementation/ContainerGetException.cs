using System;

namespace IoCContainer.Implementation
{
    public class ContainerGetException : Exception
    {
        public ContainerGetException(Type type, Exception innerException = null) 
            : base($"Can't get type {type.FullName} by container", innerException)
        {
        }
    }
}