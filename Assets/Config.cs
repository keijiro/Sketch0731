using UnityEngine;

namespace Sketch0731 {

[System.Serializable]
sealed class Config
{
    public readonly float Width = 0.1f;
    public readonly float Length = 5;
    public readonly Vector2 Frequency = new Vector2(10, 15);
    [Range(0, 1)] public readonly float Bias = 0.2f;
    [Range(0, 1)] public readonly float BurstProbability = 0.2f;
    [Range(0, 1)] public readonly float BurstIntensity = 200;
    public readonly int Resolution = 100;
    public readonly uint Seed = 1234;
}

} // namespace Sketch0731
