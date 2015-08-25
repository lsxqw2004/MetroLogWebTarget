namespace MetroLogWebTarget.Domain
{
    public class UserClaim
    {

        public virtual int Id { get; set; }

        public virtual int UserId { get; set; }

        public virtual string ClaimType { get; set; }

        public virtual string ClaimValue { get; set; }
    }
}
