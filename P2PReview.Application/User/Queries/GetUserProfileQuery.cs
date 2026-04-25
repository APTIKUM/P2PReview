namespace P2PReview.Application.User.Queries
{
    public class GetUserProfileQuery
    {
        public Guid UserId { get; set; }
        public Guid? AuthUserId { get; set; }

        public GetUserProfileQuery(Guid userId, Guid? authUserId)
        {
            UserId = userId;
            AuthUserId = authUserId;
        }
    }
}
