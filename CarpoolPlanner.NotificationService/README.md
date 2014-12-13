# CarpoolPlanner.NotificationService

This compiles to an executable that is meant to run on the same server as the CapoolPlanner web app. 
Its job is to send notififications to attendees at the appropriate times. 

If users make changes to a carpool or change their status via the web app, then the web app will notify
the NotificationService by sending an HTTP request.

Note: currently my server seems unable to communicate with textnow.com, so I am running the
NotificationService from my PC. As such, it detects changes by periodically checking the database
instead of through HTTP requests.
