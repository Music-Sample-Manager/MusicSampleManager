using PackagesService.Domain;
using System.ComponentModel.DataAnnotations;

namespace PackagesService.DAL.Entities
{
    public class PackageRec
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Identifier { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int AuthorId { get; set; }


        public Package ToPackage => new Package(Identifier, Description, AuthorId);
    }
}