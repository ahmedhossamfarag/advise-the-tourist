using System.Web;
namespace AdviseTheTourist.Models
{
    public class NewMemberModel : Member
    {
        public IFormFile? ImageFile { get; set; }
    }

    public class MemberFriendModel
    {
        public string Email { get; set; } = string.Empty;
        public string Firstname { get; set; } = string.Empty;

        public string Lastname { get; set; } = string.Empty;

        public string Image { get; set; } = string.Empty;

        public bool IsFriend { get; set; }

        public bool IsRequest { get; set; }

        public bool IsResponse { get; set; }

        public bool? Accepted { get; set; } 
    }
}
