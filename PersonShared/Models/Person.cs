using System.ComponentModel.DataAnnotations;

namespace PersonDemo.Shared.Models
{
    public class Person
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Phone]
        public string Telephone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
