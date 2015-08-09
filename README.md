# ErrH

* Reusable error-handling framework for C#.
* Intended for use on early releases of an app (dev/alpha)
* Automatic error-reporting on unhandled exceptions
* Notifies developer (via Growl, email, etc.)
* Auto-downloads updated versions of app binaries/files


## Goals
* Minimize user intervention in the error-reporting-patching-updating loop.
* Minimize developer response times in resolving issues/bug fixes.


## Problem Being Solved
In the context of a deployed app, when an end-user encounters a bug 
(an unhandled exception, perhaps) the developer must wait for the 
end-user to report the bug before the developer can investigate the issue.
After finding the problem, fixing it, and recompiling, the developer must...


## Road Map
* Server for receiving error reports and hosting app files
* Client for sending error reports and downloading updates
* Context/location-aware Notification router


## Release Notes
* No working workflow yet. Still gathering the needed components.