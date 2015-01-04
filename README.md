CarpoolPlanner
==============

A web app for organizing carpooling. The app will send SMS or e-mail
notifications to each user before the carpool meeting time to confirm that
they are coming.

# How it works

The administrator sets up carpools on a recurring basis, and users can choose
to attend those carpools. Users who choose to attend a carpool recurrence will
be automatically added to the list for each instance of that recurrence.

Some time before the start of each instance (4 hours by default), each
attendee will receive a notification (via SMS or e-mail, their choice) asking
them to confirm that they are coming. Later (30 mintes before the start, by
default), each attendee will receive another notification informing them who
is coming and who is driving.

## Drivers

Each user specifies whether they are a driver, and if they are a driver, they
specify how many seats their car has. After the attendees confirm that they
are coming, the app will decide who is driving based on the required number of
seats and who has been driving the least frequently recently.

If there are not enough seats for all users who wish to attend, then the app
will only allow those who confirmed the earliest to come, and the others will
be added to a waiting list in case another driver decides to attend.

# Developers

Have a look at the [wiki](https://github.com/aj-r/CarpoolPlanner/wiki) if you want
to do development on CarpoolPlanner.
