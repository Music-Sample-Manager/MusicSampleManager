using System;
using System.Collections.Generic;
using System.Linq;

namespace PackagesService.Domain
{
    public class Package
    {
        public readonly int Id;

        public readonly string Identifier;

        public readonly string Description;

        // TODO Need to replace this with some type of hydrated User/Author object.
        public readonly int AuthorId;

        public virtual IEnumerable<PackageRevision> AllVersions { get; private set; }

        public Package(string identifier, string description, int authorId)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            if (identifier == string.Empty)
            {
                throw new ArgumentException("Empty package identifiers are not allowed.");
            }

            Identifier = identifier;
            Description = description;
            AuthorId = authorId;
        }

        public PackageRevision LatestVersion()
        {
            // TODO This should return a package based on revision.
            return AllVersions.Last();
        }
    }
}
