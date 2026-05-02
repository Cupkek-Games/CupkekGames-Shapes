# CupkekGames Shapes

Wrappers + helpers around the Freya Shapes addon. Provides polyline-spline, depth-projected UI spline raycaster, and gradient utilities used by potion-throw and indicator visuals.

## What's inside

**Runtime** (`CupkekGames.Shapes.asmdef`)

- `PolylineSpline`, `PolylinePointFollower` — polyline path + waypoint follower
- `SplineUIToRaycast` — projects a UI spline onto a raycast layer at a given depth
- Gradient and color helpers

**Editor** (`CupkekGames.Shapes.Editor.asmdef`)

- Custom inspectors / drawers.

## Dependencies

- Freya Shapes addon (asmdef reference, not a UPM dep — bring your own copy)
- CupkekGames foundation packages: `services`, `pool`, `fadeables`, etc. via the registry.
