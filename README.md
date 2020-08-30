# Host Europe Dns Updater
Checks for new public IP and updates DNS entries for your domain at Host Europe

* Stores last IP in local txt file
* Only send update mail when current IP is other then last ip or no last ip is stored
* Configuration by appsettings.json. (Example checked in)
* Uses external webpage to get public IP like http://ipv4.icanhazip.com
