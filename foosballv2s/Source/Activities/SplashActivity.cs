using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using foosballv2s.Droid.Shared;
using foosballv2s.Droid.Shared.Source.Services.CredentialStorage;
using Xamarin.Forms;
using Application = Android.App.Application;

namespace foosballv2s.Source.Activities
{
    /// <summary>
    /// Shows a splash screen on the launch of the app
    /// </summary>
    [Activity(MainLauncher = true, Theme = "@style/SplashTheme")]
    public class SplashActivity : Activity
    {
        private ICredentialStorage _credentialStorage;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            
            _credentialStorage = DependencyService.Get<ICredentialStorage>();

            if (_credentialStorage.GetSavedLanguage() != null)
                Helpers.LanguageHelper.UpdateResources(this, _credentialStorage.GetSavedLanguage());

            if (_credentialStorage.HasExpired())
            {
                // Start auth activity
                StartActivity(new Intent(this, typeof(AuthActivity)));
            }
            else
            {
                // Start home activity
                StartActivity(new Intent(this, typeof(MainActivity)));
                Toast.MakeText(Application.Context, Resource.String.auto_logged_in, ToastLength.Long).Show();
            }
            // close splash activity
            Finish();
        }
    }
}
