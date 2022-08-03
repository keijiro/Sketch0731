using UnityEngine;
using System.Collections.Generic;
using Random = Unity.Mathematics.Random;

sealed class MeshBuilder : MonoBehaviour
{
    const int Capacity = 256;

    [SerializeField, Range(0, Capacity)] int _rowCount = 10;
    [SerializeField] float _width = 0.1f;
    [SerializeField] float _length = 5;
    [SerializeField] int _resolution = 100;
    [SerializeField] Vector2 _frequency = Vector2.one * 10;
    [SerializeField] float _bias = 0;
    [SerializeField] uint _seed = 10;
    [SerializeField] Material _material = null;

    (GameObject go, MeshConstructor mesh) [] _pool
      = new (GameObject, MeshConstructor)[Capacity];

    void InitializeObjectPool()
    {
        var comps = new [] { typeof(MeshFilter), typeof(MeshRenderer) };

        for (var i = 0; i < Capacity; i++)
        {
            var go = new GameObject($"Row{i}", comps);
            go.hideFlags = HideFlags.DontSave;

            go.transform.parent = transform;

            go.GetComponent<MeshRenderer>().sharedMaterial
              = new Material(_material);

            _pool[i] = (go, null);
        }
    }

    void OnDestroy()
    {
        for (var i = 0; i < Capacity; i++)
        {
            if (_pool[i].go != null) Destroy(_pool[i].go);
            if (_pool[i].mesh != null) _pool[i].mesh.Dispose();
            _pool[i] = (null, null);
        }
    }

    void Start()
    {
        InitializeObjectPool();

        var rand = new Random(_seed);
        rand.NextUInt4();

        for (var i = 0; i < _rowCount; i++)
        {
            _pool[i].go.SetActive(true);
            
            if (_pool[i].mesh != null) _pool[i].mesh.Dispose();

            var cons = new MeshConstructor(_resolution)
              { Width = _width, Length = _length,
                Frequency = rand.NextFloat(_frequency.x, _frequency.y),
                Phase = rand.NextFloat(0, _bias) };

            _pool[i].mesh = cons;

            cons.InitializeIndexBuffer();
            cons.InitializeVertexBuffer();
            cons.ConstructMesh();

            _pool[i].go.GetComponent<MeshFilter>().sharedMesh = cons.SharedMesh;
            var material = _pool[i].go.GetComponent<MeshRenderer>().sharedMaterial;

            _pool[i].go.transform.localPosition = new Vector3(i * _width, 0, 0);
            _pool[i].go.transform.localRotation = Quaternion.identity;

            var color = Color.HSVToRGB(rand.NextFloat(), 1, 1);
            var intensity = rand.NextFloat() < 0.35f ? 200 : 0;
            material.SetColor("_BaseColor", color);
            material.SetColor("_EmissiveColor", color * intensity);
        }

        for (var i = _rowCount; i < Capacity; i++)
            _pool[i].go.SetActive(false);
    }
}
