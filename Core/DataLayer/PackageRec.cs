using Domain;
using System.ComponentModel.DataAnnotations;

namespace Core.DataLayer
{
    public class PackageRec
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Identifier { get; set; }


        public Package ToPackage => new Package(Identifier);
    }
}