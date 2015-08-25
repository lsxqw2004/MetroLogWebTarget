using System.Collections.Generic;

namespace MetroLogWebTarget.Domain
{
    public class LogEnvironment:BaseEntity
    {


        public string PackageArchitecture { get; set; }

        public string PackageFullName { get; set; }

        public string PackagePublisher { get; set; }

        public string PackagePublisherId { get; set; }

        public string PackageResourceId { get; set; }

        public string PackageVersion { get; set; }

        public string InstallationId { get; set; }

        
        public List<LogEvent> Events { get; set; } 
    }
}
