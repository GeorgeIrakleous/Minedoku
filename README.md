# Minedoku

A fresh take on puzzle games, blending grid-based deduction with multiplication scoring. Reveal and clear blocks to match target scores, flag suspected blocks, and navigate levels of increasing complexity.

---

## Features

- **5×5 Grid**: Always play on a 5×5 grid with hidden values.
- **Unique Mechanics**:
  - **Mines (**``**)**: Reveal one and it's game over.
  - **Values (**`1`**, **`2`**, **`3`**)**: Multiply your score by revealed values. Only `2` and `3` are required to complete a level; `1` simply clears blocks and gives hints.
  - **Flags**: Use keyboard shortcuts (` `` `, `1`, `2`, `3`, `Q`) or right-click to cycle flag types, marking suspected block values.
- **Hint System**: Row/column hints show the sum of values and number of mines.
- **Dynamic Audio**: SFX and music adjustable via in-game mixer; persists across sessions.
- **Smooth Animations**: DOTween-powered button hovers, panel pop‑ins, and score pop animations.
- **Settings & How To Play Panels**: In-game UI panels with open/close animations and input blocking when active.

---

## Getting Started

### Prerequisites

- **Unity** (Unity 6 6000.0.33f1 LTS or newer)
- **DOTween** package
- **TextMeshPro** (built into modern Unity)

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/GeorgeIrakleous/Minedoku.git
   ```
2. Open Unity Hub and add the cloned folder as a project.
3. Let Unity import assets and compile scripts.

### Running the Game

- **Play**: In the Unity Editor, press the **Play** button.
- **Build**: File → Build Settings → select platform → **Build**.

---

## How to Play

1. **Reveal Blocks**: Click a block to uncover its value.
2. **Avoid Mines (**``**)**: Revealing a mine ends the level.
3. **Score Goal**: Multiply your score by each revealed `2` or `3`. Reach the maximum product (precomputed per level) to win.
4. **Flags**:
   - Use ` `` ` to select mine‑flag mode.
   - Use `1`, `2`, `3`, `Q` to select value‑flag mode.
   - **Right-click** cycles through available flag types.
5. **Hints**: Row/column indicators show the sum of values and count of mines remaining.

---

## Controls & Shortcuts

| Action                 | Input         |
| ---------------------- | ------------- |
| Reveal block           | Left-click    |
| Cycle flag mode        | Right-click   |
| Select mine flag       | ` `` `           |
| Select 1‑flag          | `1`           |
| Select 2‑flag          | `2`           |
| Select 3‑flag          | `3`           |
| Select hint‑flag       | `Q`           |
| Open Settings panel    | `Esc`         |
| Close Settings panel   | `Esc`         |
| Open How To Play panel | *Menu button* |

---

## Audio & Assets

- **Asset Licenses**:
  - Unity Asset Store packs are covered under the Asset Store EULA (commercial use permitted).
  - `SwishSwoosh` free pack: Personal License allows embedding in commercial titles by a single user.
  - Other audio packs: ensure compliance with their individual licenses.

---

## Contributing

Contributions are welcome! Please fork the repo and submit a pull request with clear descriptions of your changes.

---

## License

This project—its code, art, audio, and other assets—is made available **only for non‑commercial, personal use** under the  
**Creative Commons Attribution‑NonCommercial 4.0 International** license (CC BY‑NC 4.0).

You are free to:

- **Share** — copy and redistribute the material in any medium or format  
- **Adapt** — remix, transform, and build upon the material  

Under the following terms:

1. **Attribution** — You must give appropriate credit, provide a link to the license, and indicate if changes were made.  
2. **NonCommercial** — You may not use the material for commercial purposes.

Full license text is in [LICENSE](LICENSE).  
To view the legal code, see: https://creativecommons.org/licenses/by-nc/4.0/

---

*Happy puzzling!*

