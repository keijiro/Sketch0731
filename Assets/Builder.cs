using UnityEngine;

namespace Sketch0731 {

[ExecuteInEditMode]
sealed class Builder : MonoBehaviour
{
    const int Capacity = 256;

    [SerializeField, Range(0, Capacity)] int _rowCount = 10;
    [SerializeField] Config _config = null;
    [SerializeField] Material _material = null;

    Node[] _pool;

    void InitializePool()
    {
        _pool = new Node[Capacity];
        for (var i = 0; i < Capacity; i++)
            _pool[i] = Node.Create(transform, _material);
    }

    void OnDisable()
      => OnDestroy();

    void OnDestroy()
    {
        if (_pool != null)
        {
            for (var i = 0; i < Capacity; i++) Util.DestroyObject(_pool[i]);
            _pool = null;
        }
    }

    void Update()
    {
        if (_pool == null || _pool.Length == 0) InitializePool();

        for (var i = 0; i < _rowCount; i++)
            _pool[i].Activate(_config, i);

        for (var i = _rowCount; i < Capacity; i++)
            _pool[i].Deactivate();
    }
}

} // namespace Sketch0731
