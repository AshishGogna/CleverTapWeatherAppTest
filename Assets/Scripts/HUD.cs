using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    #region Public declarations
    public GameObject fetchingText;
    #endregion

    #region Public functions
    public void ShowTemperature()
    {
        try
        {
            fetchingText.SetActive(true);
            LocationService.instance.FindLocation(() =>
            {
                WeatherAPICommunicator.instance.GetCurrentWeather(LocationService.instance.longitude, LocationService.instance.latitude, (temp) =>
                {
                    string text = string.Format("Current temperature: {0}°C", temp);
                    CleverTapSDK.ShowToast(text, CleverTapSDK.ToastDuration.Long);
                    fetchingText.SetActive(false);
                });
            });
        } catch(Exception e)
        {
            CleverTapSDK.ShowToast("Unexpected error occured: " + e.Message, CleverTapSDK.ToastDuration.Long);
            fetchingText.SetActive(false);
        }
    }
    #endregion
}
