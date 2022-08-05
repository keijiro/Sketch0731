using UnityEngine;
using Unity.Collections;
using Unity.Mathematics;

namespace Sketch0731 {

sealed class Modeler : System.IDisposable
{
    #region Model properties

    public float Width { get; set; } = 1;
    public float Length { get; set; } = 1;
    public float Frequency { get; set; } = 14;
    public float Phase { get; set; } = 0;

    #endregion

    #region Object reference property

    public Mesh SharedMesh { get; private set; }

    #endregion

    #region Private variables

    int _pointCount;
    (NativeArray<int> i, NativeArray<float3> v) _buffers;

    #endregion

    #region Private utility properties

    int IndexCount => (_pointCount - 1) * 3 * 6;
    int VertexCount => _pointCount * 6;

    #endregion

    #region Constructor / destructor

    public Modeler(int pointCount)
    {
        _pointCount = pointCount;

        _buffers.i =
          new NativeArray<int>(IndexCount, Allocator.Persistent,
                               NativeArrayOptions.UninitializedMemory);

        _buffers.v =
          new NativeArray<float3>(VertexCount, Allocator.Persistent,
                                  NativeArrayOptions.UninitializedMemory);
    }

    public void Dispose()
    {
        if (_buffers.i.IsCreated) _buffers.i.Dispose();
        if (_buffers.v.IsCreated) _buffers.v.Dispose();

        if (SharedMesh != null)
        {
            Util.DestroyObject(SharedMesh);
            SharedMesh = null;
        }
    }

    #endregion

    #region Mesh building methods

    public void InitializeIndexBuffer()
    {
        ref var buf = ref _buffers.i;
        var offs = 0;

        for (var i = 0; i < _pointCount - 1; i++)
        {
            var i1 = i * 3 * 2;
            var i2 = i1 + 3 * 2;

            for (var j = 0; j < 3; j++)
            {
                buf[offs++] = i1 + 0;
                buf[offs++] = i2 + 0;
                buf[offs++] = i1 + 1;

                buf[offs++] = i1 + 1;
                buf[offs++] = i2 + 0;
                buf[offs++] = i2 + 1;

                i1 += 2;
                i2 += 2;
            }
        }
    }

    public void InitializeVertexBuffer()
    {
        ref var buf = ref _buffers.v;
        var offs = 0;

        for (var i = 0; i < _pointCount; i++)
        {
            var p = (float)i / (_pointCount - 1);
            var wave = math.sin(p * Frequency + Phase);

            var x1 = -0.5f * Width;
            var x2 = +0.5f * Width;

            var y = (wave * 0.2f + 1) * (p / 2 + 0.5f);
            var z = (p - 0.5f) * Length;

            buf[offs++] = math.float3(x1, 0, z);
            buf[offs++] = math.float3(x1, y, z);

            buf[offs++] = math.float3(x1, y, z);
            buf[offs++] = math.float3(x2, y, z);

            buf[offs++] = math.float3(x2, y, z);
            buf[offs++] = math.float3(x2, 0, z);
        }
    }

    public void ConstructMesh()
    {
        SharedMesh = new Mesh();
        SharedMesh.SetVertices(_buffers.v);
        SharedMesh.SetIndices(_buffers.i, MeshTopology.Triangles, 0);
        SharedMesh.bounds = new Bounds(Vector3.zero, Vector3.one * 1000);
        SharedMesh.RecalculateNormals();
        SharedMesh.RecalculateBounds();
    }

    #endregion
}

} // namespace Sketch0731
