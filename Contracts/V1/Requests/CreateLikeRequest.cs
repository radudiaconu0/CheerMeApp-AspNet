namespace CheerMeApp.Contracts.V1.Requests
{
    public class CreateLikeRequest
    {
        public string LikerId { get; set; }
        public string LikableType { get; set; }
        public string LikableId { get; set; }
    }
}