Palmer Stolly: Ghost states, TargetTilesContoller, Decision Making
Stephen Frombach: Ghost States, 2nd AI
Simon Hopkins: Ghost States, Decision Making, Resetting Level

The majority of our code and our states can be found within the GhostAI.cs file. There is a public enum within GhostAI: these enums, when used with a switch, are our different states. 
Every ghost starts out in the "waiting" state, where they simply sit in their starting position. The waiting state keeps track of how long they have been waiting, based on a public 
variable which is set for each individual ghost in the inspector. After this specificed time (called releaseTime) passes, the ghost will enter the "leaving" state. 

In this state, the ghosts move towards the middle of their starting cage. Once they reach the middle, they move towards the top of the cage, outside of the gate. Once they exit the gate and are right above it, they will enter the "active" state, with their intial direction being right. This means the ghosts will always move right after exiting the gate. Since 
the starting ghost begins on at the exit of the gate, it is already at the destination required to enter the active state, meaning it will effectively begin in the active state 
(it will only take a frame or 2 to realize it needs to switch out of the waiting and leaving states). 

The active state is where the ghost makes all of its decisions for chasing Pac-Man. The important part about the active state is that the ghost is assigned a target to move towards, 
and then move towards this target's position using the "directionToTurn" function. This function, also in GhostAI.cs, first determines which ways are available for it to move using 
the given checkDirectionClear function in the Movement.cs script. Once the ghost knows all of the potential directions it can move, it determines which of its available directions 
it can move by determining which movement will bring it physically closer to the target. It then moves in that direction, using the given move functions in Movement.cs. It will only make this decision every couple of frames so the ghost does not constantly get stuck in one place, bouncing back and forth in one spot; hence the "movementConfirm" value. The ghost 
will move towards whatever target it is given in a way that makes sense. So, the only other important part of this code is determining what the actual target is. To determine this, 
the active state consists of two "sub-states": dead and alive. The dead sub-state occurs when Pac-Man eats a ghost after eating a power pellet, while the alive sub-state occurs after 
a dead ghost reaches the starting gate, re-entering the active state in the alive sub-state. While in the dead sub-state, the ghost's target is the gate, meaning it will move towards 
the gate. It's speed will also be increased when dead. 
In the alive sub-state, there exist another two sub-states: fleeing and not fleeing. Fleeing occurs when Pac-Man eats a power pellet while the ghost is in the active state. In the 
fleeing sub-state, the ghost does not move towards a target; instead, it chooses a random direction to move in out of the potential directions in can move in at each intersection. 
When the ghost is in the fleeing state for a certain amount of time and is not eaten, it will enter not fleeing mode. If it is eaten while in fleeing mode, then it enters the dead 
sub-state, which was described previously. However, when the ghost is not fleeing, the target is determined by the XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX. These target positions are 
kept track of and updated in TargetTilesController.cs. In here is the only place where the ghost's AIs differ: so the GhostAI.cs file can be applied to any ghost, without needing to 
specify which ghost it is being applied to. The targets for our "original" Pac-Man ghosts function as follows:

Blinky: the target is Pac-Man's current position.
Pinky: the target is two "tiles" (where 1 tile = 1.5 Unity units) in front of Pac-Man's position.
Inky:
Clyde: if Clyde is within 8 units of Pac-Man, Clyde's target is the bottom-left corner of the map. If Clyde is not within 8 units of Pac-Man, then Clyde's target is Pac-Man's current 
position.

The targets for our own version of the Pac-Man ghosts function as follows:

Blinky: 
Pinky: 
Inky: 
Clyde: 

We also fixed some small things to make the game feel more like Pac-Man. The biggest one is that, after beating the level, the player can simply press "R" and restart the game, 
allowing them to move on to the next level.

The diagram for our ghost FSM can be found in a document called "Ghost FSM.png" in the same folder as this text document. The states are shown in individual circles, while the 
state transitions are shown using arrows with labels on them that describe what triggers the transition. The sub-states for a given state are placed in a dashed box which the 
"super-state" points to with a dashed arrow.