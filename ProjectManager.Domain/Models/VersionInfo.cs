namespace ProjectManager.Domain.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("VersionInfo")]
    public class VersionInfo
    {
        [Key]
        public long Version { get; set; }

        public DateTime? AppliedOn { get; set; }

        [StringLength(1024)]
        public string Description { get; set; }
    }
}
