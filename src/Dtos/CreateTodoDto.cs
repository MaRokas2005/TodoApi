namespace TodoApi.Dtos
{
    public class CreateTodoDto
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required bool IsCompleted { get; set; }
        public required DateTime DueDate { get; set; }
    }
}
