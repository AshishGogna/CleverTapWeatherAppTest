using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class WeatherAPICommunicator : MonoBehaviour
{
    #region Public declarations
    public static WeatherAPICommunicator instance { get; private set; }
    #endregion

    #region Private declarations
    private const String ENDPOINT = "https://api.open-meteo.com/v1/forecast?latitude={0}&longitude={1}&timezone=IST&daily=temperature_2m_max&current_weather=true";
    #endregion

    #region Unity lifecycle functions
    private void Awake()
    {
        instance = this;  
    }
    #endregion

    #region Public functions
    public void GetCurrentWeather(string longitude, string latitude, UnityAction<float> callback)
    {
        StartCoroutine(GetCurrentWeatherRoutine(longitude, latitude, (t) => { callback(t); }));
    }

    public IEnumerator GetCurrentWeatherRoutine(string longitude, string latitude, UnityAction<float> callback)
    {
        string url = string.Format(ENDPOINT, latitude, longitude);
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            CleverTapSDK.ShowToast("Network error, could not fetch weather information:" + request.error, CleverTapSDK.ToastDuration.Long);
        }
        else
        {
            Debug.Log("Received" + request.downloadHandler.text);
            WeatherAPIResponse wares = JsonUtility.FromJson<WeatherAPIResponse>(request.downloadHandler.text);
            callback(wares.current_weather.temperature);
        }
    }
    #endregion
}
