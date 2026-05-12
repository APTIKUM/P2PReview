namespace P2PReview.Domain.Entities
{
    public class ReviewRequestFile
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Content { get; set; }
        public string ReviewRequestId { get; set; }
        public ReviewRequest ReviewRequest { get; set; }
    }
}
