namespace Hibernate.Sample.Test.Domain
{
    public class Address
    {
        public virtual long Id { get; set; }
        public virtual string AddressDetail { get; set; }
        public virtual string ZipCode { get; set; }
        public virtual User User { get; set; }

        protected bool Equals(Address other)
        {
            return Id == other.Id && string.Equals(AddressDetail, other.AddressDetail) && string.Equals(ZipCode, other.ZipCode);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Address) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();
                hashCode = (hashCode*397) ^ (AddressDetail != null ? AddressDetail.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (ZipCode != null ? ZipCode.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}