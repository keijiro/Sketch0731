using UnityEngine;
using Random = Unity.Mathematics.Random;

sealed class Node : MonoBehaviour
{
    static System.Type [] ComponentList
      = new [] { typeof(MeshFilter), typeof(MeshRenderer), typeof(Node) };

    MeshConstructor _mesh;

    public static Node Create(Transform parent, Material material)
    {
        var go = new GameObject("Node", ComponentList);
        go.transform.parent = parent;
        go.hideFlags = HideFlags.DontSave;
        go.GetComponent<MeshRenderer>().sharedMaterial = new Material(material);
        return go.GetComponent<Node>();
    }

    void OnDestroy()
      => _mesh?.Dispose();

    public void Activate(Modeling modeling, int index)
    {
        gameObject.SetActive(true);
        BuildMesh(modeling, index);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        _mesh?.Dispose();
        _mesh = null;
    }

    void BuildMesh(Modeling modeling, int index)
    {
        var rand = new Random(modeling.Seed + (uint)index);
        rand.NextUInt4();

        _mesh?.Dispose();
        _mesh = new MeshConstructor(modeling.Resolution)
          { Width = modeling.Width, Length = modeling.Length,
            Frequency = rand.NextFloat(modeling.Frequency.x, modeling.Frequency.y),
            Phase = rand.NextFloat(0, modeling.Bias) };

        _mesh.InitializeIndexBuffer();
        _mesh.InitializeVertexBuffer();
        _mesh.ConstructMesh();

        GetComponent<MeshFilter>().sharedMesh = _mesh.SharedMesh;
        var material = GetComponent<MeshRenderer>().sharedMaterial;

        transform.localPosition = new Vector3(index * modeling.Width, 0, 0);
        transform.localRotation = Quaternion.identity;

        var color = Color.HSVToRGB(rand.NextFloat(), 1, 1);
        var intensity = rand.NextFloat() < 0.35f ? 200 : 0;
        material.SetColor("_BaseColor", color);
        material.SetColor("_EmissiveColor", color * intensity);
    }
}
