namespace Hibernate.Sample.Test.Domain
{
    public class User2
    {
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }
        public virtual long Version { get; set; }

        protected bool Equals(User2 other)
        {
            return Id == other.Id && string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((User2) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Id.GetHashCode()*397) ^ (Name != null ? Name.GetHashCode() : 0);
            }
        }
    }
}