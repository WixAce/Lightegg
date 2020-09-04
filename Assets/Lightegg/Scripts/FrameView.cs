using UnityEngine;

public class FrameView : MonoBehaviour {
	FrameObject _frameObject;

	public string forceUrl;

	public static string Url      { get; set; }
	public static string Uri      { get; set; }
	public static bool   IsHybrid { get; set; }

	void Start() {
		Init();
		if (!string.IsNullOrEmpty(forceUrl))
			_frameObject.LoadURL(forceUrl);
		else
			_frameObject.LoadURL(IsHybrid ? Uri : Url);
		_frameObject.SetVisibility(true);
	}

	void Init() {
		_frameObject = (new GameObject("WebViewObject")).AddComponent<FrameObject>();
		_frameObject.Init(
		                    cb: (msg) => { Debug.Log(string.Format("CallFromJS[{0}]", msg)); },
		                    err: (msg) => { Debug.Log(string.Format("CallOnError[{0}]", msg)); },
		                    started: (msg) => { Debug.Log(string.Format("CallOnStarted[{0}]", msg)); },
		                    ld: (msg) => { _frameObject.SetVisibility(true); }, enableWKWebView: true,
		                    transparent: true);
	}
}