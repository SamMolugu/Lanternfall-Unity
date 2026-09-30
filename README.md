# Lanternfall (Unity)

Cave crawler rewrite of the Electron prototype ([Lanternfall](https://github.com/SamMolugu/Lanternfall)).

## Approach

**Swords & Sandals–style visuals first:** character bases → worn equipment sockets → monsters → dungeon kits → then gameplay systems.

The Electron repo stays as the design / systems reference (skills, dungeon rules, combat feel). This repo is the new source of truth for the shippable game.

## Requirements

- Unity Hub
- **Unity 6000.0 LTS** (Unity 6) or **2022.3 LTS**
- Modules: **2D**, **Universal RP** (optional later)

Open this folder in Unity Hub → *Add* → open `Assets/_Project/Scenes/CharacterLab.unity`.

## Branching

| Branch | Purpose |
|--------|---------|
| `main` | Stable scaffold |
| `feature/character-foundation` | Character, equip sockets, monster & level art folders |

## Folder map

```
Assets/_Project/
  Art/Characters|Equipment|Monsters|Levels|Reference
  Prefabs/...
  Scripts/Character|Equipment|World|Core
  Scenes/CharacterLab
  Docs/DESIGN.md
```

## Next steps

1. Install Unity 6 LTS via Hub and open this project
2. Drop base character sprites into `Art/Characters/`
3. Assign them on `CharacterVisual` + slot transforms on the Character prefab
4. Expand loadouts (Blade / Lamp / Bulwark / Shadow) as worn visuals
