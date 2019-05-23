using PackagesService.Domain;

namespace DesktopClient.Domain
{
    public interface IPackageStore
    {
        void Initialize();

        void AddPackage(PackageRevision packageRev);
    }
}
