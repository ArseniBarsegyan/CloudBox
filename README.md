# CloudBox
Simple service for storing files on server (WCF).
User can register in system, create/delete folders, upload files to server, download, delete them.
WPF client additionally listen to events in folder where user store files on his PC and when files change
communicate with WCF service and synchronize them.

This project contains WCF service, Console based service host and it's clients - MVC4 and WPF applications.
SimpleMembership authorize system. MySQL database.

How to use:
1) Run service host
2) Run any other client project
