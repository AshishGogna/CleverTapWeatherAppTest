using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LocationService : MonoBehaviour
{
    #region Public declarations
    public static LocationService instance { get; private set; }
    public string longitude;
    public string latitude;
    #endregion

    #region Unity lifecycle functions
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
    }
    #endregion

    #region Public functions
    public void FindLocation(UnityAction callback)
    {
        if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.FineLocation))
        {
            UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.FineLocation);
        }
        else
        {
            StartCoroutine(FindLocationRoutine(() => { callback(); }));
        }
    }
    #endregion

    #region Private functions
    private IEnumerator FindLocationRoutine(UnityAction callback)
    {
        if (!Input.location.isEnabledByUser)
        {
            CleverTapSDK.ShowToast("Please provide location permissions to this app so that you can be shown correct weather information.", CleverTapSDK.ToastDuration.Long);
            yield break;
        }

        Input.location.Start();

        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait < 1)
        {
            CleverTapSDK.ShowToast("Unexpectedly timed out while trying to fetch your location.", CleverTapSDK.ToastDuration.Long);
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            CleverTapSDK.ShowToast("Unable to determine your location.", CleverTapSDK.ToastDuration.Long);
            yield break;
        }
        else
        {
            longitude = Input.location.lastData.longitude.ToString("n2");
            latitude = Input.location.lastData.latitude.ToString("n2");
            callback();
        }

        Input.location.Stop();
    }
    #endregion
}
