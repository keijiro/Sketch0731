using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Sketch0731 {

[ExecuteInEditMode]
sealed class Node : MonoBehaviour
{
    #region Public factory method

    public static Node Create(Transform parent, Material material)
    {
        // Create as a "don't save" object.
        var go = new GameObject("Node", ComponentList);
        go.hideFlags = HideFlags.DontSave;

        // Parenting
        go.transform.parent = parent;

        // Reference to the instance
        var node = go.GetComponent<Node>();

        // Material copy
        node._material = new Material(material);
        node._material.hideFlags = HideFlags.DontSave;
        go.GetComponent<MeshRenderer>().sharedMaterial = node._material;

        return node;
    }

    #endregion

    #region Public methods

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

    #endregion

    #region Private objects

    static System.Type [] ComponentList
      = new [] { typeof(MeshFilter), typeof(MeshRenderer), typeof(Node) };

    Modeler _modeler;
    Material _material;

    #endregion

    #region MonoBehavior implementation

    void OnDestroy()
    {
        _modeler?.Dispose();
        _modeler = null;

        Util.DestroyObject(_material);
        _material = null;
    }

    #endregion

    #region Mesh operation

    void BuildMesh(Config config, int index)
    {
        // Local random number generator
        var rand = new Random(config.Seed + (uint)index);
        rand.NextUInt4();

        // Modeler setup
        _modeler?.Dispose();
        _modeler = new Modeler(config.Resolution)
          { Width = config.Width,
            Length = config.Length,
            Frequency = rand.NextFloat(config.Frequency.x, config.Frequency.y),
            Phase = rand.NextFloat(0, config.Bias) };

        // Mesh construction
        _modeler.InitializeIndexBuffer();
        _modeler.InitializeVertexBuffer();
        _modeler.ConstructMesh();
        GetComponent<MeshFilter>().sharedMesh = _modeler.SharedMesh;

        // Coloring
        var color = Color.HSVToRGB(rand.NextFloat(), 1, 1);
        var burst = rand.NextFloat() < config.BurstProbability;
        var intensity = burst ? config.BurstIntensity : 0;
        _material.SetColor("_BaseColor", color);
        _material.SetColor("_EmissiveColor", color * intensity);

        // Node placement
        transform.localPosition = new Vector3(index * config.Width, 0, 0);
        transform.localRotation = Quaternion.identity;
    }

    #endregion
}

} // namespace Sketch0731
