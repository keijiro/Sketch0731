using UnityEngine;

namespace Sketch0731 {

[System.Serializable]
sealed class Config
{
    public readonly float Width = 0.1f;
    public readonly float Length = 5;
    public readonly Vector2 Frequency = new Vector2(10, 15);
    public readonly float Bias = 0;
    public readonly int Resolution = 100;
    public readonly uint Seed = 10;
}

} // namespace Sketch0731
