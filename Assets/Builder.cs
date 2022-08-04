using UnityEngine;

namespace Sketch0731 {

sealed class Builder : MonoBehaviour
{
    const int Capacity = 256;

    [SerializeField, Range(0, Capacity)] int _rowCount = 10;
    [SerializeField] Config _config = null;
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
            _nodes[i].Activate(_config, i);

        for (var i = _rowCount; i < Capacity; i++)
            _nodes[i].Deactivate();
    }
}

} // namespace Sketch0731
