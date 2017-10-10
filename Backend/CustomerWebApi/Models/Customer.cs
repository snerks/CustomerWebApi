using System.ComponentModel.DataAnnotations;

namespace CustomerWebApi.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
