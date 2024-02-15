using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;

namespace AdviseTheTourist.Models
{

    public class NewPlaceModel : Place, IValidatableObject
    {
        [MaxLength(50)]
        public string Location { get; set; } = string.Empty;

        public bool Coastal { get; set; }

        [MaxLength(50)]
        public string Cuisine { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Style { get; set; } = string.Empty; 
        
        [DataType(DataType.Time)]
        public TimeOnly? OpenTime { get; set; }

        [DataType(DataType.Time)]
        public TimeOnly? CloseTime { get; set; }

        public IFormFile? ImageFile { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            switch((PlaceType)Type)
            {
                case PlaceType.City:
                    if (string.IsNullOrEmpty(Location))
                        yield return new ValidationResult ("Location is required", new[] { nameof(Location) } );
                    break;
                case PlaceType.Restaurant:
                    if (string.IsNullOrEmpty(Cuisine))
                        yield return new ValidationResult ("Cuisine is required", new[] { nameof(Cuisine) } );
                     if (string.IsNullOrEmpty(Style))
                        yield return new ValidationResult ("Style is required", new[] { nameof(Style) } );
                    break;
                case PlaceType.Museum:
                    if(OpenTime == null)
                        yield return new ValidationResult("Open Time is required", new[] { nameof(OpenTime) });
                    if(CloseTime == null)
                        yield return new ValidationResult("Close Time is required", new[] { nameof(CloseTime) });
                    break;
            }
        }
    }
}
