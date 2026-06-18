![Unity Version](https://img.shields.io/badge/Unity%20Version-2021.3.11f1_LTS-red?style=for-the-badge) ![GitHub](https://img.shields.io/github/license/almartson/Unity_TurnBasedStrategyGame_CodeMonkeyGameDevCourse?style=for-the-badge) ![Unity Version](https://img.shields.io/badge/O.S.-XUBUNTU_20.04.1_LTS-purple?style=for-the-badge)


Technical demo based on the course taught by Code Monkey, titled _Turn Based Strategy Course_.

The demo and the course were developed concurrently alongside a self-directed learning program, strengthening my intermediate and advanced expertise in Gameplay Development, C# Programming, and Unity3D.

The Unity Course focuses on building expandable systems like grid-based movement, A* pathfinding, action points, and enemy AI—taking learners from beginner concepts to professional architecture.

**Course Highlights**

- **Project Scope:** Create a tactical game similar to XCOM or Final Fantasy Tactics.
- **Core Systems:** Master grid movement, turn management, action points, and enemy AI.
- **Clean Code Focus:** Learn proper code architecture so the codebase can be expanded for other strategy games.
- **Hex Grid Update:** Includes a free expansion to convert the square grid system to a hex grid.
    
The repository also contains a branch for a Hexagonal Grid Expansion (replacing all square grid cells with hexagons) and a multi-floor expansion.

---
## What else is included?

The technical demo also includes several additional features and expansions that I designed and implemented independently:

### 1. Enhanced Enemy AI System

I modified and expanded the enemy AI implementation (including classes such as `EnemyAI.cs` and related systems) to introduce more advanced tactical behaviors:

- **Configurable Aggression System:** Enemies have a customizable aggression ("aggro") value which, combined with a configurable maximum movement range per turn, determines how far they are willing to advance toward the player before engaging in combat.
    
- **Compound Tactical Actions:** Enemies can execute more sophisticated action sequences. For example, they may traverse multiple grid cells and then chain that movement into an attack action (such as those implemented in `ShootAction.cs`), allowing them to engage the player immediately after repositioning.
    
- **Extended Movement Capabilities:** Enemies are capable of traversing significantly larger portions of the battlefield than originally supported by the base implementation presented in the course, enabling more dynamic and challenging encounters.
    

### 2. Custom Visual Effects (VFX)

I created and integrated several custom visual effects into the project:

- **Dissolve / Erosion Effect:** When a character is eliminated, its mesh gradually disintegrates into particles that separate and dissipate into the air as the character collapses. The effect is driven by a procedural dissolve shader based on noise textures and additional visual processing techniques.
    
- **Teleportation + Inverted Dissolve Effect:** Character spawning is accompanied by a custom teleportation effect composed of concentric cylinders rotating in opposite directions. Their arrangement creates the illusion of a character materializing from an ethereal space. This effect is combined with an inverted version of the Dissolve effect, causing the character to be progressively reconstructed from particles as it appears on the battlefield.

---
## Related Links:

- Course on GameDev.Tv: [Unity Turn Based Strategy: Code Your Own Strategy Game | GameDev.tv](https://gamedev.tv/courses/unity-turn-based-strategy?srsltid=AfmBOoqa0wi3-Zs_csru8j9knxpewVe12ehrU3TELsvEcgo4utQ5Bb9c)


---

## Video

[![Take your Skills to the NEXT LEVEL by making a Turn-Based Strategy game! (Unity Complete Course) - YouTube](https://www.youtube.com/watch?v=QDr_pjzedv0)](https://www.youtube.com/watch?v=QDr_pjzedv0)


---

Peace.

AlMartson


********************************


MIT License

Copyright (c) 2022 AlMartson

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE
