using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AndroidTenjin : BaseTenjin {

	private const string AndroidJavaTenjinClass = "com.tenjin.android.TenjinSDK";
	private const string AndroidJavaTenjinAppStoreType = "com.tenjin.android.TenjinSDK$AppStoreType";

#if UNITY_ANDROID && !UNITY_EDITOR
	private AndroidJavaObject tenjinJava = null;
	private AndroidJavaObject activity = null;

	public override void Init(string apiKey){
		if (Debug.isDebugBuild) {
            Debug.Log ("Android Initializing - v"+this.SdkVersion);
		}
		ApiKey = apiKey;

        SetUnityVersionInNativeSDK();

		initActivity();
		AndroidJavaClass sdk = new AndroidJavaClass (AndroidJavaTenjinClass);
		if (sdk == null){
			throw new MissingReferenceException(
				string.Format("AndroidTenjin failed to load {0} class", AndroidJavaTenjinClass)
			);
		}
		tenjinJava = sdk.CallStatic<AndroidJavaObject> ("getInstance", activity, apiKey);
	}

	public override void InitWithSharedSecret(string apiKey, string sharedSecret){
		if (Debug.isDebugBuild) {
            Debug.Log("Android Initializing with Shared Secret - v"+this.SdkVersion);
		}
		ApiKey = apiKey;
		SharedSecret = sharedSecret;

        SetUnityVersionInNativeSDK();

		initActivity();
		AndroidJavaClass sdk = new AndroidJavaClass (AndroidJavaTenjinClass);
		if (sdk == null){
			throw new MissingReferenceException(
				string.Format("AndroidTenjin failed to load {0} class", AndroidJavaTenjinClass)
			);
		}
		tenjinJava = sdk.CallStatic<AndroidJavaObject> ("getInstanceWithSharedSecret", activity, apiKey, sharedSecret);
	}

	public override void InitWithAppSubversion(string apiKey, int appSubversion){
		if (Debug.isDebugBuild) {
            Debug.Log("Android Initializing with App Subversion: " + appSubversion + " v" +this.SdkVersion);
		}
		ApiKey = apiKey;
		AppSubversion = appSubversion;

        SetUnityVersionInNativeSDK();

		initActivity();
		AndroidJavaClass sdk = new AndroidJavaClass (AndroidJavaTenjinClass);
		if (sdk == null){
			throw new MissingReferenceException(
				string.Format("AndroidTenjin failed to load {0} class", AndroidJavaTenjinClass)
			);
		}
		tenjinJava = sdk.CallStatic<AndroidJavaObject> ("getInstanceWithAppSubversion", activity, apiKey, appSubversion);
		tenjinJava.Call ("appendAppSubversion", new object[]{appSubversion});
	}

	public override void InitWithSharedSecretAppSubversion(string apiKey, string sharedSecret, int appSubversion){
		if (Debug.isDebugBuild) {
            Debug.Log("Android Initializing with Shared Secret + App Subversion: " + appSubversion +" v" +this.SdkVersion);
		}
		ApiKey = apiKey;
		SharedSecret = sharedSecret;
		AppSubversion = appSubversion;

		SetUnityVersionInNativeSDK();

		initActivity();
		AndroidJavaClass sdk = new AndroidJavaClass (AndroidJavaTenjinClass);
		if (sdk == null){
			throw new MissingReferenceException(
				string.Format("AndroidTenjin failed to load {0} class", AndroidJavaTenjinClass)
			);
		}
		tenjinJava = sdk.CallStatic<AndroidJavaObject> ("getInstanceWithSharedSecretAppSubversion", activity, apiKey, sharedSecret, appSubversion);
		tenjinJava.Call ("appendAppSubversion", new object[]{appSubversion});
	}

    private void SetUnityVersionInNativeSDK() {
		var unitySdkVersion = this.SdkVersion + "u";

		AndroidJavaClass sdk = new AndroidJavaClass (AndroidJavaTenjinClass);
		if (sdk == null){
			throw new MissingReferenceException(
				string.Format("AndroidTenjin failed to load {0} class", AndroidJavaTenjinClass)
			);
		}

		sdk.CallStatic("setWrapperVersion", unitySdkVersion);
    }

	private void initActivity(){
		AndroidJavaClass javaContext = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		activity = javaContext.GetStatic<AndroidJavaObject>("currentActivity");
	}

	public override void Connect() {
		string optInOut = null;
		if (optIn) {
			optInOut = "optin";
		}
		else if (optOut) {
			optInOut = "optout";
		}
		object[] args = new object[]{null, optInOut};
		tenjinJava.Call ("connect", args);
	}

	public override void Connect(string deferredDeeplink){
		string optInOut = null;
		if (optIn) {
			optInOut = "optin";
		}
		else if (optOut) {
			optInOut = "optout";
		}
		object[] args = new object[]{deferredDeeplink, optInOut};
		tenjinJava.Call ("connect", args);
	}

	//SendEvent accepts a single eventName as a String
	public override void SendEvent (string eventName){
		object[] args = new object[]{eventName};
		tenjinJava.Call ("eventWithName", args);
	}

	//SendEvent accepts eventName as a String and eventValue as a String
	public override void SendEvent (string eventName, string eventValue){
		object[] args = new object[]{eventName, eventValue};
		tenjinJava.Call ("eventWithNameAndValue", args);
	}

	public override void Transaction(string productId, string currencyCode, int quantity, double unitPrice, string transactionId, string receipt, string signature){

		transactionId = null;
		//if the receipt and signature have values then try to validate. if there are no values then manually log the transaction.
		if(receipt != null && signature != null){
			object[] receiptArgs = new object[]{productId, currencyCode, quantity, unitPrice, receipt, signature};
			if (Debug.isDebugBuild) {
				Debug.Log ("Android Transaction " + productId + ", " + currencyCode + ", " + quantity + ", " + unitPrice + ", " + receipt + ", " + signature);
			}		
			tenjinJava.Call ("transaction", receiptArgs);
		}
		else{
			object[] args = new object[]{productId, currencyCode, quantity, unitPrice};
			if (Debug.isDebugBuild) {
				Debug.Log ("Android Transaction " + productId + ", " + currencyCode + ", " + quantity + ", " + unitPrice);
			}
			tenjinJava.Call ("transaction", args);
		}
	}

	public override void GetDeeplink(Tenjin.DeferredDeeplinkDelegate deferredDeeplinkDelegate) {
		DeferredDeeplinkListener onDeferredDeeplinkListener = new DeferredDeeplinkListener(deferredDeeplinkDelegate);
		tenjinJava.Call ("getDeeplink", onDeferredDeeplinkListener);
	}

	private class DeferredDeeplinkListener : AndroidJavaProxy {
		private Tenjin.DeferredDeeplinkDelegate callback;

		public DeferredDeeplinkListener(Tenjin.DeferredDeeplinkDelegate deferredDeeplinkCallback) : base("com.tenjin.android.Callback") {
			this.callback = deferredDeeplinkCallback;
		}

		public void onSuccess(bool clickedTenjinLink, bool isFirstSession, AndroidJavaObject data) {
			Dictionary<string, string> deeplinkData = new Dictionary<string, string>();
			string adNetwork = data.Call<string>("get", "ad_network");
			string advertisingId = data.Call<string>("get", "advertising_id");
			string campaignId = data.Call<string>("get", "campaign_id");
			string campaignName = data.Call<string>("get", "campaign_name");
			string deferredDeeplink = data.Call<string>("get", "deferred_deeplink_url");
			string referrer = data.Call<string>("get", "referrer");
			string siteId = data.Call<string>("get", " site_id");

			if (!string.IsNullOrEmpty(adNetwork)) {
				deeplinkData["ad_network"] = adNetwork;
			}
			if (!string.IsNullOrEmpty(advertisingId)) {
				deeplinkData["advertising_id"] = advertisingId;
			}
			if (!string.IsNullOrEmpty(campaignId)) {
				deeplinkData["campaign_id"] = campaignId;
			}
			if (!string.IsNullOrEmpty(campaignName)) {
				deeplinkData["campaign_name"] = campaignName;
			}
			if (!string.IsNullOrEmpty(deferredDeeplink)) {
				deeplinkData["deferred_deeplink_url"] = deferredDeeplink;
			}
			if (!string.IsNullOrEmpty(referrer)) {
				deeplinkData["referrer"] = referrer;
			}
			if (!string.IsNullOrEmpty(siteId)) {
				deeplinkData["site_id"] = siteId;
			}

			deeplinkData.Add("clicked_tenjin_link", Convert.ToString(clickedTenjinLink));
			deeplinkData.Add("is_first_session", Convert.ToString(isFirstSession));

			callback(deeplinkData);
		}
	}

	public override void OptIn(){
		optIn = true;
		tenjinJava.Call ("optIn");
	}

	public override void OptOut(){
		optOut = true;
		tenjinJava.Call ("optOut");
	}

	public override void OptInParams(List<string> parameters){
		tenjinJava.Call ("optInParams", new object[] {parameters.ToArray()});
	}

	public override void OptOutParams(List<string> parameters){
		tenjinJava.Call ("optOutParams", new object[] {parameters.ToArray()});
	}

	public override void RegisterAppForAdNetworkAttribution(){
	}

	public override void UpdateConversionValue(int conversionValue){
		if (Debug.isDebugBuild) {
			Debug.Log ("Android UpdateConversionValue");
		}
		object[] args = new object[]{conversionValue};
		tenjinJava.Call ("updateConversionValue", args);
	}

	public override void RequestTrackingAuthorizationWithCompletionHandler(Action<int> trackingAuthorizationCallback) {
	}

	public override void AppendAppSubversion (int appSubversion){
		object[] args = new object[]{appSubversion};
		tenjinJava.Call ("appendAppSubversion", args);
	}

	public static AndroidJavaObject CreateJavaMapFromDictainary(IDictionary<string, string> parameters){
		AndroidJavaObject javaMap = new AndroidJavaObject("java.util.HashMap");
		IntPtr putMethod = AndroidJNIHelper.GetMethodID(
			javaMap.GetRawClass(), "put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");

		object[] args = new object[2];
		foreach (KeyValuePair<string, string> kvp in parameters){
			using (AndroidJavaObject k = new AndroidJavaObject("java.lang.String", kvp.Key)){
				using (AndroidJavaObject v = new AndroidJavaObject("java.lang.String", kvp.Value)){
					args[0] = k;
					args[1] = v;
					AndroidJNI.CallObjectMethod(javaMap.GetRawObject(),
						putMethod, AndroidJNIHelper.CreateJNIArgArray(args));
				}
			}
		}
    	return javaMap;
    }

	public override void DebugLogs(){
		Debug.Log ("Debug logs not implemented on android");
	}

    public override void SubscribeMoPubImpressions()
    {
        Debug.Log("Subscribing to Mopub ILRD");
        TenjinMopubIntegration.ListenForImpressions(ImpressionHandler);
    }

    private void ImpressionHandler(string json)
    {
        Debug.Log($"Got ILRD impression data {json}");
        var args = new object[] {json};
        tenjinJava.Call ("eventAdImpressionMoPub", args);
    }

	public override void SetAppStoreType (AppStoreType appStoreType){
		object[] args = new object[]{appStoreType};
		AndroidJavaClass appStoreTypeClass = new AndroidJavaClass(AndroidJavaTenjinAppStoreType); 
		if (appStoreTypeClass != null){
			AndroidJavaObject tenjinAppStoreType = appStoreTypeClass.GetStatic<AndroidJavaObject>(appStoreType.ToString());
			if (tenjinAppStoreType != null) {
				tenjinJava.Call ("setAppStore", tenjinAppStoreType);
			}
		}
	}
#else
	public override void Init(string apiKey){
		Debug.Log ("Android Initializing - v"+this.SdkVersion);
		ApiKey = apiKey;
	}

	public override void InitWithSharedSecret(string apiKey, string sharedSecret)
	{
		Debug.Log("Android Initializing with Shared Secret - v"+this.SdkVersion);
		ApiKey = apiKey;
		SharedSecret = sharedSecret;
	}

	public override void InitWithAppSubversion(string apiKey, int appSubversion)
	{
		Debug.Log("Android Initializing with App Subversion: " + appSubversion + " v" +this.SdkVersion);
		ApiKey = apiKey;
		AppSubversion = appSubversion;
	}

	public override void InitWithSharedSecretAppSubversion(string apiKey, string sharedSecret, int appSubversion)
	{
		Debug.Log("Android Initializing with Shared Secret + App Subversion: " + appSubversion +" v" +this.SdkVersion);
		ApiKey = apiKey;
		SharedSecret = sharedSecret;
		AppSubversion = appSubversion;
	}

	public override void Connect(){
		Debug.Log ("Android Connecting");
	}

	public override void Connect(string deferredDeeplink){
		Debug.Log ("Android Connecting with deferredDeeplink " + deferredDeeplink);
	}

	public override void SendEvent (string eventName){
		Debug.Log ("Android Sending Event " + eventName);
	}

	public override void SendEvent (string eventName, string eventValue){
		Debug.Log ("Android Sending Event " + eventName + " : " + eventValue);
	}

	public override void Transaction(string productId, string currencyCode, int quantity, double unitPrice, string transactionId, string receipt, string signature){
		Debug.Log ("Android Transaction " + productId + ", " + currencyCode + ", " + quantity + ", " + unitPrice + ", " + transactionId + ", " + receipt + ", " + signature);
	}

	public override void GetDeeplink(Tenjin.DeferredDeeplinkDelegate deferredDeeplinkDelegate) {
		Debug.Log ("Sending AndroidTenjin::GetDeeplink");
	}

	public override void OptIn(){
		Debug.Log ("Sending AndroidTenjin::OptIn");
	}

	public override void OptOut(){
		Debug.Log ("Sending AndroidTenjin::OptOut");
	}

	public override void OptInParams(List<string> parameters){
		Debug.Log ("Sending AndroidTenjin::OptInParams");
	}

	public override void OptOutParams(List<string> parameters){
		Debug.Log ("Sending AndroidTenjin::OptOutParams");
	}

	public override void AppendAppSubversion(int subversion){
		Debug.Log("Sending AndroidTenjin::AppendAppSubversion :" + subversion);
	}

    public override void SubscribeMoPubImpressions()
    {
        Debug.Log("Sending AndroidTenjin:: SubscribeMoPubImpressions " );
    }
    public override void DebugLogs(){
	    Debug.Log ("Setting debug logs ");
    }

	public override void UpdateConversionValue(int conversionValue)
	{
		Debug.Log("Sending UpdateConversionValue: " + conversionValue);
	}

	public override void RegisterAppForAdNetworkAttribution()
	{
		throw new NotImplementedException();
	}

	public override void RequestTrackingAuthorizationWithCompletionHandler(Action<int> trackingAuthorizationCallback)
	{
		throw new NotImplementedException();
	}

	public override void SetAppStoreType(AppStoreType appStoreType) {
		Debug.Log("Setting AndroidTenjin::SetAppStoreType: " + appStoreType);
	}
#endif
}
