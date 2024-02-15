using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdviseTheTourist.Models
{
    [PrimaryKey("Email")]
    public class Member
    {
        [MaxLength(50)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(50)]
        [DataType(DataType.Password)]
        [MinLength(8)]
        public string Password { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Firstname { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Lastname { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Nationality { get; set; } = string.Empty;

        [MaxLength(50)]
        [DataType(DataType.ImageUrl)]
        public string Image {  get; set; } = string.Empty;

    }


    [PrimaryKey("Name")]
    public class Place
    {
        [MaxLength(50)]
        [Required]
        public string Name { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [Required]
        public DateOnly Buildingdate { get; set; }

        [MaxLength(50)]
        [Required]
        public string MapLongitude { get; set; } = string.Empty;

        [MaxLength(50)]
        [Required]
        public string MapLatitude { get; set; } = string.Empty;

        [MaxLength(50)]
        [DataType(DataType.EmailAddress)]
        public string AdminEmail { get; set; } = string.Empty;

        [EnumDataType(typeof(PlaceType))]
        public byte Type { get; set; } = (int) PlaceType.Other;

        [MaxLength(50)]
        [DataType(DataType.ImageUrl)]
        public string Image { get; set; } = string.Empty;

    }


    [PrimaryKey("Name")]
    public class City
    {
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Location { get; set; } = string.Empty;

        public bool Coastal { get; set; }

    }


    [PrimaryKey("Name")]
    public class Restaurant
    {
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Cuisine { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Style { get; set; } = string.Empty;

    }


    [PrimaryKey("Name")]
    public class Hotel
    {
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

    }


    [PrimaryKey("Name")]
    public class Museum
    {
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [DataType(DataType.Time)]
        public TimeOnly? OpenTime { get; set; }

        [DataType(DataType.Time)]
        public TimeOnly? CloseTime { get; set; }

    }


    [PrimaryKey("Name", "PlaceName")]
    public class RatingCriteria
    {
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(50)]
        [DataType(DataType.EmailAddress)]
        public string MemberEmail { get; set; } = string.Empty;

        [MaxLength(50)]
        public string PlaceName { get; set; } = string.Empty;

    }


    [PrimaryKey("MemberEmail", "Member2Email")]
    public class FriendRequest
    {
        [MaxLength(50)]
        [DataType(DataType.EmailAddress)]
        public string MemberEmail { get; set; } = string.Empty;

        [MaxLength(50)]
        [DataType(DataType.EmailAddress)]
        public string Member2Email { get; set; } = string.Empty;

        public bool Accepted { get; set; }

    }


    [PrimaryKey("MemberEmail", "Member2Email")]
    public class Friend
    {
        [MaxLength(50)]
        [DataType(DataType.EmailAddress)]
        public string MemberEmail { get; set; } = string.Empty;

        [MaxLength(50)]
        [DataType(DataType.EmailAddress)]
        public string Member2Email { get; set; } = string.Empty;

    }


    [PrimaryKey("MemberEmail", "Member2Email", "PlaceName")]
    public class Invitation
    {
        [MaxLength(50)]
        [DataType(DataType.EmailAddress)]
        [DisplayName("From")]
        public string MemberEmail { get; set; } = string.Empty;

        [MaxLength(50)]
        [DataType(DataType.EmailAddress)]
        [DisplayName("To")]
        public string Member2Email { get; set; } = string.Empty;

        [MaxLength(50)]
        [DisplayName("Place")]
        public string PlaceName { get; set; } = string.Empty;

        [DisplayName("Status")]
        public bool Accepted { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayName("Time")]
        public DateTime SentTime { get; set; }

    }


    [PrimaryKey("MemberEmail", "Member2Email", "SentTime")]
    public class Message
    {

        [MaxLength(50)]
        [DataType(DataType.EmailAddress)]
        [DisplayName("From")]
        public string MemberEmail { get; set; } = string.Empty;

        [MaxLength(50)]
        [DisplayName("To")]
        [DataType(DataType.EmailAddress)]
        public string Member2Email { get; set; } = string.Empty;

        [DisplayName("Time")]
        public string SentTime { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Content { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Reply { get; set; } = string.Empty;
    }


    [PrimaryKey("MemberEmail", "PlaceName")]
    public class Visit
    {
        public bool Liked { get; set; }

        [MaxLength(50)]
        [DataType(DataType.EmailAddress)]
        public string MemberEmail { get; set; } = string.Empty;

        [MaxLength(50)]
        public string PlaceName { get; set; } = string.Empty;

    }


    [PrimaryKey("MemberEmail", "PlaceName", "Src")]
    public class Image
    {
        [MaxLength(50)]
        [DataType(DataType.ImageUrl)]
        public string Src { get; set; } = string.Empty;

        [MaxLength(50)]
        [DataType(DataType.EmailAddress)]
        public string MemberEmail { get; set; } = string.Empty;

        [MaxLength(50)]
        public string PlaceName { get; set; } = string.Empty;

    }


    [PrimaryKey("MemberEmail", "PlaceName", "Content")]
    public class Comment
    {
        [MaxLength(100)]
        public string Content { get; set; } = string.Empty;

        [MaxLength(50)]
        [DataType(DataType.EmailAddress)]
        public string MemberEmail { get; set; } = string.Empty;

        [MaxLength(50)]
        public string PlaceName { get; set; } = string.Empty;

    }


    [PrimaryKey("MemberEmail", "PlaceName", "Content")]
    public class Hashtag
    {
        [MaxLength(50)]
        public string Content { get; set; } = string.Empty;

        [MaxLength(50)]
        [DataType(DataType.EmailAddress)]
        public string MemberEmail { get; set; } = string.Empty;

        [MaxLength(50)]
        public string PlaceName { get; set; } = string.Empty;

    }


    [PrimaryKey("MemberEmail", "PlaceName", "Content")]
    public class Question
    {
        [MaxLength(50)]
        public string Content { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Answer { get; set; } = string.Empty;

        [MaxLength(50)]
        [DataType(DataType.EmailAddress)]
        public string MemberEmail { get; set; } = string.Empty;

        [MaxLength(50)]
        public string PlaceName { get; set; } = string.Empty;

    }


    [PrimaryKey("MemberEmail", "CriteriaName", "PlaceName")]
    public class Rating
    {
        public int? Value { get; set; }

        [MaxLength(50)]
        [DataType(DataType.EmailAddress)]
        public string MemberEmail { get; set; } = string.Empty;

        [MaxLength(50)]
        public string CriteriaName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string PlaceName { get; set; } = string.Empty;

    }


    [PrimaryKey("MemberEmail", "PhoneNo")]
    public class MemberPhoneNo
    {
        [MaxLength(50)]
        [DataType(DataType.EmailAddress)]
        public string MemberEmail { get; set; } = string.Empty;

        [MaxLength(50)]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNo { get; set; } = string.Empty;

    }


    [PrimaryKey("MemberEmail", "Address")]
    public class MemberAddress
    {
        [MaxLength(50)]
        [DataType(DataType.EmailAddress)]
        public string MemberEmail { get; set; } = string.Empty;

        [MaxLength(50)]
        [DataType(DataType.EmailAddress)]
        public string Address { get; set; } = string.Empty;

    }


    [PrimaryKey("HotelName", "Type")]
    public class HotelRoom
    {
        [MaxLength(50)]
        public string HotelName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Type { get; set; } = string.Empty;

        public double Price { get; set; }

    }


    [PrimaryKey("HotelName", "Name")]
    public class HotelFacility
    {
        [MaxLength(50)]
        public string HotelName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Description { get; set; } = string.Empty;

    }


    [PrimaryKey("MuseumName", "Type")]
    public class MuseumTicket
    {
        [MaxLength(50)]
        public string MuseumName { get; set; } = string.Empty;

        public double Price { get; set; }

        [MaxLength(50)]
        public string Type { get; set; } = string.Empty;

    }


    [PrimaryKey("MuseumName", "Name")]
    public class MuseumMonument
    {
        [MaxLength(50)]
        public string MuseumName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Description { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

    }


    [PrimaryKey("AdminEmail", "PlaceName", "Photo")]
    public class PlacePhoto
    {
        [MaxLength(50)]
        [DataType(DataType.EmailAddress)]
        public string AdminEmail { get; set; } = string.Empty;

        [MaxLength(50)]
        public string PlaceName { get; set; } = string.Empty;

        [MaxLength(50)]
        [DataType(DataType.ImageUrl)]
        public string Photo { get; set; } = string.Empty;

    }


    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<Member> Member { get; set; } = default!;
        public DbSet<Place> Place { get; set; } = default!;
        public DbSet<City> City { get; set; } = default!;
        public DbSet<Restaurant> Restaurant { get; set; } = default!;
        public DbSet<Hotel> Hotel { get; set; } = default!;
        public DbSet<Museum> Museum { get; set; } = default!;
        public DbSet<RatingCriteria> RatingCriteria { get; set; } = default!;
        public DbSet<FriendRequest> FriendRequest { get; set; } = default!;
        public DbSet<Friend> Friend { get; set; } = default!;
        public DbSet<Invitation> Invitation { get; set; } = default!;
        public DbSet<Message> Message { get; set; } = default!;
        public DbSet<Visit> Visit { get; set; } = default!;
        public DbSet<Image> Image { get; set; } = default!;
        public DbSet<Comment> Comment { get; set; } = default!;
        public DbSet<Hashtag> Hashtag { get; set; } = default!;
        public DbSet<Question> Question { get; set; } = default!;
        public DbSet<Rating> Rating { get; set; } = default!;
        public DbSet<MemberPhoneNo> MemberPhoneNo { get; set; } = default!;
        public DbSet<MemberAddress> MemberAddress { get; set; } = default!;
        public DbSet<HotelRoom> HotelRoom { get; set; } = default!;
        public DbSet<HotelFacility> HotelFacility { get; set; } = default!;
        public DbSet<MuseumTicket> MuseumTicket { get; set; } = default!;
        public DbSet<MuseumMonument> MuseumMonument { get; set; } = default!;
        public DbSet<PlacePhoto> PlacePhoto { get; set; } = default!;

    }
}