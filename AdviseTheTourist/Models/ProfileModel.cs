using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AdviseTheTourist.Models
{
    
    public class ProfileModel
    {
        public bool IsActive { get; set; }

        public Member? Member { get; set; }

        public List<FriendModel> Friends {  get; set; } = new List<FriendModel>();
        
        public List<MemberAddress> MemberAddresses { get; set; } = new List<MemberAddress>();

        public List<MemberPhoneNo> MemberPhoneNumbers { get; set; } = new List<MemberPhoneNo>();

        public List<VisitModel> Visits { get; set; } = new List<VisitModel>();

        public List<Place> AdminPlaces { get; set; } = new List<Place>();
    }

    public class FriendModel
    {
        public string Email { get; set; } = string.Empty;

        public string Firstname { get; set; } = string.Empty;

        public string Lastname { get; set; } = string.Empty;

        public string Nationality { get; set; } = string.Empty;

        public string Image { get; set; } = string.Empty;
    }

    public class VisitModel
    {
        public string Name { get; set; } = string.Empty;

        public string Photo { get; set; } = string.Empty;

        public bool Liked { get; set; }
    }
}
