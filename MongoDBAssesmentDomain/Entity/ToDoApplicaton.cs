namespace MongoDBAssesmentDomain.Entity
{
    public class ToDoApplicaton
    {
        public string? Id { get; set; }
        public string? UserId { get; set; }
        public string? Name { get; set; }
        public bool HasDone { get; set; }
    }
}
