namespace AdviseTheTourist.Models
{
    public class RequestsModel
    {
        public List<FriendRequestModel> Requests { get; set; } = new List<FriendRequestModel>();

        public List<FriendResponseModel> Responses { get; set; } = new List<FriendResponseModel>();
    }

    public class FriendRequestModel
    {
        public string Firstname { get; set; } = string.Empty;

        public string Lastname { get; set; } = string.Empty;

        public string MemberEmail { get; set; } = string.Empty;

        public string Member2Email { get; set; } = string.Empty;

        public string Image { get; set; } = string.Empty;
    }

    public class FriendResponseModel : FriendRequestModel
    {
        
        public bool Accepted { get; set; }
    }
}
