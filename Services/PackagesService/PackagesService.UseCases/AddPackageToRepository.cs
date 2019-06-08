using PackagesService.Domain;

namespace PackagesService.UseCases
{
    public class AddPackageToRepository
    {
        private readonly IPackageRepository _packageRepository;
        private readonly string _packageName;
        private readonly string _description;
        private readonly int _authorId;

        public AddPackageToRepository(IPackageRepository packageRepository, string packageName,
                                      string description, int authorId)
        {
            _packageRepository = packageRepository;
            _packageName = packageName;
            _description = description;
            _authorId = authorId;
        }

        public void Execute()
        {
            _packageRepository.Add(new Package(_packageName, _description, _authorId));
        }
    }
}