using Microsoft.Extensions.FileProviders;
using System.ComponentModel;

namespace AdviseTheTourist.Models
{

    enum PlaceType
    {
        Hotel,
        Restaurant,
        Museum,
        City,
        Other
    }

    public class PlaceModel
    {
        public Place? Place { get; set; }

        public Hotel? Hotel { get; set; }

        public List<HotelFacility>? HotelFacilities { get; set; }

        public List<HotelRoom>? HotelRooms { get; set; }

        public City? City { get; set; }

        public Restaurant? Restaurant { get; set; }

        public Museum? Museum { get; set; }

        public List<MuseumMonument>? MuseumMonuments { get; set; }

        public List<MuseumTicket>? MuseumTickets { get; set; }

        public List<Comment> Comments { get; set; } = new List<Comment>();

        public List<Hashtag> Hashtags { get; set; } = new List<Hashtag>();

        public List<Question> Questions { get; set; } = new List<Question>();

        public List<PlacePhoto> Photos { get; set; } = new List<PlacePhoto>();

        public List<RatingCriteria> RatingCriterias { get; set; } = new List<RatingCriteria>();

    }


    public class NewPhotoModel : PlacePhoto
    {
        public IFormFile? PhotoFile { get; set; }
    }

    public class PlaceViewModel : PlaceModel
    {
        [DisplayName("Visits")]
        public int VisitsNo { get; set; }

        public int LikesNo { get; set; }

        public Visit? Visit { get; set; }

        public List<RatingModel> RatingModels { get; set; } = new List<RatingModel>();
    }

    public class RatingModel
    {
        public string CriteriaName { get; set; } = string.Empty;

        public double Value { get; set; }

        public bool Active {  get; set; }
    }
}
