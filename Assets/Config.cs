using UnityEngine;

namespace Sketch0731 {

[System.Serializable]
sealed class Config
{
    public float Width = 0.1f;
    public float Length = 5;
    public Vector2 Frequency = new Vector2(10, 15);
    [Range(0, 1)] public float Bias = 0.2f;
    [Range(0, 1)] public float BurstProbability = 0.2f;
    public float BurstIntensity = 200;
    public int Resolution = 100;
    public uint Seed = 1234;
}

} // namespace Sketch0731
