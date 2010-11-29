Release 1.0 Alpha

Known Bugs
--------------
Currently when a Dynamics Nav instance becomes attached to a Client instance, it will refuse to restore the window (until the Client instance is disposed of) if it is minimized.
This bug only happens when the client events are subscribed to, and I suspect it is related to the OnActiveChanged event.