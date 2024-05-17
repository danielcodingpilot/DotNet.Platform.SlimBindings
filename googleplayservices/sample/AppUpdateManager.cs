using Android.App;
using Com.Codingpilot.Mauigoogleplayservices;
using Com.Google.Android.Play.Core.Appupdate;



namespace GooglePlayServices;

public class AppUpdateManager
{
    public class CheckForUpdateResult
    {
        public enum UpdateAvailabilities
        {
            Unknown = 0,
            UpdateNotAvailable = 1,
            UpdateAvailable = 2,
            UpdateInProgress = 3
        }
        
        public UpdateAvailabilities UpdateAvailability { get; set; }
        public int AvailableVersionCode { get; set; }
    }
    
    class CheckUpdateOnSuccessListener(TaskCompletionSource<CheckForUpdateResult> taskCompletionSource)
        : Java.Lang.Object, AppUpdateManagerSdk.ISuccessListener
    {
        public void OnSuccess(AppUpdateInfo? p0)
        {
            var result = new CheckForUpdateResult();

            if (Enum.TryParse(p0.UpdateAvailability().ToString(),
                    out CheckForUpdateResult.UpdateAvailabilities updateAvailability))
            {
                result.UpdateAvailability = updateAvailability;
            }
            else
            {
                // Handle the case when parsing fails, you can set a default value or throw an exception
                result.UpdateAvailability = CheckForUpdateResult.UpdateAvailabilities.Unknown;
            }

            result.AvailableVersionCode = p0.AvailableVersionCode();
            
            taskCompletionSource.TrySetResult(result);
        }
    }
    
    class CheckUpdateOnFailureListener(TaskCompletionSource<CheckForUpdateResult> taskCompletionSource) 
        : Java.Lang.Object, AppUpdateManagerSdk.IFailureListener
    {

        public void OnFailure(Java.Lang.Exception? p0)
        {
            taskCompletionSource.SetException(p0);
        }
    }
    
    public static Task<CheckForUpdateResult> CheckForUpdate(Activity activity) 
    {
        var result = new TaskCompletionSource<CheckForUpdateResult>();
        
        var onSuccessListener = new CheckUpdateOnSuccessListener(result);
        var onFailureListener = new CheckUpdateOnFailureListener(result);
        
        var appUpdateManagerSdk = new AppUpdateManagerSdk();
        appUpdateManagerSdk.CheckForUpdate(
            activity,
            onSuccessListener,
            onFailureListener);

        return result.Task;
    }
}