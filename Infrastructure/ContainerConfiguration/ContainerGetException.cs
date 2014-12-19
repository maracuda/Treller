using System;

namespace SKBKontur.Infrastructure.ContainerConfiguration
{
    public class ContainerGetException : Exception
    {
        public ContainerGetException(Type type, Exception innerException = null) 
            : base(string.Format("Can't get type {0} by container", type.FullName), innerException)
        {
        }
    }
}