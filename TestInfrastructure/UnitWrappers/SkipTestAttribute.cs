using NUnit.Framework;

namespace SKBKontur.TestInfrastructure.UnitWrappers
{
    public class SkipTestAttribute : IgnoreAttribute
    {
        public SkipTestAttribute(string reason) : base(reason)
        {
        }
    }
}