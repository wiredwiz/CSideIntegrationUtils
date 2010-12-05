Release 1.0 Alpha

Known Bugs
--------------
Currently when a Dynamics Nav instance becomes attached to a Client instance, it will refuse to restore the window (until the Client instance is disposed of) if it is minimized.
This bug only happens when the client events are subscribed to, and I suspect it is related to the OnActiveChanged event.

CHANGE LOG

Version 1.1 Alpha
----------------
Added IsBusy property to Client class
Added IsRunning property to Client class
Added Locking to the Client methods
Added GetSpecificDesigner to Client class
Added support for lazy loading to the Record class (this means all field values are now loaded as needed in real time)
Added Server Type as a parameter to GetSpecificClient method
Added static CleanUp method to Client class
Added Commit method to Client class
Added EndTransaction to Client class
Changed FieldValue struct to a Class
Removed Refresh method from Record Class since lazy loading makes it pointless
Made minor changes to various methods and variables
Fixed bug where Client.Dispose() would not dispose properly
Fixed bug where Error method on the Client would cause an exception to be immediately thrown
