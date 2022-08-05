using UnityEngine;

namespace Sketch0731 {

static class Util
{
    public static void DestroyObject(Object o)
    {
        if (o == null) return;
        if (Application.isPlaying)
            Object.Destroy(o);
        else
            Object.DestroyImmediate(o);
    }
}

} // namespace Sketch0731
