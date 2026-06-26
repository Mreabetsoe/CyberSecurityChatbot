using System;
using System.Collections.Generic;
using System.Windows;
using CyberSecurityChatbot2.Models;
using CyberSecurityChatbot2.Services;

namespace CyberSecurityChatbot2
{
    public partial class MainWindow : Window
    {
        private ChatbotService chatbot;
        private SentimentService sentiment;
        private DatabaseService database;
        private QuizService quizService;

        private Dictionary<string, string> memory;

        private List<string> activityLog;

        private bool quizMode = false;
        private int currentQuestion = 0;
        private int score = 0;

        private bool addingTask = false;
        private bool waitingForReminder = false;

        private string taskTitle = "";
        private string taskDescription = "";

        private DateTime? taskReminder = null;

        public MainWindow()
        {
            InitializeComponent();

            chatbot = new ChatbotService();
            sentiment = new SentimentService();
            database = new DatabaseService();
            quizService = new QuizService();

            memory = new Dictionary<string, string>();
            activityLog = new List<string>();

            txtChat.Text +=
                "Bot: Hello! Welcome to the Cybersecurity Awareness Chatbot.\n\n" +
                "What is your name?\n\n" +
                "";
        }

        private void LogActivity(string activity)
        {
            activityLog.Add(
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                + " - "
                + activity);
        }

        private void DisplayQuestion()
        {
            QuizQuestion question =
                quizService.Questions[currentQuestion];

            txtChat.Text +=
                "Bot: Question " +
                (currentQuestion + 1) +
                "/10\n\n" +

                question.Question + "\n\n" +

                "A. " + question.OptionA + "\n" +
                "B. " + question.OptionB + "\n" +
                "C. " + question.OptionC + "\n" +
                "D. " + question.OptionD + "\n\n";
        }

        private void HandleQuiz(string answer)
        {
            QuizQuestion question =
                quizService.Questions[currentQuestion];

            if (answer.ToLower() ==
                question.CorrectAnswer.ToLower())
            {
                score++;

                txtChat.Text +=
                    "Bot: Correct!\n\n";
            }
            else
            {
                txtChat.Text +=
                    "Bot: Incorrect!\n\n";
            }

            currentQuestion++;

            if (currentQuestion >=
                quizService.Questions.Count)
            {
                quizMode = false;

                txtChat.Text +=
                    "Bot: Quiz Finished!\n" +
                    "Final Score: " +
                    score +
                    "/10\n\n";

                LogActivity(
                    "Quiz completed. Score: "
                    + score +
                    "/10");

                return;
            }

            DisplayQuestion();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMessage.Text))
                return;

            string userMessage =
                txtMessage.Text.Trim();

            string message =
                userMessage.ToLower();

            txtChat.Text +=
                "You: " +
                userMessage +
                "\n";

            string response = "";

            if (quizMode)
            {
                HandleQuiz(message);

                txtMessage.Clear();
                txtChat.ScrollToEnd();

                return;
            }

            // NAME MEMORY
            if (message.Contains("my name is"))
            {
                string name =
                    userMessage
                    .Replace("my name is", "")
                    .Trim();

                memory["name"] = name;

                response =
                    "Nice to meet you " +
                    name +
                    "! I will remember your name.";

                LogActivity(
                    "Stored name: " +
                    name);
            }

            // RECALL NAME
            else if (message.Contains("what is my name"))
            {
                if (memory.ContainsKey("name"))
                {
                    response =
                        "Your name is " +
                        memory["name"];
                }
                else
                {
                    response =
                        "You have not told me your name yet.";
                }
            }

            // GREETINGS
            else if (message == "hi" ||
                     message == "hello" ||
                     message == "hey")
            {
                string name =
                    memory.ContainsKey("name")
                    ? memory["name"]
                    : "friend";

                response =
                    "Hello " +
                    name +
                    "! How can I help you stay safe online?";
            }

            // ADVICE
            else if (message.Contains("advice"))
            {
                string name =
                    memory.ContainsKey("name")
                    ? memory["name"]
                    : "friend";

                response =
                    name +
                    ", remember to use strong passwords and avoid suspicious links.";
            }

            // TASK ASSISTANT
            else if (message == "task assistant")
            {
                response =
                    "I am your Cybersecurity Task Assistant.\n\n" +
                    "Commands:\n" +
                    "- add task\n" +
                    "- show tasks\n" +
                    "- complete task [id]\n" +
                    "- delete task [id]\n" +
                    "- activity log";
            }

            // START QUIZ
            else if (message == "start quiz")
            {
                quizMode = true;

                currentQuestion = 0;
                score = 0;

                LogActivity(
                    "Started cybersecurity quiz");

                txtChat.Text +=
                    "Bot: Cybersecurity Quiz Started!\n\n";

                DisplayQuestion();

                txtMessage.Clear();
                txtChat.ScrollToEnd();

                return;
            }

            // ADD TASK
            else if (message == "add task")
            {
                addingTask = true;

                taskTitle = "";
                taskDescription = "";

                response =
                    "Please enter the task title.";
            }

            // TASK TITLE
            else if (addingTask &&
                     taskTitle == "")
            {
                taskTitle =
                    userMessage;

                response =
                    "Please enter the task description.";
            }

            // TASK DESCRIPTION
            else if (addingTask &&
                     taskDescription == "")
            {
                taskDescription =
                    userMessage;

                waitingForReminder = true;

                response =
                    "Please enter reminder date and time (yyyy-MM-dd HH:mm) or type skip.";
            }

            // REMINDER
            else if (waitingForReminder)
            {
                if (message != "skip")
                {
                    DateTime reminder;

                    if (DateTime.TryParse(
                        userMessage,
                        out reminder))
                    {
                        taskReminder =
                            reminder;
                    }
                }

                TaskItem task =
                    new TaskItem
                    {
                        Title =
                            taskTitle,

                        Description =
                            taskDescription,

                        ReminderDate =
                            taskReminder
                    };

                database.AddTask(task);

                response =
                    "Task saved successfully.";

                LogActivity(
                    "Added task: "
                    + taskTitle);

                addingTask = false;
                waitingForReminder = false;

                taskTitle = "";
                taskDescription = "";
                taskReminder = null;
            }

            // SHOW TASKS
            else if (message == "show tasks")
            {
                List<TaskItem> tasks =
                    database.GetTasks();

                response = "";

                foreach (TaskItem task
                         in tasks)
                {
                    response +=
                        "Task ID: " +
                        task.TaskId +

                        "Title: " +
                        task.Title +

                        "Description: " +
                        task.Description +

                        "Status: " +
                        (task.IsCompleted
                            ? "Completed"
                            : "Pending");

                    if (task.ReminderDate != null)
                    {
                        response +=
                            "\nReminder: " +
                            task.ReminderDate.Value
                            .ToString("yyyy-MM-dd HH:mm");
                    }

                    response +=
                        "\n------------------\n";
                }

                if (response == "")
                {
                    response =
                        "No tasks found.";
                }
            }

            // COMPLETE TASK
            else if (message.StartsWith("complete task"))
            {
                int id;

                if (int.TryParse(
                    message.Replace(
                        "complete task",
                        "").Trim(),
                    out id))
                {
                    database.CompleteTask(id);

                    response =
                        "Task marked completed.";

                    LogActivity(
                        "Completed task " +
                        id);
                }
            }

            // DELETE TASK
            else if (message.StartsWith("delete task"))
            {
                int id;

                if (int.TryParse(
                    message.Replace(
                        "delete task",
                        "").Trim(),
                    out id))
                {
                    database.DeleteTask(id);

                    response =
                        "Task deleted.";

                    LogActivity(
                        "Deleted task " +
                        id);
                }
            }

            // ACTIVITY LOG
            else if (message == "activity log")
            {
                response = "";

                foreach (string item
                         in activityLog)
                {
                    response +=
                        item +
                        "\n";
                }

                if (response == "")
                {
                    response =
                        "No activities recorded.";
                }
            }

            // SENTIMENT ANALYSIS
            else
            {
                string mood =
                    sentiment.DetectSentiment(message);

                if (mood == "happy")
                {
                    response =
                        "I'm glad you're happy today!";
                }
                else if (mood == "sad")
                {
                    response =
                        "I'm sorry you're feeling sad.";
                }
                else if (mood == "stressed")
                {
                    response =
                        "You sound stressed. Take a break.";
                }
                else
                {
                    response =
                        chatbot.GetResponse(message);
                }
            }

            txtChat.Text +=
                "Bot: " +
                response +
                "\n\n";

            txtMessage.Clear();
            txtChat.ScrollToEnd();
        }
    }
}