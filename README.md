# ErrH

* C# framework for web-deployment and error-handling of exe apps.
* Intended for offsite development of live production applications.
* Facilitates remote support and unattended installation of bug fixes and patches.
* Downloads and installs updated versions of app binaries/files while app is in use.
* Automatic error-reporting on unhandled exceptions
* Notifies developer (via Growl, email, etc.)


## Goals
* Minimize user intervention in the error-reporting-patching-updating loop.
* Minimize developer response times in resolving issues/bug fixes.


## Problem Being Solved
In the context of a deployed app, when an end-user encounters a bug 
(an unhandled exception, perhaps) the developer must wait for the 
end-user to report the bug before the developer can investigate the issue.
After finding the problem, fixing it, and recompiling, the developer must...


## Road Map
* Server for receiving error reports and hosting app files - done.
* Client for sending error reports and downloading updates - done.
* Context/location-aware Notification router


## Release Notes
* Web deploy, background download and install now works.
