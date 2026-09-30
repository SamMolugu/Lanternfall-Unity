# Lanternfall Unity — Design Foundation

## Goal

Rewrite the Electron cave crawler in Unity with **Swords & Sandals–style** equipment: wearing gear changes the character’s visible model via sockets / layered sprites, not floating inventory icons.

## Prototype reference

Electron repo: `https://github.com/SamMolugu/Lanternfall`

Port these *ideas* later (not the JS code):

- Race × gender × class × kit origin
- Skill trees (32 arts / class)
- Cave delves, FOV / Sight, Burden / Footfall
- Biomes: moss, bone, crystal, molten

## Art pipeline (build in this order)

1. **Character bases** — idle (and later walk) per race × gender  
2. **Equipment worn pieces** — chest, helm, boots, gloves, weapon, offhand authored on the same pose  
3. **Monsters** — one archetype at a time with idle/attack  
4. **Level kits** — seamless floor/wall tiles + props per biome  
5. **Gameplay** — movement, combat, inventory, dungeon gen  

## Equipment model

`EquipmentSlot`: Weapon, Offhand, Helm, Chest, Gloves, Boots, Amulet, Ring, Lantern  

`CharacterVisual` holds a `Transform` socket per visual slot.  
`EquipmentItemDefinition` ScriptableObject points at the worn sprite/prefab for that slot.  
`LoadoutDefinition` groups a starting kit (Blade / Lamp / Bulwark / Shadow).

Applying a loadout or swapping an item updates sockets only — never paste UI icons onto the body.

## Scenes

| Scene | Purpose |
|-------|---------|
| `CharacterLab` | Preview race/gender + loadout swaps |
| `Bootstrap` (later) | Boot into camp / delve |

## Branching

- `main` — stable  
- `feature/character-foundation` — this scaffold  
- Later: `feature/monster-pack-*`, `feature/biome-moss`, `feature/delve-core`, etc.
