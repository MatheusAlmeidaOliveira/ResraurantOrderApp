using System.ComponentModel.DataAnnotations;

namespace Api.Domain.Dtos
{
    public class OrderDto
    {
        [Required(ErrorMessage="Input is required")]
        [MinLength(7, ErrorMessage="The minimal lenght is of seven characters")]
        public string Input { get; set; }
    }
}