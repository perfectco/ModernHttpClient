using Android.App;
using Android.Widget;
using Android.OS;
using System.Net.Http;
using ModernHttpClient;
using System;

namespace androidtest
{
    [Activity (Label = "androidtest", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        int count = 1;

        protected override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button> (Resource.Id.myButton);

            button.Click += delegate { button.Text = $"{count++} clicks!"; Run (); };

            Run();
        }

        private async void Run()
        {
            HttpClient client = new HttpClient (new NativeMessageHandler());

            string [] urls = new string[]{"http://perdu.com/", "https://qwartz92.altima.fr/api/navigation/v1/getnavigation", "https://www.afpa.fr/", "https://www.filemaker.com/"};
            
            foreach (string url in urls)
            {
                try
                {
                    HttpResponseMessage result = await client.GetAsync (url);

                    var statusCode = result.StatusCode;

                    string content = await result.Content.ReadAsStringAsync ();

                    System.Diagnostics.Debug.WriteLine ($"{statusCode} : {content.Length} : {url}");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            }
        }
    }
}

