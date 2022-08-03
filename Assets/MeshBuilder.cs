using UnityEngine;

[System.Serializable]
sealed class Modeling
{
    public float Width = 0.1f;
    public float Length = 5;
    public Vector2 Frequency = Vector2.one * 10;
    public float Bias = 0;
    public int Resolution = 100;
    public uint Seed = 10;
}

sealed class MeshBuilder : MonoBehaviour
{
    const int Capacity = 256;

    [SerializeField, Range(0, Capacity)] int _rowCount = 10;
    [SerializeField] Modeling _modeling = null;
    [SerializeField] Material _material = null;

    Node[] _nodes = new Node[Capacity];

    void InitializePool()
    {
        for (var i = 0; i < Capacity; i++)
            _nodes[i] = Node.Create(transform, _material);
    }

    void Start()
    {
        InitializePool();

        for (var i = 0; i < _rowCount; i++)
            _nodes[i].Activate(_modeling, i);

        for (var i = _rowCount; i < Capacity; i++)
            _nodes[i].Deactivate();
    }
}
