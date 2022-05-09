//
//  Copyright (c) 2022 Tenjin. All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class TenjinIronSourceIntegration
{
    private static bool _subscribed_ironsource = false;
    public TenjinIronSourceIntegration()
    {
    }
    public static void ListenForImpressions(Action<string> callback)
    {
#if tenjin_ironsource_enabled
        if (_subscribed_ironsource)
        {
            Debug.Log("Ignoring duplicate ironsource subscription");
            return;
        }
        IronSourceEvents.onImpressionSuccessEvent += (arg1) =>
        {
            double parsedDoubleLifetimeRevenue = 0.0;
            double parsedDoubleRevenue = 0.0;
            int parsedIntConversionValue = 0;
            CultureInfo invCulture = CultureInfo.InvariantCulture;
            if (arg1.lifetimeRevenue != null && arg1.revenue != null) {
                double.TryParse(string.Format(invCulture, "{0}", arg1.lifetimeRevenue), NumberStyles.Any, invCulture, out parsedDoubleLifetimeRevenue);
                double.TryParse(string.Format(invCulture, "{0}", arg1.revenue), NumberStyles.Any, invCulture, out parsedDoubleRevenue);
                if (arg1.conversionValue != null) {
                    int.TryParse(string.Format(invCulture, "{0}", arg1.conversionValue), NumberStyles.Any, invCulture, out parsedIntConversionValue);  
                }
                try {
                    IronSourceAdImpressionData ironSourceAdImpressionData = new IronSourceAdImpressionData()
                    {
                        ab = arg1.ab,
                        ad_network = arg1.adNetwork,
                        ad_unit = arg1.adUnit,
                        auction_id = arg1.auctionId,
                        country = arg1.country,
                        instance_id = arg1.instanceId,
                        instance_name = arg1.instanceName,
                        lifetime_revenue = parsedDoubleLifetimeRevenue,
                        placement = arg1.placement,
                        precision = arg1.precision,
                        revenue = parsedDoubleRevenue,
                        segment_name = arg1.segmentName,
                        encrypted_cpm = arg1.encryptedCPM,
                        conversion_value = arg1.conversionValue != null ? parsedIntConversionValue : arg1.conversionValue
                    };
                    string json = JsonUtility.ToJson(ironSourceAdImpressionData);
                    callback(json);
                }
                catch (Exception ex)
                {
                    Debug.Log($"error parsing impression " + ex.ToString());
                }
            }
        };
        _subscribed_ironsource = true;
#endif
    }
}

[System.Serializable]
internal class IronSourceAdImpressionData
{
    public string ab;
    public string ad_network;
    public string ad_unit;
    public string auction_id;
    public string country;
    public string instance_id;
    public string instance_name;
    public double lifetime_revenue;
    public string placement;
    public string precision;
    public double revenue;
    public string segment_name;
    public string encrypted_cpm;
    public int? conversion_value;
}
