namespace LinkDev.Talabat.Core.Domain.Common
{
#nullable disable
    public interface IBaseAuditableEntity
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedOn { get; set; }
    }
    public abstract class BaseAuditableEntity<TKey> : BaseEntity<TKey>, IBaseAuditableEntity
      where TKey : IEquatable<TKey>
    {


        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }         
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedOn { get; set; }    

    }
}
