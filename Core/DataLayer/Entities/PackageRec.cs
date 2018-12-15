using DataLayer.Entities;
using Domain;
using System.Collections.Generic;
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

        [Required]
        public string Description { get; set; }

        [Required]
        public int AuthorId { get; set; }


        public Package ToPackage => new Package(Identifier);
    }
}