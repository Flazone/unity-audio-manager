## Overview
An easy-to-use and performant audio system for Unity.

Perfect for game jams and prototypes.

## Features
- Create sounds assets and tweak their volume, pitch, delay and more
- Play them from wherever you want using a single line of code
- Preview your sound in edit mode
  
## Usage
- Create a new SFX ScriptableObject by right clicking on your AudioClip file `Create/Audio/SFX`
- Set it up according to your needs, add your clips, tweak its volume, pitch, delay...
- Call its **Play** method in one of your script

```C#
_sfx.Play();
```
- Optionally, override its settings
```C#
_sfx.Play().SetVolume(2f);
```

```C#
_sfx.Play().SetVolume(2f).SetPitch(0.8f).SetDelay(0.5f).SetLoop(true);
```

```C#
_sfx.Play().SetVolume(2f).SetPitch(0.8f);
```

```C#
SFX.Play().OnFinished(() =>
{
    // on finished
});
```

## Media
https://github.com/Flazone/unity-audio-manager/assets/12300786/575bdd32-7fc9-4fd3-b965-dbc0ab5c5da8


## To Dos
- Spatialization
- Music implementation
- Composite sounds
- Unity built-in effects support
