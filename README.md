# CyberSecurityChatbot2

## Project Overview Final POE

CyberSecurityChatbot2 is a Windows Presentation Foundation (WPF) desktop application developed in C# using the .NET Framework. The project was created to promote cybersecurity awareness by educating users about common online threats and safe digital practices. The chatbot combines intelligent conversation with interactive features, providing users with both educational content and practical cybersecurity tools.


## Features

* Modern Graphical User Interface (GUI)
* Natural Language Processing (NLP) for recognising cybersecurity-related questions
* Memory recall to remember the user's name
* Sentiment detection for personalised responses
* Cybersecurity Task Assistant with MySQL database integration
* Add, view, complete, and delete cybersecurity tasks
* Optional reminders with date and time for tasks
* Educational Cybersecurity Quiz (10 multiple-choice questions)
* Quiz score tracking and final results
* Activity Log to record user interactions
* Two-Factor Authentication (2FA) awareness and simulation
* Text-to-Speech support
* Multiple response variations for more natural conversations


## Cybersecurity Topics Covered

The chatbot provides information on:
* Phishing
* Malware
* Password Safety
* Privacy
* Safe Browsing
* Online Scams
* Hackers
* Cyberbullying
* Computer Viruses
* Two-Factor Authentication (2FA)


## Technologies Used

* C#
* .NET Framework
* Windows Presentation Foundation (WPF)
* XAML
* MySQL & MySQL Workbench 8.0
* MySql.Data Connector
* Visual Studio 2026
* Object-Oriented Programming (OOP)


## Database

The application uses a MySQL database named **CyberSecurityChatbot2**. User tasks are stored in a **Tasks** table containing:
* Task ID
* Title
* Description
* Reminder Date and Time
* Completion Status

This allows users to manage their cybersecurity tasks directly from the chatbot.


## Project Structure

* **Assets** – Images and chatbot logo
* **Models** – User, TaskItem, and QuizQuestion classes
* **Services** – Chatbot, Sentiment, Database, and Quiz services
* **MainWindow.xaml** – User interface
* **MainWindow.xaml.cs** – Application logic
* **App.xaml** – Application configuration


## How to Run

1. Open the solution in Visual Studio 2026.
2. Ensure the MySQL server is running.
3. Create the **CyberSecurityChatbot2** database and **Tasks** table.
4. Update the database connection string if necessary.
5. Build and run the application (Ctrl + F5).


## Learning Outcomes

This project demonstrates:

* Object-Oriented Programming (OOP)
* GUI Development
* MySQL Database Integration
* CRUD Operations
* Natural Language Processing
* Memory Recall
* Sentiment Analysis
* Activity Logging
* Interactive Quiz Development


## Author

Name:	Reabetsoe Modiri
Module:	Programming
Project:CyberSecurityChatbot2 – Part 3 Final POE
