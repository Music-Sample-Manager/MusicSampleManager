using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    public class Package
    {
        public readonly int Id;

        public readonly string Identifier;

        public virtual IEnumerable<PackageRevision> AllVersions { get; private set; }

        public Package(string identifier)
        {
            Identifier = identifier;
        }

        public PackageRevision LatestVersion()
        {
            // TODO This should return a package based on revision.
            return AllVersions.Last();
        }
    }
}
