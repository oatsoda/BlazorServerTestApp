using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipesApp.Domain.Bases
{
    public abstract class BaseUniqueEntity : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local - required by EF
        public Guid Id { get; protected set; }

        protected BaseUniqueEntity()
        {
            Id = Guid.NewGuid();
        }
    }
}