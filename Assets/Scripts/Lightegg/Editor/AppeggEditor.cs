using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AppeggEditor : Editor
{
    
    [MenuItem("Appegg/Clear Cache",false,1000)]
    static void ClearCache() =>Caching.ClearCache();
    
}
