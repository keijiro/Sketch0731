using UnityEngine;

namespace Sketch0731 {

[ExecuteInEditMode]
sealed class Builder : MonoBehaviour
{
    [SerializeField] Config _config = null;
    [SerializeField] Material _material = null;

    Node[] _pool;

    void InitializePool()
    {
        _pool = new Node[Config.MaxRows];
        for (var i = 0; i < _pool.Length; i++)
            _pool[i] = Node.Create(transform, _material);
    }

    void OnDisable()
      => OnDestroy();

    void OnDestroy()
    {
        if (_pool != null)
        {
            for (var i = 0; i < _pool.Length; i++)
                Util.DestroyObject(_pool[i]);
            _pool = null;
        }
    }

    void Update()
    {
        if (_pool == null || _pool.Length == 0) InitializePool();

        for (var i = 0; i < _config.Rows; i++)
            _pool[i].Activate(_config, i);

        for (var i = _config.Rows; i < _pool.Length; i++)
            _pool[i].Deactivate();
    }
}

} // namespace Sketch0731
