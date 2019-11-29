using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        //[NotMapped]
        //public ICollection<INotification> Events { get; } = new List<INotification>();
    }
}
