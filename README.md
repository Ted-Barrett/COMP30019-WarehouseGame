[![Open in Visual Studio Code](https://classroom.github.com/assets/open-in-vscode-f059dc9a6f8d3a56e377f745f24479a46679e63a5d9fe6f495e02850cd0d8118.svg)](https://classroom.github.com/online_ide?assignment_repo_id=448274&assignment_repo_type=GroupAssignmentRepo)

**The University of Melbourne**  
**COMP30019 – Graphics and Interaction**

# Project 2 "Warehouse" Report

## Table of contents

- [Team Members](#team-members)
- [Technologies](#technologies)
- [Explanation of the game](#explanation-of-the-game)
- [Player Guide](#player-guide)
- [Object and Entity Design](#object-and-entity-design)
- [Graphics Pipeline and Camera](#graphics-pipeline-and-camera-motion)
- [Procedural Generation](#procedural-generation)
- [Custom Shaders](#custom-shaders)
  - [Shader 1](#shader-1)
  - [Shader 2](#shader-2)
- [Particle System](#particle-system)
- [Evaluation](#evaluation)
  - [Querying Method](#querying-method)
  - [Observational Method](#observational-method)
  - [Post-Feedback Improvements](#post-feedback-improvements)
- [References](#references)
- [Group Member Contributions](#group-member-contributions)


## Team Members

| Name        |             Task              | State |
| :---------- | :---------------------------: | ----: |
| Ben Cook    |      Drones and Gameplay      |  Done |
| Max Plumley | Animations and Game Mechanics |  Done |
| Ted Barrett |   Level Design and Vehicles   |  Done |
| Luke Russo  |     Graphics and Shaders      |  Done |

## Technologies

Project is created with:

- Unity 2021.1.13f1
- Blender 2.8
- Audacity

## Explanation of the game

Warehouse is an Overcooked-style game set in an Amazon-inspired warehouse. Players must deliver coloured packages to the correctly coloured lorry to earn as many points as possible within a set time limit. Packages are supplied to the warehouse floor by a swarm of airborne drones. The player has two strategic options for delivering packages: packages can either be carried one by one to their respective lorries or loaded onto an appropriately colored hand truck, which has a larger carrying capacity, for a more efficient round trip. In later levels the player must also be wary of warehouse hazards such as rogue forklift convoys and speeding delivery lorries which will knock the player over, wasting precious time on the clock and potentially damaging any packages they might be delivering. The player can also strategically throw boxes around the warehouse for maximum efficiency.

## Player Guide

### Controls

The controls are fairly simple, and the game targets both users playing via a keyboard or console controller.

Note that menus must be navigated using the mouse.

#### Keyboard

| Action         |       Button       |
| :------------- | :----------------: |
| Movement       | WASD or Arrow Keys |
| Pick Up / Drop |      Spacebar      |
| Throw          |         F          |

#### Controller

| Action         |       Button        |
| :------------- | :-----------------: |
| Movement       | Left Analogue Stick |
| Pick Up / Drop |          A          |
| Throw          |          X          |

### Mechanics

- The objective is to deliver as many packages as possible within the time limit
- Packages are coloured and can only be delivered to the same coloured lorry (i.e.: red packages to the red lorry)
- Packages are worth 100 points each; the total score for the current game is displayed in the lower left corner
- The time remaining for the current level is displayed in the lower right corner
- The player can be hit by moving hazards such as forklifts. Getting knocked over by a hazard will waste time on the clock and also destroy any carried packages including those in a held hand truck
- The player is able to carry one package at a time or pick up a hand truck to carry up to three packages at a time
- The hand truck is auto loading, and can only load packages of the same colour (i.e.: the red hand truck only loads red packages)
- Held packages are automatically loaded into waiting lorries when the player enters the loading bay area in front of each lorry

## Object and Entity Design

### Scenes

The game was implemented across a number of scenes. It was decided that each level would be implemented in its own scene, and the main menu is also implemented as a stand alone scene. While it would be possible to implement most levels in a single scene - the game is not particularly large - this approach was not adopted as the alternative allowed for team members to implement individual scenes in parallel without as many merge conflicts to resolve.

### Entity Component System

The team made full use of Unity's built in Entity Component System to implement a modular system for Warehouse with low coupling and high cohesion. Game functionality and behaviors were implemented via small focussed classes that could be easily attached to appropriate game objects.

Where beneficial the team coded to interfaces to hide implementation details from client code while also taking advantage of dynamic dispatch and polymorphism to transparently and scalability introduce variation within the game. One notable instance of this is the <code>IBoxContainer</code> interface and its two implementations <code>HandTruck</code> and <code>GrabScript</code> classes. The team wanted Warehouse to provided a number of ways for players to deliver packages, part of the interest of the game was to optimise the use of these delivery strategies, by coding each strategy to an interface dependent classes such as the <code>BoxCollector</code> class could be decoupled from individual strategies, simplifying this client code greatly but also simplifying the process of extending game behaviors with new strategies.

### Prefabs

A fair few prefabs were created so that game components could be reused between levels and modifications to existing components could be simultaneously applied to all scenes in the game.

Notable prefabs were implemented for the player, drones, boxes, hand trucks, various particle systems, hazard spawners and paths, and also the game HUD.

## Graphics Pipeline and Camera Motion

For this project we used the built-in render pipeline, with most objects simply using the Low Poly Logistics material (from the low poly logistics asset pack - see references) with the standard unity shader on them. In terms of the draw order of the various objects, the large majority are simply drawn at <code>Geometry</code> (2000), <code>Mask.shader</code> is drawn at <code>Geometry - 10</code> (1990), and the transparent shader <code>Box Outline.shader</code> is drawn at <code>Transparent</code> (3000). This is set up so that transparent shaders are drawn after solid ones (to ensure they don't get drawn over when they shouldn't) and so that <code>Mask.shader</code> is drawn before <code>Box Outline.shader</code> (because they are using the stencil buffer in a way that the mask must come first - see custom shader #2 for more details).

Each level in warehouse uses only one camera which is mainly static and looks down on the entire level at an incline. This choice suits the genre and gameplay, which involves small scale and simple level designs. The only movement in the camera is a shaking effect which triggers when the player gets hit by a truck or forklift. This makes the experience more immersive and the added visual feedback reinforces that they did something that is not conducive to achieving the game objective.

## Procedural Generation

<code>bullet-hell/Assets/Scripts/ScattererStuff/Scatterer.cs</code>

<code>Scatterer</code> is a script written to distribute prefabs over a plane, ensuring that they do not intersect.

<code>Scatterer</code> takes two lists as inputs: <code>sourceObjects</code> (containing prefabs) and <code>weights</code> (containing integers). For a given index, the weight describes the frequency at which to spawn the prefab in <code>sourceObjects</code>. For example, if <code>sourceObjects</code> is <code>{box,pallet}</code> and <code>weights</code> is <code>{1,2}</code> then the pallet will spawn twice as frequently as the box.

<code>Scatterer</code> works by randomly distributing the prefabs in <code>sourceObjects</code> over <code>spawnPlane</code>, and then removing any prefabs which intersect with others. In order to maintain list order while "removing" intersecting prefabs, each prefab is given a component <code>Scatterable</code>, which keeps track of if the prefab will be removed after all the other prefabs have been processed. Once all prefabs have been checked for collisions, all prefabs marked for removal will be destroyed, in reverse list order as to avoid index errors.

<code>Scatterer</code> also has the option to run without collision checks, for example if distributing grass prefabs. This however is not used in our game.

<img src="report images/trucks.png">

<code>Scatterer</code> being used to distribute trees, as well as the boxes in the back of the vans.

## Custom Shaders

The custom shaders to mark are <code>Interactable Effect.shader</code> and <code>Box Outline.shader</code> both found in <code>bullet-hell/Assets/Shaders</code>.

### Shader 1

<p align = "left">
  <img src="report images/handTruck.gif"  width="200" >
</p>

<code>bullet-hell/Assets/Shaders/Interactable Effect.shader</code>

This shader is used on the hand trucks to show that the player is able to interact with them. Additive blending is used to create a bright and noticeable effect.

The vertex shader converts the vertex position to both clip space and view space and passes it to the fragment shader.

The fragment shader samples the main texture using the view pos of the pixel. This creates a continuous effect over the screen rather than using the hand truck UVs which make the texture render discontinuously. View position is used for sampling instead of clip position to ensure a consistent look over different devices, even though using clip position probably would have been fine. <code>\_MainTex_ST</code> accounts for the tiling and offset of the texture while the <code>+ \_Speed \*\_Time.y</code> pans the texture over the hand trucks. The red channel from the texture is then used to interpolate between the base colour and the shimmer colour, while a time varying sin value between 0 and 1 is multiplied by the colour to make the effect flash. This uses <code>\_FlashFreq</code> from the editor as the frequency.

The final colour is then returned, producing a bright, transparent, flashing and moving effect which renders on top of the hand trucks.

### Shader 2

<p align = "left">
  <img src="report images/boxOutline.gif"  width="300" >
</p>

<code>bullet-hell/Assets/Shaders/Box Outline.shader</code>

This shader is used to create a pulsating, transparent box outline. It is not directly on the different coloured boxes but is actually on the box outline game object, which is a child of the box prefabs. The box outline mesh consists only of the vertices representing the 8 corners (no repeated vertices) of the box and instead of being defined in terms of triangles, the mesh topology is set to lines - i.e. the mesh is defined in terms of 12 box edges (no repeated edges). The vertex normals are the average of the normals to the faces of the box that the vertex is a part of (i.e. smooth normals). Finally, edges that should not have an outline are removed from the mesh by a script called <code>BoxDoubleOutlineCulling.cs</code> which is also on the box outline game object.

The main functionality of the shader is in a <code>CGINCLUDE</code> which is then used for two separate passes. The vertex function converts the vertex position and vertex normals to view space for later manipulation in the geometry function.

The geometry function takes an edge as an input and outputs triangles - which make up the transparent outline corresponding to that edge of the box - to a triangle stream.

Sin is used to make a time dependent sin value between 0 and 1 which is used to interpolate between the two outer outline colours and the two outline thicknesses to create the pulsating effect. Inner and outer corners for each point in the input line are calculated by displacing the point along the vertex normal. The outer corners are displaced by the outline thickness (time varying) and the inner corners are displaced by outline thickness - corner radius. A rounding direction for each corner is the vector projection of the difference between the inner and outer corners onto the input line (adjusted for the correct direction). This points from the outer corner to the final edge of the triangles that make up the round corners.

<code>\_CornerResolution</code> triangles per input vertex are then appended to the stream (as sets of vertices) in the for loop to make the rounded corners. Each new vertex calculated (<code>roundingPos</code>) as part of the rounded corners is corner radius distance away from the inner corner.

<p align = "center">
  <img src="report images/roundedCornerCloseup.JPG" width="500" >
</p>

The diagram above shows only the triangles that make up the round portion of the box outline. <code>\_CornerResolution</code> is set to 2 and the corner radius is set to the maximum. In this case the for loop would draw 4 triangles, two of which are outlined and the other two are off the screen to the right at the other side of the box edge. The vectors from the inner corner to the new corners are calculated by travelling from the inner corner to the outer corner, then a portion of the way along the rounding direction vector. In this case the triangles will split the rounding direction vector in half as <code>\_CornerResolution</code> is 2. If <code>\_CornerResolution</code> was 3, they would split it into thirds. The distance from the inner corner to the all corners of the triangles is always corner radius to achieve that rounded effect. Since the corner radius is set to the maximum, the inner corner is on the box, if the corner radius was lower it would be closer to the outer corner and the outline corner would be sharper.

<code>RestartStrip()</code> is used whenever the next vertex appended to the stream is part of a new triangle. The final two triangles are then appended to the stream to make up the non-rounded bit of the outline. For all triangles, the vertices output are assigned either the inner or outer colour and the view position is converted into clip position.

The fragment function simply outputs the colour of the pixel, which has been interpolated from the output of the geometry function.

A few tags are used at the start of the subshader, mainly to set up the transparency. The most important is the render queue tag which dictates the draw order. Both passes use the standard transparency blend mode, do not write to the depth buffer (standard for transparency) and have culling off. The first pass is drawn regularly (using the functions in the <code>CGINCLUDE</code>) when the box outline is the front most object, the depth testing is set by <code>ZTest LEqual</code>. The second pass is drawn when the box outline is not the front most object, the depth testing is set by <code>ZTest GEqual</code>. In this second pass, it checks the stencil buffer and only draws the outline if the value in the buffer is not 2. This is used in combination with <code>Mask.shader</code>, which always writes a value of 2 to the stencil buffer and has an earlier draw order, to make sure that the outline renders over objects without the mask (regardless of the depth), but renders normally when in front or behind objects with the mask. This provides the effect where the outline always displays clearly in front of trucks and forklifts - so it is easily visible - and normally in relation to the player, drones and other boxes - where it looks weird if it always renders in front. See the examples below:

<p align = "left">
  <img src="report images/OutlineInFrontOfPlayer.JPG" width="200" />
  <img src="report images/OutlineBehindPlayer.JPG" width="200" />
  <img src="report images/OutlineBehindTruck.JPG" width="500" />
</p>

## Particle System

<code>bullet-hell/Assets/Prefabs/boxDeliveryEffects</code>

The prefabs in this folder are all particle effects, and are used when a box is delivered successfully. <code>collectRed</code>,<code>collectGreen</code> and <code>collectBlue</code> are designed to look like fireworks, and are a single spherical burst of sparkle particles which occurs when instantiated.

The <code>collectTick</code> particle effect is used as a child of the other particle effects, and creates a single tick particle. The tick is coloured to be the same as the sparkles, using an additive colour mode on the tick material.

<img src="report images/blue_particle.gif"  width="500">

## Evaluation

Two evaluation techniques were used to identify usability improvements for Warehouse. Each is described in detail below.

Raw data is located in [evaluationRawData](./evaluationRawData)</code>

### Participants

The team selected 10 unique participants for querying and observational evaluation. All 10 participants participated in both the interview and questionnaire. Participants were primarily categorized according to their familiarity with games, three categories we identified by the team, Non-gamer, Casual gamer, and Gamer. The ratio of Non-gamer to game proficient categories (Casual gamer, and Gamer) was roughly one to one, which allowed the study to see how people unfamiliar with common video game design patterns would perceive and interact with the interface. In addition to the categories described prior a few other significant participant attributes are worth noting: the participants were of a wide range of ages, and one color blind participant was also the target of evaluation.

### Querying Method

We used a google form to gather anonymous feedback from the 10 participants after they had played the game. The form utilised the [system usability scale (SUS)](https://en.wikipedia.org/wiki/System_usability_scale), in combination with some questions more specifically tailored to target aspects of the game that the team felt might be problematic.

#### Feedback Gathered

The mean SUS score was 76, which is considered above average. Largely, we tried to keep the control scheme and interface as minimal as possible without detracting from gameplay, so this likely contributed to the high usability.

Some specific questions are below.

- **Was the level timer a good length?**

```
Too short
1 |#####
2 |###
3 |##
4 |
5 |
Too long
```

The vast majority of participants felt that the length of each level (60 seconds at the time) was too short.

- **I felt very confident playing the game.**

40% of players surveyed gave a less than neutral response to this question indicating that it was not especially clear 'what' and 'how' they were expected to perform required tasks within the game.

- **Do you have any other comments or suggestions?**

One respondent said they would "like to have been able to have a second go at levels to improve my score".

- **I found the game cohesive as a whole.**

All players responded with either a 4 or a 5 in this category.

### Observational Method

Cooperative Evaluation was used as the primary Observational Method when evaluating Warehouse. Participant interviews began on the Warehouse main screen and participants were asked play each level of Warehouse to complete the following tasks:

- collect a package
- drop off a package in the correct lorry
- avoid hazards such as forklifts and delivering lorries
- use a hand cart to collect and deliver multiple packages
- navigate the inter-level menu
- identify their current score and time left on the game clock

Observational sessions were conducted either in person or remotely. The game controls were explained to each participant at the start of the interview. Each interview was recorded for analysis. Interviewers prompted participants that appeared to be stuck or encountered issues, but only as a last resort as to allow participants a chance to work out solution from in game cues. A period of time post-task was dedicated to discussing any further observations the participant had while playing the game that they had not previously mentioned, it was common for Non-gamer participants to feel overwhelmed attempting to play the game while also providing feedback.

#### Feedback Gathered

Many players were confused in the first level, due to the size of the collection zones. This was a bug, as the collector hitbox was attached to the wrong object. We resolved this to make the collector size more in line with the other levels.

Furthermore, some players thought that when they successfully delivered the boxes, they had destroyed them. We believe that reducing the delivery zone size will help with this, however the now included tutorial makes it clear what the player should do to successfully deliver boxes. The delivery particle effect was also updated based on player feedback, and now includes a 'tick' particle to provide more positive feedback upon successful task completion.

Players significantly varied with their competence at using the hand trolleys to carry multiple boxes at once. Some players didn't use the trolleys at all, whereas some thought that they had to use the trolleys to carry any boxes. This is also addressed in the tutorial.

Game lockout was frequently an issue for participants playing through the final high-rise level as a respawn on falling was not included in the game.

Some participants experienced a soft game lockout while repeatably being hit by game hazards. Hazards were designed as time penalties, and after being hit control is removed from the player for a short time while their character recovers. It was observed that in some circumstances participants could be hit multiple times in a row without an opportunity to move their character to safety, effectively locking the player out of the game for far longer than intended.

Some relevant quotes are as follows:

- **Instantly understandable - red goes in the red, blue goes in the blue...**

- **It makes it interesting having to dodge the lorries and forklifts**

- **It’s not clear how many points you need to proceed to the next level… and there is no method of retrying for a higher score**

Most players understood the objective of the game but felt that they had no way of knowing if they were performing well

- **I thought the drones were stealing my boxes**

It was often not clear to players that they had delivered packages successfully, in part due to the hit boxes of delivery zones being too large and the delivery particle effect not being prominent enough

- **It feels like I’ve hardly got started**

It was common for participants to feel that the levels were too short.

## Post-Feedback Improvements

The following improvements we made to the game in response to feedback detailed in prior sections

- The time afforded for each level was extended from 60 seconds to 120 seconds to give less experienced gamers a chance to familiarize themselves with the game controls and mechanics
- Quite a few participants required assistance to complete some game tasks during observational evaluation or reported low confidence during querying evaluations. To address this the team modified the first level so that it contained a tutorial component which introduced new gamers to both the controls and objectives of the game.
- Participants frequently reported that they would have liked to retry certain levels, the team implemented a retry mechanism within the inter-level screen. A level select screen was also added to the main menu.
- The team introduced a respawn mechanism to avoid lockout for falling players in the final high-rise level.
- The hit boxes for delivery zones were reduced and set back further into the waiting lorry models to make it clearer that the player was delivering the packages to the lorry rather than the packages inexplicably disappearing.
- The particle effect for successful delivery was enhanced to draw more focus and include a large 'tick' symbol to better communicate to the player that the packages had been successfully collected in line with the objective of the game.
- A period of player invulnerability was added to the game and triggered immediately after the player recovered from a collision with a hazard, this allowed players a short period of time to move away from hazards and try another approach and reduce the likelihood of soft lockout via multiple successive collisions.
- Participants reported that they did not know how well they were performing, in response to this the team implemented a three star rating system, allowing players to measure their performance against a standard.
- A number of bugs we identified during observational evaluation, and fixed in the final version.
  - Some transitions were either missing or incorrectly implemented for a number of player animations.
  - The colliders of some drop off zones were not set to 'trigger' mode and caused the player to appear to float above the delivery zones.

## References

[A3D - Low Poly Logistics](https://assetstore.unity.com/packages/3d/environments/low-poly-logistics-137583)

[Broken Vector - Low Poly Tree Pack](https://assetstore.unity.com/packages/3d/vegetation/trees/low-poly-tree-pack-57866)

## Group Member Contributions

Max Plumley

- Player animations
- Player package collection/drop off mechanics
- Hand truck mechanics
- Exploding box particle system
- Sound effects
- Player/hazard collision mechanics
- Player respawn

Ted Barrett

- Modeled and textured road and forklift path pieces in blender
- Designed truck and forklift spawning and movement systems
- Designed level_diagonals, level_uphill, level_highrise
- Delivery particle effects
- Made tutorial
- Various image assets and logo design
- Trailer editing

Luke Russo

- Designed box outline shader and mesh
- Added retry and restart buttons
- Added the star rating system
- Created the settings menu
- Created the level select menu
- Designed the system to implement volume settings and game progress
- Designed the interactable effects shader

Ben Cook

- Added the basic player controls and made the first level
- Created the drone spawning, box-dropping functionality and drone animations
- Created the main menu and level progression system
- Added pause functionality
- Added the HUD with the score and timer animation
- Recorded gameplay footage / cinematics for the trailer
- Added some support from Bezos when the player is doing well (bezos flashing on the screen)
