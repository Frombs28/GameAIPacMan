Palmer Stolly: Ghost states, TargetTilesContoller, Decision Making
Stephen Frombach: Ghost States, 2nd AI
Simon Hopkins: Ghost States, Decision Making, Resetting Level

The majority of our code and our states can be found within the GhostAI.cs file. There is a public enum within GhostAI: these enums, when used with a switch, are our different states. 
Every ghost starts out in the "waiting" state, where they simply sit in their starting position. The waiting state keeps track of how long they have been waiting, based on a public 
variable which is set for each individual ghost in the inspector. After this specificed time (called releaseTime) passes, the ghost will enter the "leaving" state.