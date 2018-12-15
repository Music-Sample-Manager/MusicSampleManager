using Core.DataLayer;
using System.ComponentModel.DataAnnotations;

namespace DataLayer.Entities
{
    public class PackageRevisionRec
    {
        public int PackageId { get; set; }
        public PackageRec Package { get; set; }


        [MaxLength(20)]
        public string VersionNumber { get; set; }
    }
}