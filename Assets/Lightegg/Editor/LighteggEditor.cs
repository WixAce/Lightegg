using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LighteggEditor : Editor
{
    
    [MenuItem("Lightegg/Clear Cache",false,1000)]
    static void ClearCache() =>Caching.ClearCache();
    
}
