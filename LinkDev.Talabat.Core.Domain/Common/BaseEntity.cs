namespace LinkDev.Talabat.Core.Domain.Common
{
#nullable disable
    public abstract class BaseEntity<TKey> where TKey : IEquatable<TKey>
    {
        public  TKey Id { get; set; }
    }
}
