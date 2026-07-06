<p align="center">
  <img src="https://github.com/Chandan-Baskey/SnowRide-2dGame/blob/f07154bd5c1882806cb7d0cc2aa33e6970dc07de/Snow%20Ride%20B.jpg?raw=true" alt="Escape The Cave banner" width="100%">
</p>

<div align="center">

# ❄️ SnowRide2d

### A 2D Surface-Effector Sledding / Flip Game built in Unity

![Unity](https://img.shields.io/badge/Unity-2022.3%2B-black?logo=unity)
![C%23](https://img.shields.io/badge/C%23-239120?logo=c-sharp&logoColor=white)
![Platform](https://img.shields.io/badge/Platform-PC%20%7C%20Android-blue)
![Status](https://img.shields.io/badge/Status-In%20Development-yellow)
![License](https://img.shields.io/badge/License-MIT-green)

[![Game Scene](https://github.com/Chandan-Baskey/SnowRide2d/blob/main/Game%20Scene.png?raw=true)](https://github.com/Chandan-Baskey/SnowRide2d/blob/main/Game%20Scene.png)

</div>

---

## 📖 Overview

**SnowRide2d** is a 2D arcade-style sledding game where the player rides a `Rigidbody2D`-driven sled/ball down a snowy slope powered by a **`SurfaceEffector2D`**. Instead of traditional `AddForce` locomotion, the player tilts/rotates the rigidbody using torque while a surface effector continuously pushes it forward — holding **Vertical input** boosts the effector speed.

The core scoring loop is built around **flip tracking**: the game accumulates the player's total rotation every frame, and a full 360°-ish rotation (`> 340°`) registers as a completed flip and a point. The run ends either by **crashing into the floor** (loss) or **reaching the finish line** (win), and a pre-game **character selection screen** lets the player pick between two characters before the run begins.

> 📌 This README was generated from the actual scripts in the repository (`PlayerController`, `CrashDetector`, `FinishLine`, `SnowTril`, `ScoreManager`, `CharSelection`, `MainUi`). Sections describing Inspector wiring (effector references, layers, particle systems) are inferred from script behavior — confirm against your actual scene setup.

---

## 🎮 Features

| Feature | Description |
|---|---|
| 🛷 Surface-Effector Locomotion | Forward movement driven by `SurfaceEffector2D.speed`, not direct rigidbody force |
| 🌀 Torque-Based Tilt Control | Horizontal input applies `Rigidbody2D.AddTorque` to spin/tilt the player |
| ⚡ Speed Boost | Holding Vertical input swaps the effector from base speed to boost speed |
| 🔁 Flip Detection & Scoring | Tracks accumulated rotation per frame; full flips add to the score |
| 💥 Crash / Loss State | Colliding with the `Floor` layer (via trigger) disables movement and reloads the scene |
| 🏁 Finish Line / Win State | Player reaching the finish trigger plays a win effect and reloads the scene |
| ❄️ Dynamic Snow Trail | Particle trail toggles on/off based on contact with the `Floor` layer |
| 🧑‍🤝‍🧑 Character Selection | Pre-run screen to choose between two characters (Dino / Frog), pausing time until a choice is made |
| 🖥️ Main Menu | Simple Play / Quit entry screen |

---

## 🖼️ Gameplay Preview

<div align="center">

| Main Scene Layout | Gameplay View | Character Selection |
|---|---|---|
| ![Game Scene](https://github.com/Chandan-Baskey/SnowRide2d/blob/main/Game%20Scene.png?raw=true) | ![Game View](https://github.com/Chandan-Baskey/SnowRide2d/blob/main/Game%20View.png?raw=true) | ![Character Selection](https://github.com/Chandan-Baskey/SnowRide2d/blob/main/Character%20Selection.png?raw=true) |

</div>

---

## 🧭 Game Flow

```
┌─────────────────┐
│   Main Menu      │  (MainUi.cs)
│  Scene Index 0   │
└────────┬─────────┘
         │ Play()
         ▼
┌─────────────────────────┐
│  Character Selection     │  (CharSelection.cs)
│  Time.timeScale = 0      │  ← game frozen until a character is chosen
│  Scene Index 1           │
└────────┬─────────────────┘
         │ ChooseDino() / Choosefrog()
         │  → activates chosen GameObject
         │  → Time.timeScale = 1
         │  → scoreCanvas.SetActive(true)
         ▼
┌─────────────────────────────────────────────┐
│                 GAMEPLAY LOOP                 │
│                                                │
│   PlayerController.Update()                   │
│     ├─ RotatePlayer()  → AddTorque (tilt)     │
│     ├─ BoostPlayer()   → effector.speed       │
│     └─ CalculateFlips()→ ScoreManager.AddScore│
│                                                │
│   SnowTril → toggles particle trail on Floor  │
└───────────────┬───────────────┬───────────────┘
                │               │
        Floor trigger     Player reaches
        (CrashDetector)   FinishLine trigger
                │               │
                ▼               ▼
        ❌ Loss state      🏁 Win state
        DisableMovement()  WinEffect.Play()
        Invoke→ReloadScene Invoke→ReloadScene
        (1s delay)         (1s delay)
                │               │
                └───────┬───────┘
                        ▼
               SceneManager.LoadScene(1)
```

---

## 🧩 Script Breakdown

### `MainUi.cs`
The main menu controller.

| Method | Behavior |
|---|---|
| `Play()` | Loads scene at build index `1` |
| `QuitGame()` | Calls `Application.Quit()` |

**Inspector setup:** Attach to a Canvas/UI GameObject; wire `Play()` and `QuitGame()` to corresponding UI Buttons' `OnClick()` events.

---

### `CharSelection.cs`
Pre-run character picker. Freezes time on load so the player can choose without the world simulating underneath them.

```csharp
void Start()
{
    Time.timeScale = 0;
}

void BeginGame()
{
    Time.timeScale = 1f;
    scoreCanvas.SetActive(true);
    gameObject.SetActive(false);
}
```

| Inspector Field | Type | Purpose |
|---|---|---|
| `scoreCanvas` | `GameObject` | Score UI, enabled once gameplay begins |
| `dino` | `GameObject` | Dino character, activated by `ChooseDino()` |
| `frog` | `GameObject` | Frog character, activated by `Choosefrog()` |

| Public Method | Behavior |
|---|---|
| `ChooseDino()` | Activates `dino`, then calls `BeginGame()` |
| `Choosefrog()` | Activates `frog`, then calls `BeginGame()` |
| `Back()` | Returns to scene index `0` (main menu) |

> ⚠️ Both `dino` and `frog` should start **disabled** in the scene — the script only ever turns them *on*, never off, so if both start active they will both be present in-game.

---

### `PlayerController.cs`
The heart of the gameplay — movement, boosting, and flip-based scoring.

| Inspector Field | Type | Purpose |
|---|---|---|
| `torqueAmount` | `float` | Torque applied per frame while tilting (default `1`) |
| `baseSpeed` | `float` | `SurfaceEffector2D.speed` when not boosting (default `15`) |
| `boostSpeed` | `float` | `SurfaceEffector2D.speed` while boosting (default `20`) |
| `addScore` | `ScoreManager` | Reference used to report completed flips |

**Movement — `RotatePlayer()`:**
```csharp
float moveX = Input.GetAxis("Horizontal");
if (moveX < 0)       player.AddTorque(torqueAmount);
else if (moveX > 0)  player.AddTorque(-torqueAmount);
```
Tilting is purely torque-based — there's no direct horizontal translation. The player's apparent forward motion comes entirely from the surface effector, while left/right input spins the rigidbody.

**Boosting — `BoostPlayer()`:**
```csharp
float moveY = Input.GetAxis("Vertical");
surfaceEffector.speed = moveY > 0 ? boostSpeed : baseSpeed;
```
The `SurfaceEffector2D` is located via `FindFirstObjectByType<SurfaceEffector2D>()` at `Start()` — there should be **exactly one** `SurfaceEffector2D` in the scene (typically on the slope/ground collider) for this lookup to behave predictably.

**Flip scoring — `CalculateFlips()`:**
```csharp
float currentRotation = transform.rotation.eulerAngles.z;
totalRotation += Mathf.DeltaAngle(previousRotation, currentRotation);
if (totalRotation > 340 || totalRotation < -340)
{
    flipCount += 1;
    totalRotation = 0;
    addScore.AddScore(1);
}
```
Rotation is accumulated frame-to-frame using `Mathf.DeltaAngle` (handles the 0°/360° wraparound correctly), and a flip is counted once accumulated rotation crosses ±340°.

**Public API:**
| Method | Behavior |
|---|---|
| `DisableMovement()` | Sets `canplayerMove = false`, halting `Update()` logic (called by `CrashDetector` on loss) |

---

### `CrashDetector.cs`
Detects the loss condition when the player hits the ground/floor.

```csharp
private void OnTriggerEnter2D(Collider2D collision)
{
    int layerIndex = LayerMask.NameToLayer("Floor");
    if (collision.gameObject.layer == layerIndex)
    {
        playerController.DisableMovement();
        Invoke("ReloadScene", 1f);
        lossEffect.Play();
    }
}
```

| Inspector Field | Type | Purpose |
|---|---|---|
| `lossEffect` | `ParticleSystem` | Played on crash |

**Setup requirements:**
- A `Floor` layer must exist in **Project Settings → Tags and Layers**.
- The collider that should trigger a loss must be marked **Is Trigger** and assigned to the `Floor` layer.
- `playerController` is auto-resolved via `FindFirstObjectByType<PlayerController>()` — only one `PlayerController` should exist in the scene.

---

### `FinishLine.cs`
Detects the win condition.

```csharp
private void OnTriggerEnter2D(Collider2D collision)
{
    int layerIndex = LayerMask.NameToLayer("Player");
    if (collision.gameObject.layer == layerIndex)
    {
        Invoke("ReloadScene", 1f);
        WinEffect.Play();
    }
}
```

| Inspector Field | Type | Purpose |
|---|---|---|
| `WinEffect` | `ParticleSystem` | Played on reaching the finish line |

**Setup requirements:**
- The player GameObject's collider must be on the `Player` layer.
- The finish line's collider must be a trigger.

---

### `SnowTril.cs`
Cosmetic snow-trail effect that turns on while the player is in contact with the floor.

```csharp
private void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.gameObject.layer == LayerMask.NameToLayer("Floor"))
        snowEffect.Play();
}
private void OnCollisionExit2D(Collision2D collision)
{
    if (collision.gameObject.layer == LayerMask.NameToLayer("Floor"))
        snowEffect.Stop();
}
```

| Inspector Field | Type | Purpose |
|---|---|---|
| `snowEffect` | `ParticleSystem` | Snow spray, toggled by floor contact |

> Note: this script relies on **physical collisions** (`OnCollisionEnter2D`/`OnCollisionExit2D`), unlike `CrashDetector` and `FinishLine` which use **trigger** events. The floor collider that drives the snow trail must therefore be a *non-trigger* collider, separate from (or in addition to) any trigger collider used for the crash check.

---

### `ScoreManager.cs`
Displays the running flip count.

```csharp
public void AddScore(int amount)
{
    score += amount;
    scoreText.text = "Flip: " + score;
}
```

| Inspector Field | Type | Purpose |
|---|---|---|
| `scoreText` | `TextMeshProUGUI` | UI label showing `"Flip: <score>"` |

---

## 🎹 Controls

| Input | Action |
|---|---|
| `A` / `D` or `←` / `→` | Tilt the player (apply torque) |
| `W` or `↑` | Boost forward speed |
| (release) | Return to base speed |

---

## ⚙️ Setup & Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/Chandan-Baskey/SnowRide2d.git
   ```
2. Open the project in **Unity 2022.3 LTS or later** via Unity Hub.
3. Confirm **Project Settings → Tags and Layers** includes both:
   - `Floor`
   - `Player`
4. Confirm **Build Settings** scene order:
   - **Index 0:** Main Menu
   - **Index 1:** Gameplay scene (also doubles as the reload target on win/loss)
5. In the gameplay scene, ensure:
   - Exactly one `SurfaceEffector2D` exists on the slope surface.
   - Exactly one `PlayerController` exists on the player rigidbody.
   - `dino` and `frog` GameObjects start **inactive**.
6. Required packages (via **Package Manager**):
   - `TextMeshPro`
7. Press **Play** from the Main Menu scene to test the full flow.

---

## 📁 Project Structure

```
SnowRide2d/
├── Assets/
│   └── Scripts/
│       ├── MainUi.cs            # Main menu (Play / Quit)
│       ├── CharSelection.cs     # Pre-game character picker
│       ├── PlayerController.cs  # Movement, boost, flip scoring
│       ├── CrashDetector.cs     # Loss condition
│       ├── FinishLine.cs        # Win condition
│       ├── SnowTril.cs          # Snow trail VFX
│       └── ScoreManager.cs      # Flip counter UI
├── Packages/
├── ProjectSettings/
├── Game Scene.png
├── Game View.png
└── Character Selection.png
```

---

## 🐛 Known Issues

| Issue | Location | Detail |
|---|---|---|
| Both characters can stay active | `CharSelection.cs` | `ChooseDino()`/`Choosefrog()` only ever `SetActive(true)` their target — if both `dino` and `frog` are active by default in the scene, choosing one won't deactivate the other |
| Mixed trigger/collision detection | `CrashDetector.cs` vs `SnowTril.cs` | Crash detection uses `OnTriggerEnter2D` against the `Floor` layer, while the snow trail uses `OnCollisionEnter2D`/`Exit2D` against the same layer name — this requires two separate floor colliders (one trigger, one solid) or careful collider layering to avoid conflicting physics behavior |
| Win reloads gameplay scene, not a "win" scene | `FinishLine.cs` | `ReloadScene()` calls `SceneManager.LoadScene(1)`, the same index used by `CrashDetector`'s loss reload — there is currently no dedicated win/results scene, so winning and losing both restart the same run |
| Unused/incomplete code | `SnowTril.cs` | Contains a commented-out `audioSource` field/calls and an empty `Clip()` method that is never called |
| Hardcoded scene indices | `MainUi.cs`, `CharSelection.cs`, `CrashDetector.cs`, `FinishLine.cs` | All scene transitions use raw `LoadScene(int)` indices rather than named scenes or constants, making the Build Settings order load-bearing and easy to break when scenes are reordered |
| Single-instance assumptions | `PlayerController.cs`, `CrashDetector.cs` | Both rely on `FindFirstObjectByType` to locate `SurfaceEffector2D` / `PlayerController` respectively — silently fails (null reference) if the scene has zero or multiple matching objects |

---

## 🗺️ Roadmap

- [ ] Add a dedicated win/results scene distinct from the gameplay reload
- [ ] Fix character selection to deactivate the unchosen character explicitly
- [ ] Replace hardcoded `LoadScene(int)` indices with named scene constants
- [ ] Re-enable or remove the commented-out audio in `SnowTril.cs`
- [ ] Add a persistent high-score / best-flip tracker across runs
- [ ] Android touch input support (currently keyboard-axis only)

---

## 👤 Credits

**Developed by [Chandan Baskey](https://github.com/Chandan-Baskey)**

[![GitHub](https://img.shields.io/badge/GitHub-Chandan--Baskey-181717?logo=github)](https://github.com/Chandan-Baskey)

---

<div align="center">

⭐ If you like this project, consider giving it a star on GitHub!

</div>
