using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MetroLogWebTarget.Web.Models
{
    public class LogEnvironmentModel
    {
        public int Id { get; set; }

        public string PackageArchitecture { get; set; }

        public string PackageFullName { get; set; }

        public string PackagePublisher { get; set; }

        public string PackagePublisherId { get; set; }

        public string PackageResourceId { get; set; }

        public string PackageVersion { get; set; }

        public string InstallationId { get; set; }

    }
}