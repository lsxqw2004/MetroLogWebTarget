﻿namespace MetroLogWebTarget.Domain
{
    public class UserClaim
    {

        public virtual int Id { get; set; }

        /// <summary>
        /// User Id for the user who owns this login
        /// 
        /// </summary>
        public virtual int UserId { get; set; }

        /// <summary>
        /// Claim type
        /// 
        /// </summary>
        public virtual string ClaimType { get; set; }

        /// <summary>
        /// Claim value
        /// 
        /// </summary>
        public virtual string ClaimValue { get; set; }
    }
}
