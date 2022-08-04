using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Sketch0731 {

sealed class Node : MonoBehaviour
{
    static System.Type [] ComponentList
      = new [] { typeof(MeshFilter), typeof(MeshRenderer), typeof(Node) };

    Modeler _modeler;

    public static Node Create(Transform parent, Material material)
    {
        var go = new GameObject("Node", ComponentList);
        go.transform.parent = parent;
        go.hideFlags = HideFlags.DontSave;
        go.GetComponent<MeshRenderer>().sharedMaterial = new Material(material);
        return go.GetComponent<Node>();
    }

    void OnDestroy()
      => _modeler?.Dispose();

    public void Activate(Config config, int index)
    {
        gameObject.SetActive(true);
        BuildMesh(config, index);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        _modeler?.Dispose();
        _modeler = null;
    }

    void BuildMesh(Config config, int index)
    {
        var rand = new Random(config.Seed + (uint)index);
        rand.NextUInt4();

        _modeler?.Dispose();
        _modeler = new Modeler(config.Resolution)
          { Width = config.Width, Length = config.Length,
            Frequency = rand.NextFloat(config.Frequency.x, config.Frequency.y),
            Phase = rand.NextFloat(0, config.Bias) };

        _modeler.InitializeIndexBuffer();
        _modeler.InitializeVertexBuffer();
        _modeler.ConstructMesh();

        GetComponent<MeshFilter>().sharedMesh = _modeler.SharedMesh;
        var material = GetComponent<MeshRenderer>().sharedMaterial;

        transform.localPosition = new Vector3(index * config.Width, 0, 0);
        transform.localRotation = Quaternion.identity;

        var color = Color.HSVToRGB(rand.NextFloat(), 1, 1);
        var intensity = rand.NextFloat() < 0.35f ? 200 : 0;
        material.SetColor("_BaseColor", color);
        material.SetColor("_EmissiveColor", color * intensity);
    }
}

} // namespace Sketch0731
