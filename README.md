# Martian Robots

A .NET console application that simulates robots navigating on Mars. Robots receive commands to move and rotate on a rectangular grid, with special handling for robots that fall off the grid and leave "scent" markers for future robots.

## 🎯 Overview

This project implements a robot simulation system where:
- Multiple robots can be deployed on a Martian grid
- Robots respond to movement and rotation commands
- Robots that fall off the grid are marked as "lost" and leave a scent
- Future robots won't fall off the grid at positions where previous robots were lost
- The system tracks robot positions and their final states

## 🏗️ Architecture

The project follows a **layered architecture** with clean separation of concerns:

### Core Services
- **WorldService** (`IWorldService`): Manages grid bounds, robot placement, and scent tracking
- **RobotService** (`IRobotService`): Creates robots and parses command sequences
- **RobotNavigationService** (`IRobotNavigationService`): Handles robot movement and rotation logic
- **ConsoleController**: Orchestrates user input/output and coordinates all services

### Models
- **MartianRobot**: Represents a robot with position, direction, and state
- **BoundWorld**: Represents the Martian grid with bounds and robots
- **Direction** (enum): Compass directions (N, E, S, W) represented as degrees (0, 90, 180, 270)

### I/O Abstraction
- **IConsoleProvider**: Abstraction for console I/O, enabling easy testing with mock providers

## 📋 Features

### Robot Commands
- **L**: Rotate left (counter-clockwise 90°)
- **R**: Rotate right (clockwise 90°)
- **F**: Move forward one grid point

### Scent System
- When a robot moves off the grid, it's marked as lost
- The position where it fell is marked with a "scent"
- Future robots will not fall off at scent positions—they simply ignore the move

### Constraints
- Grid bounds: 0 < x ≤ maxX, 0 < y ≤ maxY
- Command sequences: maximum 100 characters
- Grid size: up to 50×50

## 🚀 Quick Start

### Prerequisites
- .NET 10.0
- C# 14.0

### Installation
```bash
git clone <repository-url>
cd MartianRobots
dotnet build
```
## 📖 Usage Guide

### Step-by-Step Instructions

#### Step 1: Start the Application

dotnet run

The application will start and wait for your input.

#### Step 2: Define the Grid Size

Enter the upper-right coordinates of the rectangular grid:

5 3

This creates a grid where:
- X-axis ranges from 1 to 5
- Y-axis ranges from 1 to 3
- Valid positions: (1,1) through (5,3)

#### Step 3: Deploy a Robot

Enter the robot's starting position and direction:

1 1 E

Format: <x> <y> <direction>
- **x**: X-coordinate
- **y**: Y-coordinate
- **direction**: N (North), E (East), S (South), or W (West)

#### Step 4: Send Robot Commands

Enter a sequence of commands (max 100 characters):

RFRFRFRF

Available commands:
- **R**: Turn right 90°
- **L**: Turn left 90°
- **F**: Move forward one grid point

The robot executes commands and outputs its final position.

#### Step 5: Deploy More Robots (Optional)

Repeat steps 3-4 for each additional robot.

#### Step 6: End Simulation

Type `end` to finish:

end

### Complete Example Session

5 3
1 1 E
RFRFRFRF
1 1 E

3 2 N
FRRFLLFFRRFLL
3 3 N LOST

0 3 W
LLFFFLFLFL
2 3 S

end
Closing Simulation.

### Input Format Details

**Grid Definition:** <maxX> <maxY>

**Robot Position:** <x> <y> <direction>

**Commands:** <command sequence>

Valid commands: L, R, F (case-insensitive)

## 🎮 Example Scenarios

### Scenario 1: Simple Robot Movement

Input:
```5 3
2 2 N
FFRF
```
Output:
```
2 4 E
```
Explanation:
- Robot starts at (2,2) facing North
- F: Move to (2,3)
- F: Move to (2,4)
- R: Turn right to face East
- Final position: (2,4) facing East

### Scenario 2: Robot Falls Off Grid

Input:
```
5 3
5 3 N
F
```


Output:
```
5 3 N LOST
```


Explanation:
- Robot at (5,3) tries to move North
- Would fall off grid, so marked as LOST

### Scenario 3: Scent Protection

Input:
```
5 3
5 3 N
F
5 3 N
F
```

Robot 1 Output:
```
5 3 N LOST
```
Robot 2 Output:
```
5 3 N
```


Explanation:
- Robot 1 leaves scent at (5,3)
- Robot 2 at same position tries to move
- Scent prevents it from falling off

## ⚠️ Error Handling

The application validates input and displays helpful messages:

- "Robot placed outside of boundary" - Use valid coordinates
- "Incorrect input, robots support up to 100 commands" - Reduce command length
- "Incorrect direction, supported: N E S W" - Use valid direction
- "Unexpected Input..." - Check format: x y direction

## 📝 Notes

- Grid coordinates use 1-based indexing
- Robots cannot start at (0,0)
- Lost robots ignore further commands
- Direction uses compass degrees: N=0°, E=90°, S=180°, W=270°
## 🚀 Potential Improvements

### 1. Command Handler Pattern (Strategy Pattern Enhancement)

**Current Approach:** The `RobotService` uses a switch statement to handle commands.

**Proposed Improvement:** Introduce an `ICommandHandler` abstraction with specialized handlers for each command type.

Benefits:
- **Open/Closed Principle**: Easy to add new commands without modifying existing code
- **Single Responsibility**: Each handler focuses on one command type
- **Testability**: Individual handlers can be tested in isolation
- **Extensibility**: New command types (e.g., 'D' for diagnostic, 'S' for speed adjustment) can be added without changing core logic

Implementation approach:
- Create `ICommandHandler` interface with methods like `CanHandle(char command)` and `ExecuteAsync(MartianRobot robot)`
- Implement concrete handlers: `RotateLeftHandler`, `RotateRightHandler`, `MoveForwardHandler`
- Register handlers in DI container
- Replace switch statement with handler lookup and execution

### 2. Web UI Implementation

**Current Approach:** Console-based input/output only.

**Proposed Improvement:** Build a web-based interface using ASP.NET Core with a frontend framework.

Benefits:
- **Visual Feedback**: Display grid with robot positions and movement history
- **Real-time Updates**: See robots moving in real-time
- **Better UX**: Drag-and-drop robot placement, clickable command buttons
- **Multi-user Support**: Multiple users can run simulations simultaneously
- **Simulation Replay**: Record and replay robot movements

Implementation approach:
- Create ASP.NET Core API project
- Expose endpoints: POST /simulations, POST /robots, POST /commands
- Build frontend with React or Blazor for interactive grid visualization
- Use SignalR for real-time robot position updates
- Store simulation history in database

### 3. OpenTelemetry Integration

**Current Approach:** No tracking of robot behavior or simulation statistics.

**Proposed Improvement:** Implement OpenTelemetry for comprehensive observability including traces, metrics, and logs.

Benefits:
- **Standard Industry Practice**: OpenTelemetry is the CNCF standard for observability
- **Vendor Agnostic**: Export data to any backend (Jaeger, Prometheus, Azure Monitor, etc.)
- **Performance Insights**: Track execution times and resource usage
- **Distributed Tracing**: Follow robot commands through entire execution chain
- **Metrics Collection**: Standardized metrics for monitoring

Implementation approach:

**Metrics to Track:**
- Robot loss events (counter)
- Scent protection events (counter)
- Command execution duration (histogram)
- Robots deployed per simulation (gauge)
- Average robot lifetime (gauge)
- Boundary violation hotspots (custom metric)

**Traces to Capture:**
- Simulation execution flow
- Individual robot command processing
- Grid boundary checks
- Scent detection operations

**Setup:**
- Add NuGet packages:
    - OpenTelemetry
    - OpenTelemetry.Exporter.Console (for development)
    - OpenTelemetry.Exporter.Jaeger (for distributed tracing)
    - OpenTelemetry.Exporter.Prometheus (for metrics)
    - OpenTelemetry.Instrumentation.AspNetCore
    - OpenTelemetry.Resources

- Configure in Program.cs:
    - Create `ActivitySource` for traces
    - Configure `MeterProvider` for metrics
    - Set up exporters (Jaeger for traces, Prometheus for metrics)
    - Add context propagation

- Instrument code:
    - Wrap robot operations with `Activity` for tracing
    - Record metrics with `Instrument` (Counter, Histogram, Gauge, UpDownCounter)
    - Track custom events (robot lost, scent protection, etc.)

**Observability Dashboards:**
- Grafana dashboards for metrics visualization
- Jaeger UI for distributed tracing
- Real-time monitoring of robot behavior
- Historical analysis of simulation patterns

Example metrics to expose:
- martianbots_robots_lost_total (Counter)
- martianbots_scent_protected_total (Counter)
- martianbots_command_duration_seconds (Histogram)
- martianbots_robots_deployed (Gauge)
- martianbots_average_robot_lifetime (Gauge)
- martianbots_boundary_violations_by_position (Custom metric with labels)

### Implementation Priority

1. **High Priority**: Command Handler Pattern (improves code maintainability significantly)
2. **Medium Priority**: OpenTelemetry Integration (adds operational value and industry-standard observability)
3. **Low Priority**: Web UI (nice-to-have, depends on business needs)

### Architecture Considerations for Future Features

- Maintain dependency injection for all new services
- Use primary constructors for consistency
- Write unit tests for all new functionality
- OpenTelemetry integrates seamlessly with existing DI patterns
- Use event sourcing for audit trail of robot movements
