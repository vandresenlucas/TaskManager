namespace TaskManager.Domain.TaskAggregate
{
    public class Task : Entity
    {
        public Task(
            string title, 
            string description, 
            Status status,
            DateTime createdDate)
        {
            Title = title;
            Description = description;
            Status = status;
            CreatedDate = createdDate;
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public Status Status { get; set; }
    }
}
