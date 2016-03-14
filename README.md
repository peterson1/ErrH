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

## Components
### REST API Server
* hosts app files and binaries
* receives log entries and consolidates them for review
* manages app permissions and user credentials
* transport security: basic authentication via HTTPS (preferred)
* currently implemented using Drupal 7 with modules configured to function as REST server
### Update Checker Background Thread
* polls the server to check for updates in any of the files
* downloads updated files and verifies integrity of download using SHA1
* replaces live (in-use) outdated files with newer versions while app is running
* once complete, notifies user to relaunch the app for all changes to take effect

## Road Map
* Compression of app files and log entries on transit.
* Context/location-aware Notification router


## Release Notes
* Web deploy, background download and install now works.
