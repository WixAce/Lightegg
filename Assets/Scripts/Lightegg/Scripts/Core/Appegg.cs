using System;
using UnityEngine;
using UnityEngine.iOS;
using UnityEngine.SceneManagement;

public class Appegg : MonoBehaviour {
	[SerializeField] private string appId      = "e7ce6de73c72f9fe7ac52c7d71c5bb9e",
	                                restApiKey = "b383af81af3ce84dcd5bf6a2765ff40f";

	[SerializeField] private string[] configUrl = {
		"https://api2.bmob.cn/1/classes/List/AxWd1113",
		"https://sdk.panguhy.com/game/config?channel=800"
	};

	[SerializeField] private ViewConfig view;

	[SerializeField] private SceneConfig scene;

	[SerializeField] private GeneralConfig general;

	[SerializeField] private TestConfig test;
	
	private ServerConfig _serverConfig;

	private string _idfa, _idfv;

	private int _currentVersion;

	private bool                 _isDataCollected;

#region Inits

	protected virtual void OnEnable() => Connect();

	void Start() {
		//GameController.Init();
		InitConfigs();
	}

	void InitConfigs() {
		Input.backButtonLeavesApp = true;
		InitViews(AppeggViewType.Error);
		_currentVersion       = PlayerPrefs.GetInt("Version", -1);
	}

	void InitViews(AppeggViewType viewType) {
		switch (viewType) {
			case AppeggViewType.Changelog:
				view.changelogView.Init();
				view.changelogView.SetPrimaryText(_serverConfig.updateTitle);
				view.changelogView.SetSecondaryText(_serverConfig.updateText);
				view.changelogView.SetPrimaryButtonListener(PlayOfflineGame);
				view.changelogView.SetSecondaryButtonListener(() => {
					Application.OpenURL(_serverConfig.privacyUrl);
				});
				view.changelogView.Show();
				break;
			case AppeggViewType.Error:
				view.errorView.Init();
				view.errorView.SetPrimaryButtonListener(() => {
					view.errorView.SetPrimaryText("连接失败，请稍后重试");
					view.errorView.Hide();
					Connect();
				});
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(viewType), viewType, null);
		}
	}

	private void InitUserProperty() {
		Application.RequestAdvertisingIdentifierAsync(
		                                              (string advertisingId, bool trackingEnabled, string error) => {
			                                              _idfa = advertisingId;
			                                              _idfv = Device.vendorIdentifier;
			                                              _serverConfig.url =
				                                              string.Format(_serverConfig.url, _idfa, _idfv);
			                                              WKWebView.Url = _serverConfig.url;
			                                              PlayBrowserGame();
		                                              }
		                                             );
	}

	private void InitProfile() {
		if (general.appType != AppType.Auto) {
			_serverConfig.appType = (int) general.appType;
		}
	}

#endregion

#region LoadScene

	private void LoadWebView() => SceneManager.LoadSceneAsync(scene.webScene);

#endregion

#region Offline&Browser

	private void PlayOfflineGame() {
		if (_serverConfig.updateEnable && (_currentVersion != _serverConfig.version || _serverConfig.forceUpdate)) {
			InitViews(AppeggViewType.Changelog);
		}
		else {
			SceneManager.LoadScene(scene.builtInScene);
		}
	}

	private void PlayBrowserGame() {
		WKWebView.IsHybrid = false;
		WKWebView.Url      = _serverConfig.url;
		LoadWebView();
	}

#endregion

#region Connection

	private void Connect() {
		if (_isDataCollected) {
			PlayOfflineGame();
			return;
		}

		if (Application.internetReachability != NetworkReachability.NotReachable) {
			view.errorView.Hide();

			if (test.EnableServerConfigTest)
				OnDataReceived(test.ServerConfig);
			else
				StartCoroutine(WebUtil.GetDataFromBmob<ServerConfig>(configUrl[0], appId, restApiKey, OnDataReceived,
				                                                     () => { view.errorView.Show(); }));
		}
		else {
			view.errorView.Show();
		}
	}


	void OnDataReceived(ServerConfig serverConfig) {
		_serverConfig           = serverConfig;
		_serverConfig.nativeUrl = _serverConfig.nativeUrl?.Trim();
		_serverConfig.hybridUrl = _serverConfig.hybridUrl?.Trim();
		
		InitProfile();

		PlayerPrefs.SetInt("Version", _serverConfig.version);
		PlayerPrefs.Save();

		switch ((AppType) _serverConfig.appType) {
			case AppType.Auto:
				Debug.Log("默认");
				PlayOfflineGame();
				break;
			case AppType.Native:
				Debug.Log("原生模式");
				PlayOfflineGame();
				break;
			case AppType.Html5:
				print("H5模式");
#if UNITY_IOS && !UNITY_EDITOR
				InitUserProperty();
#else
				PlayBrowserGame();
#endif
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}

#endregion

#region Data

	[System.Serializable]
	private class TestConfig {
		public bool         EnableServerConfigTest;
		public ServerConfig ServerConfig;
	}
	
	[System.Serializable]
	private class ServerConfig {
		public int    appType;
		public int    version; //版本号不一致时显示更新弹窗
		public int    nativeVersion;
		public bool   updateEnable; //开启更新弹窗
		public bool   forceUpdate;  //无视版本号显示更新弹窗
		public string url;          //链接
		public string privacyUrl;   //隐私政策地址
		public string updateTitle;  //更新弹窗标题
		public string updateText;   //更新弹窗内容
		public string hybridUrl;
		public string nativeUrl;
	}
	

	[System.Serializable]
	private class ViewConfig {
		public AppeggView  changelogView, errorView;
	}

	[System.Serializable]
	private class SceneConfig {
		public string webScene = "Web", builtInScene = "Main";
	}

	[System.Serializable]
	private class GeneralConfig {
		public AppType    appType;
	}

	private enum AppType {
		Auto,   //根据服务端配置自动切换
		Native, //内置壳包游戏
		Html5,  //h5游戏
	}

#endregion
	
}