using System.ComponentModel.DataAnnotations;

namespace ElegantVogue.Models.ViewModels
{
    public class ProductFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom est obligatoire")]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required(ErrorMessage = "Le prix est obligatoire")]
        [Range(0.01, 99999.99, ErrorMessage = "Le prix doit être supérieur à 0")]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        public string? ProductType { get; set; }

        public bool IsAvailable { get; set; } = true;

        public bool IsNewThisWeek { get; set; } = false;

        public bool IsFeatured { get; set; } = false;

        [Required(ErrorMessage = "La catégorie est obligatoire")]
        public int CategoryId { get; set; }

        public int? CollectionId { get; set; }

        public List<int> SelectedColorIds { get; set; } = new List<int>();

        public List<int> SelectedSizeIds { get; set; } = new List<int>();

        // Pour les listes déroulantes
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Collection> Collections { get; set; } = new List<Collection>();
        public List<Color> Colors { get; set; } = new List<Color>();
        public List<Size> Sizes { get; set; } = new List<Size>();
    }
}