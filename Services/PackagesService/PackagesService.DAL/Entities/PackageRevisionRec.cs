using System.ComponentModel.DataAnnotations;

namespace PackagesService.DAL.Entities
{
    public class PackageRevisionRec
    {
        public int PackageId { get; set; }
        public PackageRec Package { get; set; }


        [MaxLength(20)]
        public string VersionNumber { get; set; }
    }
}