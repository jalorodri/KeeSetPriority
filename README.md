# KeeSetPriority for KeePass

This plugin allows you to set the process priority of KeePass while opening a database, saving a database, and/or while inactive. This is useful to prevent KeePass from preempting other processes while performing heavy cryptographic operations, which can affect some applications (music players, audio/video calls (eg. Zoom, Teams, Skype), games, etc.)

NOTE: This plugin requires Windows, as it uses Windows APIs for monitoring and setting the process priority. Use on other systems (Mac, Linux) at your own peril.

To install:
* Download the .plgx file from the releases panel
* Move it to KeePass' plugins directory (by default, "C:\Program Files\KeePass Password Safe 2\Plugins")
* Restart KeePass

To use:
* Go to the Tools button in the upper toolbar in KeePass
* Click on KeeSetPriority
* For the desired action(save, open, or while inactive), select the priority level desired. By default, it allows you to select from Idle to Above Normal

The most common use case would be to set opening and saving to "below normal", and inactive left at "default". That way, KeePass operates completely normal when not doing cryptographic operations, and only lowers its priority level when doing heavy computations.

DISCLAIMER: This plugin is licensed under the terms and conditions of the GPLv3 license. As such, it is released "as is" and the author claims no responsibility for any damage or injuries. You, the user, are responsible for managing and securing your data correctly.
