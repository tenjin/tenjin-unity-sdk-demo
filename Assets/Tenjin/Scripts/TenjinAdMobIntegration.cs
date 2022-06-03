//
//  Copyright (c) 2022 Tenjin. All rights reserved.
//

using System;
using System.Collections.Generic;
using UnityEngine;

public class TenjinAdMobIntegration
{
    private static bool _subscribed_admob = false;
    public TenjinAdMobIntegration()
    {
    }
    public static void ListenForBannerViewImpressions(object bannerView, string adUnitId, Action<string> callback)
    {
#if tenjin_admob_enabled
        if (_subscribed_admob)
        {
            Debug.Log("Ignoring duplicate admob bannerView subscription");
            return;
        }
        GoogleMobileAds.Api.BannerView newBannerView = (GoogleMobileAds.Api.BannerView) bannerView;        
        newBannerView.OnPaidEvent += (sender, args) =>
        {
            ResponseInfo responseInfo = newBannerView.GetResponseInfo();
            if (responseInfo != null)
            {
                String adResponseId = responseInfo.GetResponseId();
                try
                {
                    AdMobImpressionDataToJSON adMobImpressionDataToJSON = new AdMobImpressionDataToJSON()
                    {
                        ad_unit_id = adUnitId,
                        #if UNITY_ANDROID
                            value_micros = args.AdValue.Value,
                        #elif UNITY_IPHONE
                            value_micros = (args.AdValue.Value / 1000000.0),
                        #else
                            value_micros = args.AdValue.Value,
                        #endif
                        currency_code = args.AdValue.CurrencyCode,
                        response_id = adResponseId,
                        precision_type = args.AdValue.Precision.ToString(),
                        mediation_adapter_class_name = responseInfo.GetMediationAdapterClassName()
                    };
                    string json = JsonUtility.ToJson(adMobImpressionDataToJSON);
                    callback(json);
                }
                catch (Exception ex)
                {
                    Debug.Log($"error parsing bannerView impression " + ex.ToString());
                }
            }
        };
        _subscribed_admob = true;
#endif
    }
    public static void ListenForRewardedAdImpressions(object rewardedAd, string adUnitId, Action<string> callback)
    {
#if tenjin_admob_enabled
        if (_subscribed_admob)
        {
            Debug.Log("Ignoring duplicate admob rewardedAd subscription");
            return;
        }
        GoogleMobileAds.Api.RewardedAd newRewardedAd = (GoogleMobileAds.Api.RewardedAd) rewardedAd;
        newRewardedAd.OnPaidEvent += (sender, args) =>
        {
            ResponseInfo responseInfo = newRewardedAd.GetResponseInfo();
            if (responseInfo != null)
            {
                String adResponseId = responseInfo.GetResponseId();
                try
                {
                    AdMobImpressionDataToJSON adMobImpressionDataToJSON = new AdMobImpressionDataToJSON()
                    {
                        ad_unit_id = adUnitId,
                        #if UNITY_ANDROID
                            value_micros = args.AdValue.Value,
                        #elif UNITY_IPHONE
                            value_micros = (args.AdValue.Value / 1000000.0),
                        #else
                            value_micros = args.AdValue.Value,
                        #endif
                        currency_code = args.AdValue.CurrencyCode,
                        response_id = adResponseId,
                        precision_type = args.AdValue.Precision.ToString(),
                        mediation_adapter_class_name = responseInfo.GetMediationAdapterClassName()
                    };
                    string json = JsonUtility.ToJson(adMobImpressionDataToJSON);
                    callback(json);
                }
                catch (Exception ex)
                {
                    Debug.Log($"error parsing rewardedAd impression " + ex.ToString());
                }
            }
        };
        _subscribed_admob = true;
#endif
    }
    public static void ListenForInterstitialAdImpressions(object interstitialAd, string adUnitId, Action<string> callback)
    {
#if tenjin_admob_enabled
        if (_subscribed_admob)
        {
            Debug.Log("Ignoring duplicate admob interstitialAd subscription");
            return;
        }
        GoogleMobileAds.Api.InterstitialAd newInterstitialAd = (GoogleMobileAds.Api.InterstitialAd) interstitialAd;
        newInterstitialAd.OnPaidEvent += (sender, args) =>
        {
            ResponseInfo responseInfo = newInterstitialAd.GetResponseInfo();
            if (responseInfo != null)
            {
                String adResponseId = responseInfo.GetResponseId();
                try
                {
                    AdMobImpressionDataToJSON adMobImpressionDataToJSON = new AdMobImpressionDataToJSON()
                    {
                        ad_unit_id = adUnitId,
                        #if UNITY_ANDROID
                            value_micros = args.AdValue.Value,
                        #elif UNITY_IPHONE
                            value_micros = (args.AdValue.Value / 1000000.0),
                        #else
                            value_micros = args.AdValue.Value,
                        #endif
                        currency_code = args.AdValue.CurrencyCode,
                        response_id = adResponseId,
                        precision_type = args.AdValue.Precision.ToString(),
                        mediation_adapter_class_name = responseInfo.GetMediationAdapterClassName()
                    };
                    string json = JsonUtility.ToJson(adMobImpressionDataToJSON);
                    callback(json);
                }
                catch (Exception ex)
                {
                    Debug.Log($"error parsing interstitialAd impression " + ex.ToString());
                }
            }
        };
        _subscribed_admob = true;
#endif
    }

    public static void ListenForRewardedInterstitialAdImpressions(object rewardedInterstitialAd, string adUnitId, Action<string> callback)
    {
#if tenjin_admob_enabled
        if (_subscribed_admob)
        {
            Debug.Log("Ignoring duplicate admob rewardedInterstitialAd subscription");
            return;
        }
        GoogleMobileAds.Api.RewardedInterstitialAd newRewardedInterstitialAd = (GoogleMobileAds.Api.RewardedInterstitialAd) rewardedInterstitialAd;
        newRewardedInterstitialAd.OnPaidEvent += (sender, args) =>
        {
            ResponseInfo responseInfo = newRewardedInterstitialAd.GetResponseInfo();
            if (responseInfo != null)
            {
                String adResponseId = responseInfo.GetResponseId();
                try
                {
                    AdMobImpressionDataToJSON adMobImpressionDataToJSON = new AdMobImpressionDataToJSON()
                    {
                        ad_unit_id = adUnitId,
                        #if UNITY_ANDROID
                            value_micros = args.AdValue.Value,
                        #elif UNITY_IPHONE
                            value_micros = (args.AdValue.Value / 1000000.0),
                        #else
                            value_micros = args.AdValue.Value,
                        #endif
                        currency_code = args.AdValue.CurrencyCode,
                        response_id = adResponseId,
                        precision_type = args.AdValue.Precision.ToString(),
                        mediation_adapter_class_name = responseInfo.GetMediationAdapterClassName()
                    };
                    string json = JsonUtility.ToJson(adMobImpressionDataToJSON);
                    callback(json);
                }
                catch (Exception ex)
                {
                    Debug.Log($"error parsing rewardedInterstitialAd impression " + ex.ToString());
                }
            }
        };
        _subscribed_admob = true;
#endif
    }
}

[System.Serializable]
internal class AdMobImpressionDataToJSON
{
    public string currency_code;
    public string ad_unit_id;
    public string response_id;
    public string precision_type;
    public string mediation_adapter_class_name;
#if UNITY_IPHONE
    public double value_micros;
#elif UNITY_ANDROID
    public long value_micros;
#else
    public long value_micros;
#endif
}
