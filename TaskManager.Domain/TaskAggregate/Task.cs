namespace TaskManager.Domain.TaskAggregate
{
    public class Task : Entity
    {
        public Task(
            string title, 
            string description, 
            Status status,
            Guid createdByUserId,
            DateTime createdDate,
            DateTime? updatedDate = null)
        {
            Title = title;
            Description = description;
            Status = status;
            CreatedByUserId = createdByUserId;
            CreatedDate = createdDate;
            UpdatedDate = updatedDate;
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public Status Status { get; set; }
        public Guid CreatedByUserId { get; set; }
    }
}
