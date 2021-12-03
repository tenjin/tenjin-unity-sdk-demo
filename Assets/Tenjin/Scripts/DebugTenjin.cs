using UnityEngine;
using System;
using System.Collections.Generic;

public class DebugTenjin : BaseTenjin {

	public override void Connect(){
		Debug.Log ("Connecting " + ApiKey);
	}

	public override void Connect(string deferredDeeplink){
		Debug.Log ("Connecting with deferredDeeplink " + deferredDeeplink);
	}

	public override void Init(string apiKey){
		Debug.Log ("Initializing - v"+this.SdkVersion);
	}

	public override void InitWithSharedSecret(string apiKey, string sharedSecret)
	{
		Debug.Log("Initializing with Shared Secret - v"+this.SdkVersion);
	}

	public override void InitWithAppSubversion(string apiKey, int appSubversion)
	{
		Debug.Log("Initializing with App Subversion: " + appSubversion + " v" +this.SdkVersion);
	}

	public override void InitWithSharedSecretAppSubversion(string apiKey, string sharedSecret, int appSubversion)
	{
		Debug.Log("Initializing with Shared Secret + App Subversion: " + appSubversion +" v" +this.SdkVersion);
	}

	public override void SendEvent (string eventName){
		Debug.Log ("Sending Event " + eventName);
	}

	public override void SendEvent (string eventName, string eventValue){
		Debug.Log ("Sending Event " + eventName + " : " + eventValue);
	}

	public override void Transaction(string productId, string currencyCode, int quantity, double unitPrice, string transactionId, string receipt, string signature){
		Debug.Log ("Transaction " + productId + ", " + currencyCode + ", " + quantity + ", " + unitPrice + ", " + transactionId + ", " + receipt + ", " + signature);
	}

	public override void GetDeeplink(Tenjin.DeferredDeeplinkDelegate deferredDeeplinkDelegate) {
		Debug.Log ("Sending DebugTenjin::GetDeeplink");
	}

	public override void OptIn(){
		Debug.Log ("OptIn ");
	}

	public override void OptOut(){
		Debug.Log ("OptOut ");
	}

	public override void OptInParams(List<string> parameters){
		Debug.Log ("OptInParams");
	}

	public override void OptOutParams(List<string> parameters){
		Debug.Log ("OptOutParams" );
	}

	public override void DebugLogs(){
		Debug.Log ("Setting debug logs ");
	}

	public override void AppendAppSubversion(int subversion)
	{
		Debug.Log("AppendAppSubversion: " + subversion);
	}

    public override void SubscribeMoPubImpressions()
    {
        Debug.Log("Subscribing to mopub impressions");
        TenjinMopubIntegration.ListenForImpressions(ImpressionHandler);
    }

    private void ImpressionHandler(string json)
    {
        Debug.Log($"Got impression data {json}");
    }

	public override void RegisterAppForAdNetworkAttribution()
    {
		Debug.Log("RegisterAppForAdNetworkAttribution");
	}

	public override void UpdateConversionValue(int conversionValue)
	{
		Debug.Log("UpdateConversionValue: " + conversionValue);
	}

	public override void RequestTrackingAuthorizationWithCompletionHandler(Action<int> trackingAuthorizationCallback)
    {
		Debug.Log("RequestTrackingAuthorizationWithCompletionHandler");
	}

	public override void SetAppStoreType(AppStoreType appStoreType)
	{
		Debug.Log("SetAppStoreType");
	}
}
