# SimpleMessenger 
This is a very small part of my diploma work at the university. Messenger includes:
1. Application client, which includes a login and registration window and a window for sending messages to users. There are MVVM and comands have been used on the work.
2. Server, which includes two services: session level and single instance (InstanceContextMode.Single).
3. Simple host for using TCP/IP protocol.
4. DLL-library for working with a database.
In my work, I used WPF for create client windows, WCF for transfer data between the client and the service, and T-SQL queries to database.
### Usage Guide 
The project requires a database.

First, you should start the host with administrator rights. Your firewall may ask for permission.

After launching the client, the user can either log in or register. Hints and errors are provided by output via MessageBox. For example, the program will notify you about the presence of such a nickname in the database or about an incorrect password.

After logging in or registering, the window for sending messages receives your nickname and id. On the right side there will be a list of users from the database. To select a nickname from the list, double-click on it. If you receive a message from another person, an image of the letter will appear in front of his nickname in the list.

User logins and logouts are showing in the host console application.
### Build
I used Visual Studio 2019 community edition and NET Framework v4.7.2.
For working with database, I used Microsoft SQL Server 2019 and local DB.
Also in the client I used NuGet for download Microsoft.Xaml.Behaviors.Wpf which include behaviors on an event mouse double click on listbox.
### Run
The program has two parts: a console service application and a client.
Since the host has a reference to the service, and the service to the dll-library, they form a single project, which is assembled in the ConsoleHost project.
The client should be building separately.
