using UnityEngine;

public class WKWebView : MonoBehaviour {
	WebViewObject _webViewObject;

	public string forceUrl;

	public static string Url      { get; set; }
	public static string Uri      { get; set; }
	public static bool   IsHybrid { get; set; }

	void Start() {
		Init();
		if (!string.IsNullOrEmpty(forceUrl))
			_webViewObject.LoadURL(forceUrl);
		else
			_webViewObject.LoadURL(IsHybrid ? Uri : Url);
		_webViewObject.SetVisibility(true);
	}

	void Init() {
		_webViewObject = (new GameObject("WebViewObject")).AddComponent<WebViewObject>();
		_webViewObject.Init(
		                    cb: (msg) => { Debug.Log(string.Format("CallFromJS[{0}]", msg)); },
		                    err: (msg) => { Debug.Log(string.Format("CallOnError[{0}]", msg)); },
		                    started: (msg) => { Debug.Log(string.Format("CallOnStarted[{0}]", msg)); },
		                    ld: (msg) => { _webViewObject.SetVisibility(true); }, enableWKWebView: true,
		                    transparent: true);
	}
}