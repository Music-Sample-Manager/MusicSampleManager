using PackagesService.Domain;
using System;

namespace PackagesService.UseCases
{
    public class AddPackageToRepository
    {
        private readonly IPackageRepository _packageRepository;
        private readonly string _packageName;
        private readonly string _packageDescription;
        private readonly int _authorId;

        public AddPackageToRepository(IPackageRepository packageRepository, string packageName,
                                      string packageDescription, int authorId)
        {
            if (packageName == string.Empty)
            {
                throw new ArgumentException(nameof(packageName));
            }

            if (packageDescription == string.Empty)
            {
                throw new ArgumentException(nameof(packageName));
            }

            if (authorId <= 0)
            {
                throw new ArgumentException(nameof(authorId));
            }

            _packageRepository = packageRepository ?? throw new ArgumentNullException(nameof(packageRepository));
            _packageName = packageName ?? throw new ArgumentNullException(nameof(packageName));
            _packageDescription = packageDescription ?? throw new ArgumentNullException(nameof(packageDescription));
            _authorId = authorId;
        }

        public void Execute()
        {
            _packageRepository.Add(new Package(0, _packageName, _packageDescription, _authorId));
        }
    }
}