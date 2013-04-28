Release 1.4 Alpha

To use the C/Side Integration Utilities you simply need to reference the library from your .Net project.  Once that is done you will need to get a Client instance that is bound 
to the Navision client window you specify. There are a couple of ways to do this, the easiest being to call the Client.GetClient() method and specify the client you wish to retrieve 
by its server, database, and company.  Once you have a client instance you can do all kinds of interesting things.  Don't forget to dispose of your client instance when you are 
done with it, it is good practice to clean up after yourself (or enclose the whole block of code in a using statement if you can and the .Net framework will do it for you).

Known Bugs
--------------
Currently when a Dynamics Nav instance becomes attached to a Client instance, it will refuse to restore the window (until the Client instance is disposed of) if it is minimized.
This bug only happens when the client events are subscribed to, and I suspect it is related to the OnActiveChanged event.


CHANGE LOG

Version 1.1 Alpha
-----------------
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

Version 1.2 Alpha
-----------------
Created constants for error codes
Added parameter to constructor to control whether the client instance would bind to client events
Fixed bug in client ReadObjectToStream() method
Added FetchSpecificObject() method to client
Corrected formatting for Time values in record data
Added Locked and LockedBy fields to Object class

Version 1.3 Alpha
-----------------
Added ClientLink class

Version 1.4 Alpha
-----------------
Renamed Client.FetchSpecificObjects methods to GetObjects and GetObject
Refactored the code for GetObject and GetObjects
Added missing documentation
Renamed Client.FetchTable to GetTable
Renamed old Client.GetTable method to GetTableInternal to avoid conflict
Made all ClientLink.Parse methods static
Made all ClientLink private parsing helper methods static
Renamed Client.GetSpecificClient methods to GetClient
Renamed Client.GetSpecificDesigner to GetDesigner

Version 1.4.2 Alpha
-------------------
Added back in the event hooking logic
	It had been temporarily disconnected for testing and had mistakenly been left disconnected
	The known bug with event hooks still exists.
Changed library to use the .Net 4 Client Profile framework
Added Query object type to the NavObjectType enumeration