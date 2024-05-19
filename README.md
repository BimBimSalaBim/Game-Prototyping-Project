# Irregular Innovations - Game Prototyping Project

## Overview

Irregular Innovations is tasked with prototyping a game that combines elements from popular game archetypes like collectible creature games, open-world games, and farming games. This project aims to create a cross-platform game that runs on both PC and Android, focusing on core mechanics and functionality rather than art assets.

## Team Members
- Hassan Al Lawati
- Chris Maude
- Jason Morales
- Hoanh Nguyen
- Faizan Zafar

## Project Goals

The main objective is to explore the marketability and feasibility of a new game idea. The prototype should include essential game mechanics, serialized information handling, and trading functionality between players.

## Features

### Game Mechanics
1. **Movement and Environment**
   - Players can move through a 3D world in the PC version.
   - Creatures inhabit various worlds, move around, and can be trained.
   - Creatures exhibit different traversal methods (wings, legs, fins, etc.).

2. **Creature Behaviors**
   - Creatures can learn behaviors.
   - Creatures can have offspring based on species.

3. **Resource Collection**
   - Players can collect resources from the landscape and from repeatable farms.

### Serialized Information
1. **Cloud-based Storage**
   - Players can save data to cloud-based storage to synchronize between their PC and phone.
   - A creature can only exist on one device at a time, either the PC or the phone.

### Trading Creatures
1. **Trading System**
   - Players can trade creatures with each other.

## Development Progress

### Sprint Retrospective (10-18-2023)

#### What Went Well
1. The products we produced look very good.
2. The project has made great progress.
3. Use of the asset store was beneficial.
4. The sprint was smoother this time around.

#### What Didn’t Go Well
1. **Merge Conflicts**
   - Better coordination needed when merging.

2. **File Format Issues**
   - Some files were not OS agnostic.
   - Need to avoid non-agnostic file types.

3. **Task Roll-over**
   - Some tasks rolled over from the last sprint.
   - Indicates we need to avoid overburdening ourselves in future sprints.

## Core Components

### 1. PortalController
Manages the portals in the game world, allowing players to teleport between different locations.

### 2. InventoryManager
Handles the player's inventory system, allowing items to be added, selected, and used.

### 3. Database
Manages database interactions for user accounts and game data using MongoDB.

### 4. ThirdPersonController
Controls the player's character in the game world, handling movement, jumping, and camera control.

### Additional Components

#### EquipmentController
Handles the equipping and using of tools and weapons by the player.

#### FoV (Field of View)
Manages the behavior and state of creatures in the game world, including their interactions with the player.

#### FontEndController
Handles the front-end UI and user interactions, including login and signup functionality.

## Getting Started

### Prerequisites
- Unity 3D for game development
- Cloud service for data serialization
- Version control system (e.g., Git)

### Running the Prototype
1. Clone the repository.
2. Open the project in Unity.
3. Build the project for PC and Android platforms.
4. Run the build on the respective platforms.

## Contribution Guidelines

1. Follow the coding standards outlined in the project documentation.
2. Ensure all code is thoroughly tested before committing.
3. Coordinate with team members before merging branches to avoid conflicts.
4. Use OS-agnostic file formats whenever possible.

## License

This project is licensed under the MIT License. See the LICENSE file for details.

---

**Note**: Any passwords included in the code snippets have been changed and are no longer relevant.
``` &#8203;``【oaicite:0】``&#8203;
