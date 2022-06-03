//
//  Copyright (c) 2022 Tenjin. All rights reserved.
//

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

    private void ImpressionHandler(string json)
    {
        Debug.Log($"Got impression data {json}");
    }

	public override void SubscribeAppLovinImpressions()
	{
		Debug.Log("Subscribing to applovin impressions");
		TenjinAppLovinIntegration.ListenForImpressions(AppLovinImpressionHandler);
	}

	private void AppLovinImpressionHandler(string json)
	{
		Debug.Log($"Got applovin impression data {json}");
	}

	public override void SubscribeIronSourceImpressions()
	{
		Debug.Log("Subscribing to ironsource impressions");
		TenjinIronSourceIntegration.ListenForImpressions(IronSourceImpressionHandler);
	}

	private void IronSourceImpressionHandler(string json)
	{
		Debug.Log($"Got ironsource impression data {json}");
	}

	public override void SubscribeHyperBidImpressions()
	{
		Debug.Log("Subscribing to hyperbid impressions");
		TenjinHyperBidIntegration.ListenForImpressions(HyperBidImpressionHandler);
	}

	private void HyperBidImpressionHandler(string json)
	{
		Debug.Log($"Got hyperbid impression data {json}");
	}

	public override void SubscribeAdMobBannerViewImpressions(object bannerView, string adUnitId)
	{
		Debug.Log("Subscribing to admob bannerView impressions");
		TenjinAdMobIntegration.ListenForBannerViewImpressions(bannerView, adUnitId, AdMobBannerViewImpressionHandler);
	}

	public override void SubscribeAdMobRewardedAdImpressions(object rewardedAd, string adUnitId)
	{
		Debug.Log("Subscribing to admob rewardedAd impressions");
		TenjinAdMobIntegration.ListenForRewardedAdImpressions(rewardedAd, adUnitId, AdMobRewardedAdImpressionHandler);
	}
	
	public override void SubscribeAdMobInterstitialAdImpressions(object interstitialAd, string adUnitId)
	{
		Debug.Log("Subscribing to admob interstitialAd impressions");
		TenjinAdMobIntegration.ListenForInterstitialAdImpressions(interstitialAd, adUnitId, AdMobInterstitialAdImpressionHandler);
	}

	public override void SubscribeAdMobRewardedInterstitialAdImpressions(object rewardedInterstitialAd, string adUnitId)
	{
		Debug.Log("Subscribing to admob rewardedInterstitialAd impressions");
		TenjinAdMobIntegration.ListenForRewardedInterstitialAdImpressions(rewardedInterstitialAd, adUnitId, AdMobRewardedInterstitialAdImpressionHandler);
	}

	private void AdMobBannerViewImpressionHandler(string json)
	{
		Debug.Log($"Got admob bannerView impression data {json}");
	}

	private void AdMobRewardedAdImpressionHandler(string json)
	{
		Debug.Log($"Got admob rewardedAd impression data {json}");
	}

	private void AdMobInterstitialAdImpressionHandler(string json)
	{
		Debug.Log($"Got admob interstitialAd impression data {json}");
	}

	private void AdMobRewardedInterstitialAdImpressionHandler(string json)
	{
		Debug.Log($"Got admob rewardedInterstitialAd impression data {json}");
	}

	public override void RegisterAppForAdNetworkAttribution()
    {
		Debug.Log("RegisterAppForAdNetworkAttribution");
	}

	public override void UpdateConversionValue(int conversionValue)
	{
		Debug.Log("UpdateConversionValue: " + conversionValue);
	}

	public override void SubscribeTopOnImpressions()
	{
		Debug.Log("Subscribing to topon impressions");
		TenjinTopOnIntegration.ListenForImpressions(TopOnImpressionHandler);
	}

	private void TopOnImpressionHandler(string json)
	{
		Debug.Log($"Got topon impression data {json}");
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
