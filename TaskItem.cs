namespace CyberSecurityChatbot2.Models
{
    public class TaskItem
    {
        public int TaskId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public System.DateTime? ReminderDate { get; set; }

        public bool IsCompleted { get; set; }
    }
}
