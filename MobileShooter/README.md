# 🎮 High-Performance Mobile Top-Down Shooter (Unity 6.6 URP)

A high-performance, event-driven top-down shooter built in **Unity 6.6 (URP)**, designed with a strong focus on clean architecture, zero-garbage-collection core loops, and mobile/WebGL optimization standards.

[![Unity 6.6](https://img.shields.io/badge/Unity-6.6-black?style=flat&logo=unity)](https://unity.com)
[![URP](https://img.shields.io/badge/Pipeline-URP-blue?style=flat)](https://unity.com)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

---

## 🎥 Gameplay & Preview


---

## 🛠️ Technical Highlights & Architecture

This project was built from the ground up to reflect production-ready coding standards and robust software design patterns:

*   **Event-Driven Architecture:** Decoupled systems communicating via C# `Action` delegates and observer patterns, ensuring high modularity and zero tight coupling between game logic and UI.
*   **Finite State Machine (FSM):** Clean `IState` pattern implementation managing game states (`GameManager`), transitions, and wave loops.
*   **Zero-Allocation Asynchronous Programming:** Utilizes **Cysharp.UniTask** alongside `CancellationTokenSource` for efficient, garbage-collection-free async handling of gameplay feedback (camera shakes, screen flashes, and audio cues).
*   **Robust Object Pooling:** Custom pooling systems for projectiles and enemies to eliminate runtime instantiation overhead and prevent GC spikes during intense combat waves.

---

## ⚡ Performance & Optimization

Optimization was verified using the **Unity Profiler**, **Frame Debugger**, and standalone builds:
*   **Rendering:** Fully compatible with **SRP Batcher**, verified via Frame Debugger to minimize CPU overhead and keep SetPass calls exceptionally low.
*   **Physics Optimization:** Custom layer-based collision matrix meticulously configured to strip out unnecessary collision checks between non-interacting layers, reducing CPU load during heavy wave spawns.
*   **Memory Management:** Core gameplay loops achieve virtually **0 B GC Alloc**, avoiding memory fragmentation and hitching on mobile platforms.

---

## 🚀 Tech Stack

*   **Engine:** Unity 6.6 (Universal Render Pipeline - URP)
*   **Language:** C#
*   **Key Libraries:** Cysharp.UniTask
*   **Platforms:** Android, WebGL

---

## 📁 Project Structure

```text
Assets/
├── Scripts/
│   ├── Player/        # Player controllers
│   ├── Core/          # GameManager, FSM states, initialization
│   ├── Systems/       # Object pooling architecture and other systems
│   ├── UI/            # Event-driven UI managers (Health, Waves)
│   └── AI/            # Enemy behaviors and wave management
├── Prefabs/           # Reusable game objects
└── Settings/          # URP and Renderer pipeline assets
```

## 📦 Getting Started

To get a local copy up and running, follow these steps:

### Prerequisites
* Unity 6.6 (or newer)
* Universal Render Pipeline (URP)

### Installation & Run

**1. Clone the repository:**
```bash
git clone https://github.com/your-username/your-repo-name.git
```
**2. Open the project:**
Open the project folder in Unity 6.6 (or newer).

**3. Load the scene:**
Load the main scene located in Assets/Scenes/.

**4. Run:**
Press Play in the editor or build for your target platform (WebGL / Android).

## 🕹️ Controls

The game adapts its control scheme depending on the target platform:

*   **WebGL / Desktop:**
    *   **Movement:** `W`, `A`, `S`, `D` keys
    *   **Shoot:** `Spacebar`
*   **Mobile (Android):**
    *   **Movement:** Left virtual joystick
    *   **Shoot:** Right virtual joystick / Action button

## 📄 License
Distributed under the MIT License. See [LICENSE](LICENSE) for more information.
