using JetBrains.Annotations;
using UnityEngine;
/// <summary>
/// An sfx object that could be used with or without a certain variable.
/// </summary>
/// <param name="Position"></param>
/// <param name="Id">String id for stopping manually</param>
/// <param name="Volume"></param>
/// <param name="Pitch"></param>
public struct SfxParams {
    public AudioClip Clip { get; private set; }
    public Vector3? Position  { get; private set; }
    public string Id { get; private set; }
    public float? Volume { get; private set; }
    public float? Pitch { get; private set; }

    public SfxParams(AudioClip clip)
    {
        Clip     = clip;
        Position = null;
        Id       = null;
        Volume   = null;
        Pitch    = null;
    }
    public SfxParams WithPosition(Vector3 position) => new SfxParams(this.Clip) { Position = position };
    public SfxParams WithId(string        id)       => new SfxParams(this.Clip) { Id      = id };
    public SfxParams WithVolume(float     volume)   => new SfxParams(this.Clip) { Volume  = volume };
    public SfxParams WithPitch(float      pitch)    => new SfxParams(this.Clip) { Pitch    = pitch };
}