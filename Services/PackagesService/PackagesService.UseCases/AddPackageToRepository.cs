using PackagesService.DAL;
using PackagesService.DAL.Entities;
using System;

namespace PackagesService.UseCases
{
    public class AddPackageToRepository
    {
        private readonly MSMDbContext _dbContext;
        private readonly string _packageName;
        private readonly string _description;
        private readonly int _authorId;

        public AddPackageToRepository(string packageName, string description, int authorId)
        {
            // TODO I shouldn't be reading the conn string here.
            var connectionString = Environment.GetEnvironmentVariable("DatabaseConnectionString");
            _dbContext = new MSMDbContext(false, connectionString);
            _packageName = packageName;
            _description = description;
            _authorId = authorId;
        }

        public void Execute()
        {
            _dbContext.Packages.Add(new PackageRec()
            {
                Identifier = _packageName,
                Description = _description,
                AuthorId = _authorId
            });

            _dbContext.SaveChanges();
        }
    }
}