using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.Purchasing;

public class Currency : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}

    /*public static string GetCurrencySymbol(string code)
    {
        System.Globalization.RegionInfo regionInfo = (from culture in System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.InstalledWin32Cultures)
                                                      where culture.Name.Length > 0 && !culture.IsNeutralCulture
                                                      let region = new System.Globalization.RegionInfo(culture.LCID)
                                                      where String.Equals(region.ISOCurrencySymbol, code, StringComparison.InvariantCultureIgnoreCase)
                                                      select region).First();

        return regionInfo.CurrencySymbol;
    }
    */
}
