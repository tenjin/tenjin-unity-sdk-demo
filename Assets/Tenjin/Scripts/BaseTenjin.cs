//
//  Copyright (c) 2022 Tenjin. All rights reserved.
//

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class BaseTenjin : MonoBehaviour {

	protected string apiKey;
	protected string sharedSecret;
	protected bool optIn;
	protected bool optOut;
	protected int appSubversion;

    public string SdkVersion { get; } = "1.12.18";

	public string ApiKey{
		get{
			return this.apiKey;
		}
		set{
			this.apiKey = value;
		}
	}

	public string SharedSecret{
		get{
			return this.sharedSecret;
		}
		set{
			this.sharedSecret = value;
		}
	}

	public int AppSubversion{
		get{
			return this.appSubversion;
		}
		set{
			this.appSubversion = value;
		}
	}

	public abstract void Init(string apiKey);
	public abstract void InitWithSharedSecret(string apiKey, string sharedSecret);
	public abstract void InitWithAppSubversion(string apiKey, int appSubversion);
	public abstract void InitWithSharedSecretAppSubversion(string apiKey, string sharedSecret, int appSubversion);
	public abstract void Connect();
	public abstract void Connect(string deferredDeeplink);
	public abstract void OptIn();
	public abstract void OptOut();
	public abstract void OptInParams(List<string> parameters);
	public abstract void OptOutParams(List<string> parameters);
	public abstract void AppendAppSubversion(int subversion);
	public abstract void SendEvent (string eventName);
	public abstract void SendEvent (string eventName, string eventValue);
	public abstract void Transaction(string productId, string currencyCode, int quantity, double unitPrice, string transactionId, string receipt, string signature);
	public abstract void GetDeeplink(Tenjin.DeferredDeeplinkDelegate deferredDeeplinkDelegate);
	public abstract void RegisterAppForAdNetworkAttribution();
	public abstract void UpdateConversionValue(int conversionValue);
	public abstract void RequestTrackingAuthorizationWithCompletionHandler(Action<int> trackingAuthorizationCallback);
	public abstract void DebugLogs();
	public abstract void SetAppStoreType(AppStoreType appStoreType);
	public abstract void SubscribeAppLovinImpressions();
	public abstract void SubscribeIronSourceImpressions();
	public abstract void SubscribeHyperBidImpressions();
	public abstract void SubscribeAdMobBannerViewImpressions(object bannerView, string adUnitId);
	public abstract void SubscribeAdMobRewardedAdImpressions(object rewardedAd, string adUnitId);
	public abstract void SubscribeAdMobInterstitialAdImpressions(object interstitialAd, string adUnitId);
	public abstract void SubscribeAdMobRewardedInterstitialAdImpressions(object rewardedInterstitialAd, string adUnitId);
	public abstract void SubscribeTopOnImpressions();
}
