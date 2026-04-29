# CupkekGames Fadeable

Tween-based fade utilities. Extracted from `com.cupkekgames.core` so consumers can opt in independently.

## What's inside

**Runtime** (`CupkekGames.Fadeables.asmdef`)

- `Fadeable<T>` / `FadeableMono` — base fade behavior with `OnFadeIn` / `OnFadeOut` events
- `FadeableColor` / `FadeablePosition` / `FadeableScale` — typed fade variants
- `EasingType` — easing curve enum

**Editor** (`CupkekGames.Fadeables.Editor.asmdef`)

- `FadeableMonoEditor` — custom inspector for `FadeableMono`

## Dependencies

None.
