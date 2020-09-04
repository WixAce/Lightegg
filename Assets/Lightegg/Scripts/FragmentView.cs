using UnityEngine;

public class FragmentView : MonoBehaviour {
	FrameObject _fragementObject;

	public string forceUrl;

	public static string Url      { get; set; }
	public static string Uri      { get; set; }
	public static bool   IsHybrid { get; set; }

	void Start() {
		Init();
		if (!string.IsNullOrEmpty(forceUrl))
			_fragementObject.LoadURL(forceUrl);
		else
			_fragementObject.LoadURL(IsHybrid ? Uri : Url);
		_fragementObject.SetMargins(0,0,0,200);
		_fragementObject.SetVisibility(true);
	}

	void Init() {
		_fragementObject = (new GameObject("WebViewObject")).AddComponent<FrameObject>();
		_fragementObject.Init(
		                    cb: (msg) => { Debug.Log(string.Format("CallFromJS[{0}]", msg)); },
		                    err: (msg) => { Debug.Log(string.Format("CallOnError[{0}]", msg)); },
		                    started: (msg) => { Debug.Log(string.Format("CallOnStarted[{0}]", msg)); },
		                    ld: (msg) => { _fragementObject.SetVisibility(true); }, enableWKWebView: true,
		                    transparent: true);
	}
}