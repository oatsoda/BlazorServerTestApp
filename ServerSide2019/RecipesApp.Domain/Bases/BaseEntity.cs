using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace RecipesApp.Domain.Bases
{
    public abstract class BaseEntity
    {
        [Required]
        [Timestamp]
        public byte[] RowVersion { get; private set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTimeOffset CreatedAt { get; internal set; }

        // Computed attribute causes the DbContext.SaveChanges override to break
        public DateTimeOffset UpdatedAt { get; internal set; }

        public string CreatedBy { get; internal set; }

        public string UpdatedBy { get; internal set; }
        
        [ForeignKey(nameof(CreatedBy))]
        public IdentityUser CreatedByUser { get; set; }

        [ForeignKey(nameof(UpdatedBy))]
        public IdentityUser UpdatedByUser { get; set; }

        //[NotMapped]
        //public ICollection<INotification> Events { get; } = new List<INotification>();
    }
}
