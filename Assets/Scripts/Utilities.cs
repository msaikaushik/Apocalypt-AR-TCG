using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    //=========================================================================
    // Removes all instances of [itemToRemove] from array [original]
    // Returns the new array, without modifying [original] directly
    // .Net2.0-compatible
    public static T[] RemoveFromArray<T>(this T[] original, T itemToRemove)
    {
        int numIdx = System.Array.IndexOf(original, itemToRemove);
        if (numIdx == -1) return original;
        List<T> tmp = new List<T>(original);
        tmp.RemoveAt(numIdx);
        return tmp.ToArray();
    }

    public static GameObject FindObject(this GameObject parent, string name)
    {
        Transform[] trs = parent.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in trs)
        {
            if (t.name == name)
            {
                return t.gameObject;
            }
        }
        return null;
    }
}
