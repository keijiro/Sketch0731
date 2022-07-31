using UnityEngine;
using System.Collections.Generic;
using Random = Unity.Mathematics.Random;

sealed class MeshBuilder : MonoBehaviour
{
    [SerializeField] int _rowCount = 10;
    [SerializeField] float _width = 0.1f;
    [SerializeField] float _length = 5;
    [SerializeField] int _resolution = 100;
    [SerializeField] Vector2 _frequency = Vector2.one * 10;
    [SerializeField] float _bias = 0;
    [SerializeField] uint _seed = 10;
    [SerializeField] Material _material = null;

    List<MeshConstructor> _meshes = new List<MeshConstructor>();

    void Start()
    {
        var comps = new [] { typeof(MeshFilter), typeof(MeshRenderer) };

        var rand = new Random(_seed);
        rand.NextUInt4();

        for (var i = 0; i < _rowCount; i++)
        {
            var cons = new MeshConstructor(_resolution)
              { Width = _width, Length = _length,
                Frequency = rand.NextFloat(_frequency.x, _frequency.y),
                Phase = rand.NextFloat(0, _bias) };

            cons.InitializeIndexBuffer();
            cons.InitializeVertexBuffer();
            cons.ConstructMesh();

            var go = new GameObject($"Row{i}", comps);
            go.GetComponent<MeshRenderer>().sharedMaterial = _material;
            go.GetComponent<MeshFilter>().sharedMesh = cons.SharedMesh;

            go.transform.parent = transform;
            go.transform.localPosition = new Vector3(i * _width, 0, 0);
            go.transform.localRotation = Quaternion.identity;

            _meshes.Add(cons);

            var color = Color.HSVToRGB(rand.NextFloat(), 1, 1);
            var intensity = rand.NextFloat() < 0.35f ? 200 : 0;
            go.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", color);
            go.GetComponent<MeshRenderer>().material.SetColor("_EmissiveColor", color * intensity);
        }
    }

    void OnDestroy()
    {
        foreach (var cons in _meshes) cons.Dispose();
        _meshes.Clear();
    }
}
