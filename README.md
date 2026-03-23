# Graveyard Event-Driven System

## PROJECT IS ON DESIGN-TESTING BRANCH

## Overview
This project is an event-driven gameplay system built in Unity.  
It focuses on using events and interfaces to connect gameplay systems in a modular way.

---

## 🎥 Video Demonstration

[![Watch the video](https://img.youtube.com/vi/HI_Z8_iSTCc/0.jpg)](https://www.youtube.com/watch?v=HI_Z8_iSTCc)

Click the image above to watch the full gameplay demo.

---

## Features

- Player interaction system (chests, lanterns) * gravestones were wip, wanted to be able to go up and read text on them but decided not to add that
- Skeleton AI with detection 
- Ghost AI reacting to skeleton events
- Event-driven environment (floating gravestones)
- UI updates and audio feedback through events
- Health system using interfaces

---

## Systems Breakdown

### Interfaces
- `IInteractable` (provided)
- `IHittable` (custom)

Used across multiple objects to keep interactions flexible.

---

### Events
- `OnSkeletonRoar` → triggers ghost reactions
- `OnChestLooted` → updates UI + plays audio
- `OnHauntStart / OnHauntEnd` → controls environment changes

Each event has multiple listeners.

---

### Gameplay Flow
1. Player enters skeleton range
2. Skeleton chases and roars
3. Ghost reacts and investigates
4. Ghost enters haunt state and chases player doing damage
5. Gravestones float during haunt
6. Player interacts with objects (chests, lanterns)

---

## Notes
- System is fully event-driven
- Components are loosely connected
- Designed for easy expansion
