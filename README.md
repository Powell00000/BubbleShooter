# BubbleShooter
Reimplementation of Bubble Pops! mobile game (https://youtu.be/SglS9LQOsRA) in Unity Hybrid ECS.

I am not happy with 1.0.0 version, in terms of game visuals and code tidyness and readability. Deadline came too early for proper cleanup (my bad estimation with "it's going to be quick in ECS"), refactor, even for comments. I know I can do better and in a shorter time, next time not in ECS.

Time it took for version 1.0.0: 
- whole project: 6 working days
- visuals: 1 working day
- coding: 2 working days
- debugging: 3 days

Debugging ECS without propper custom tools is pain (au chocolat). Would be quicker and less problematic with Mono approach. 

Visuals are placeholders with entry points for explosion effect, changing colors and dropping. No time left for polishes, unfortunatelly.

What is missing:
 - "perfect" cue when all bubbles are destroyed in one go
 - objects pooling
 
 What should be changed:
 - code base - there is lots of legacy code from initial approach (ex. Cells)
 - better ordering of systems and flow - the idea was to use <code>BeginInitializationBuffer</code> for adding tags, creating entities and <code>EndSimulationBuffer</code> for removing tags and destroying entities (didn't work as expected)
 - <code>SolverSystem</code> - its naive approach with "check only next step" sometimes work in players disadvantage
 - optimization, especialy <code>BubbleNodeUpdateSystem</code>
 
 What should be added:
 - COMMENTS!
 - design and testing tools
 - tests
 
 To sum it up - I would like to spend additional whole week on refactoring, cleanup and polishing. It is first time I made hyper casual game, and with ECS, so I've made couple of rookie mistakes here and there, which slowed me down. At least I gained better insight on these type of games.
