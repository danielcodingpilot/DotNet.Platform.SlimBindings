using Android.Content;
using Android.OS;

namespace GooglePlayServices.Sample;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

   
      
    private static Intent GetRateIntent(string url)
    {
        var intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(url));
        intent.AddFlags(ActivityFlags.NoHistory);
        intent.AddFlags(ActivityFlags.MultipleTask);
        intent.AddFlags((int)Build.VERSION.SdkInt >= 21 
            ? ActivityFlags.NewDocument
            : ActivityFlags.ClearWhenTaskReset);
        intent.SetFlags(ActivityFlags.ClearTop);
        intent.SetFlags(ActivityFlags.NewTask);
        
        return intent;
    }
    
    private static Task<bool> OpenApplicationInStoreAsync(CancellationToken cancellationToken)
    {
        try
        {
            var intent = GetRateIntent($"market://details?id={AppInfo.Current.PackageName}");
            intent.SetPackage("com.android.vending");
            
            Platform.CurrentActivity.StartActivity(intent);
            return Task.FromResult(true);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Unable to launch app store: " + ex.Message);
        }
        
        try
        {
            var intent = GetRateIntent($"https://play.google.com/store/apps/details?id={AppInfo.Current.PackageName}");
            
            Platform.CurrentActivity.StartActivity(intent);
            return Task.FromResult(true);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Unable to launch app store: " + ex.Message);
        }

        return Task.FromResult(false);
    }

    
   

    async void OnAppEventClicked(object sender, EventArgs e)
    {
        try {
            var result = await AppUpdateManager.CheckForUpdate(Platform.CurrentActivity);
            
            await DisplayAlert("Update Check", 
                $"UpdateAvailability: {result.UpdateAvailability} - " +
                $"AvailableVersionCode: {result.AvailableVersionCode} - " +
                $"Current-Version: {AppInfo.Current.Version} - " +
                $"Current-VersionString: {AppInfo.Current.VersionString} - ", "OK");
            
        } catch (Exception ex) {
            await DisplayAlert("Error checking for update", ex.ToString(), "OK");
        }
    }

    private async void AppEventButton2_OnClicked(object? sender, EventArgs e)
    {
        await OpenApplicationInStoreAsync(CancellationToken.None);
    }
}
