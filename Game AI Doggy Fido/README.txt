Palmer Stolly: GameManager.cs, Dog.cs, Behaviour Tree idea
Stephen Frombach: BTHelper.cs, Dog.cs, Behaviour Tree idea
Simon Hopkins: BTHelper.cs, dogBehavior.BT.txt, Behaviour Tree idea

Our team utilized PandaBehaviourTree, the free Unity asset from the Asset Store, in order to complete this project. The project first began with the three of us generating ideas for 
the dog's behaviours. This resulted in a very basic hand-drawn behaviour tree which can be found in the same folder as this text file under the name "OriginalTree.pdf". We then focused 
on learning how to use PandaBehaviourTree. As we did, we realized that we had to make major modifications to our behaviour tree, and we did so. At the same time as we were creating our 
tree, we were also creating the Dog and GameManager scripts, which were used to keep track of and update data based on player input. This was how we ultimately decided to organize our 
code: the behaviour tree would modify certain values of Fido, as well as send messages to the player, while the GameManager used the values set by the behaviour tree to determine 
whether actions that the player is trying to take are actually valid. In other words, the behaviour tree updates the information on the dog, and updates the player on this information: 
for example, if enough time passes, the behaviour tree code will inform the player that Fido needs to use the bathroom. If the player does nothing other than pass time, and does not 
take Fido outside, then after enough time passes, the behaviour tree will tell the player that Fido has gone to the bathroom inside, while also updating Fido's information within the 
Dog.cs file to reflect that he has gone to the bathroom in a way that makes him unhappy. Meanwhile, the GameManager code is used to interpret player input and perform the correct 
actions. Using the previous example, once the behaviour tree code informed the player of Fido's need to go outside to go to the bathroom, the player could press "W", which would inform 
the GameManger that the player wishes to take Fido outside for a walk. This updates Fido's information within the Dog.cs file to reflect that he has gone to the bathroom and taken a 
walk, which he likes.

In terms of specifics, our actual behaviour tree written in the PandaBehaviourTree format can be seen clearly in the dogBehaviour.BT.txt file. The "Root" tree is the "top" of the tree, 
with our main sub-trees being "isAwake", "hungry", "bathroom", and "goAboutDay" (in that order). These sub-trees contain sequences and fallbacks, which all contain tasks that can be 
seen in the BTHelper.cs file. This file contains solely the tasks that are used in the dogBehaviour.BT.txt file. As stated previoulsy, this file modifies many values within the Dog 
class. It also relates back to the tree whether the current task has succeeded or failed. The sub-trees are organized in order of priority: first, if the dog is asleep, it can do 
nothing but wake up to a strange noise (in which case, Fido perks up for a few minutes before going back to sleep). If the dog is awake, then it first must determine if it is hungry. 
If it is hungry, it has to to eat. Otherwise, it will check to see if it needs to use the bathroom. If so, it will do that before anything else (other than eating or remaining asleep). 
After its priorities are out of the way, Fido will "go about his day". This is what we called the tree that focused on Fido playing fetch, going for walks, and wanting to be pet. If 
Fido wants none of these things, than he will just relax. It is also in this behaviour where we determine if Fido is getting sleepy, and handle that accordingly in the BTHelper.cs 
file. In addition, this part of the code also has Fido listening for strange noises while in his home and while awake; if he hears a noise, he will bark at it and get an energy boost. 
Our final main piece of code is the GameManager.cs file. This file contains all of the player input handling, and uses the information of the dog's current state, provided by the 
dog's behaviour tree, to determine the output of the code, sometimes modifying some of the dog's values and sending the player a response. Finally, one more important aspect of our 
behaviour tree is the tick rate. Some behaviour trees do a "tick", or run-through of the tree, once every frame. However, we found we had better control when we had the tree tick only 
when in-game time had progressed. In other words, the tree would not tick or update any information until the player hit a button. This gave us better control over the dog's state and 
also resolved some issues we had with syncing up the constanty updating Unity time with the time that the dog was running on.

We encourage you to click on the "BehaviourTreeManager" in the inspector while playing the scene. This will allow you to see the behaviour tree play out as you press keys and progress 
time/take actions so that you may see exactly how our code is running through the tree. We often did this while testing our code, and we found it helpful for visualizing our code and 
how it progresses through the tree.