using System;
using CoreGraphics;
using Foundation;
using UIKit;
using System.Net.Http;
using System.Threading;
using System.Diagnostics;
using ModernHttpClient;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Net;
using System.Linq;

namespace Playground.iOS
{
    public partial class Playground_iOSViewController : UIViewController
    {
        public Playground_iOSViewController () : base ("Playground_iOSViewController", null)
        {
            /*
            Task.Run (async () => {
                var client = new HttpClient(new NSUrlSessionHandler());

                var item = new { MyProperty = "Property Value" };
                var content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");
                var result = await client.PostAsync("http://requestb.in/1aj9b9c1", content);

                result.EnsureSuccessStatusCode();
            });
            */

            //This API is only available in Mono and Xamarin products.
            //You can filter and/or re-order the ciphers suites that the SSL/TLS server will accept from a client.
            //The following example removes weak (export) ciphers from the list that will be offered to the server.
            //ServicePointManager.ClientCipherSuitesCallback += (protocol, allCiphers) =>
            //    allCiphers.Where(x => !x.Contains("EXPORT")).ToList();

            //Here we accept any certificate and just print the cert's data.
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => {
                System.Diagnostics.Debug.WriteLine("Callback Server Certificate: " + sslPolicyErrors);

                foreach(var el in chain.ChainElements) {
                    System.Diagnostics.Debug.WriteLine(el.Certificate.GetCertHashString());
                    System.Diagnostics.Debug.WriteLine(el.Information);
                }

                return true;
            };

        }

        CancellationTokenSource currentToken;
        HttpResponseMessage resp;

        partial void cancelIt (Foundation.NSObject sender)
        {
            this.currentToken.Cancel();
            if (resp != null) resp.Content.Dispose();
        }

        void HandleDownloadProgress(long bytes, long totalBytes, long totalBytesExpected)
        {
            Console.WriteLine("Downloading {0}/{1}", totalBytes, totalBytesExpected);

            BeginInvokeOnMainThread(() => {
                var progressPercent = (float)totalBytes / (float)totalBytesExpected;
                progress.SetProgress(progressPercent, animated: true);
            });
        }

        async partial void doIt (Foundation.NSObject sender)
        {
            var cert = "MIIK4QIBAzCCCqcGCSqGSIb3DQEHAaCCCpgEggqUMIIKkDCCBUcGCSqGSIb3DQEHBqCCBTgwggU0AgEAMIIFLQYJKoZIhvcNAQcBMBwGCiqGSIb3DQEMAQYwDgQI0Ot32GaJy4kCAggAgIIFAPhJmOktQq5TCaDUSlQkwFVCLjhTNjc6q93xsTx+HIrbdnDNYMF7jc2PA5zA8gYzlb3jvL4hMUEQrkI/ot2MGxdy4lFRCwoXm7sUhkPbCPOF0URGVqwSHDHWnwe0kH08vlJBcWtJn72fkKkD7an5ElLeNyPn0PHghckrU8Q+/O3Y/nk+BuzPMdjnKWIZu1bAJs27CX9ISm3KwcMoVrHwfeqMbK9KxQ7zRy70RQgUqegrClbCADVcAAKpOABZNkoQri4wJVnXS7/N3nvHDohk1HVsewbdu6JGvx2gaOAg1ImXBsHuy1vmQBDH9oa0mZHb7Tp/bKqBbWuDHLPLEzrItzhijd+1dKrs3C6SZRcbmaNB9YQqQW3bKLGXbf0K2jGuoB4xGHrqmrjq5Ey/nK8pPLuoQ85UelY7bpwsk3H4fBiceHv4ZzDYPYFbdmnY5zI728MrM04YHB74DnYvGRo+7vcTt1RtUCABCKxLUJkuKNqg2qTVKJoK9534F+Je9LJT9EPyrW4v484kE4lENSHcJMJzxqx+fSTNkUloA/zyvAXlAwQjAsEhy13IrtCOmjGobPRLpcDkKDbsTRRsEZ2fxjLbdA5zKQIXWB1yebtIh0mSRwyxICZp9YGBjWRZ1Ukv4O1bLjzw7KZuY6QwSAN8RPmDwICoVcCJ02H3hTtfj5buT3c7KC+zEgwcvQixi0aMjMtoNKV+N846YTIAQKb0cr3uf4dOE1rgNeSyGHJ8YgOwkd4xk1ODEFkbbsHCnRqPSX9a4z3IChMnhoa2k3vThg267L0REATUTJ0lUzE9JXzKEVCXrKNhDs+jNmQ8L+laiMZEOPIMXfBjN6y1skF3dMLKSJcy0QqI+A4OgePQKaPzp2uLoQXMOHtDZ7XVSeRT0jNOqQMfxi/zMCl6ZDxpzaUjfpzxHlrKWfyehH43CGk7lYHFDePt6sh8k01q2uq8+2W29G4cmZfKK449RrqhoI9CiIzaVXPk72VVD/ReRrVIk6Avs2gZFpP1g7ai/+ynSeJyzVR3Fs3k81fPSmzoH5AuiZaru2WQO/5Dg5A6S6cDGLUTY4WhF1R5ffCO5O0JhtOFLxYvasI/0NnZFpsZflFcfBbGcwoqOmU2ztEWlPalfXTxyY8noq6UoH6nrDGD5p4wttj+kij2m5w0Baq5RqQxJHJwKaNthddJygbg074AVRM97ojHNxeh9xHgt3XOyDKADWg2A33XaHdDR219ZwZWuFxucb+0sZeKXyzd7ceUAcu/0Ouhv7HacBMzZkb9DQAasF6mDr3GagHfbPauH6qwbLKI+cDE30FM4rfJLW4hoZqASdotvLe1xs/RUmOfH9vsITK86ZZDVzzSBQmwaOPKA2m4WDb51P57820VeTcg2VtlwkHophaWjRpF3pzrYcX9RpHfa34biP943cSrq4U/2Eblxf6hUuvdl/4MdTnpBAbrxjdo9fJmdO+AnQ3x9IjAir58eu63vIMFKn/tEgUU1kDRELcOMYTinAy1Gp0U/0VdXJ5etGa/s1NC7p9gLII4zbhtLr7BMMTAgL33+nVK4OiEl+INmgqjlndG1WCT3J8KP9yv2NrHqlP/ymGvfnMwkjDLEEPv2gZcn6lkOKu/kZUgNOJH4bGeZwhG9dqoN6Z9YuMkLmC8o7PT/pLHBt7kxLezSvpD68H4rV+xZNQFOwyPO10nLXJlwOtIYVisMIIFQQYJKoZIhvcNAQcBoIIFMgSCBS4wggUqMIIFJgYLKoZIhvcNAQwKAQKgggTuMIIE6jAcBgoqhkiG9w0BDAEDMA4ECPxdMZc5K7NhAgIIAASCBMhwxZzl5X4cDxW+lpnvd6uWWOOs6AHJa8//QUZ9CAwVvcMuwaH8DPhFLQQnhY9KPJYsj8fG4Qsx8M+IN9Y+WHrYcDx/fL0aOmW1PunFonTeEJ/JaoKaNeLgVICHzN90Ph8Zz3B2X9JA9znnJkyX5l64OkLS0ZoEYN/aD9Lk+fFgx9vx17oseI5Kr49Usqa9EUuMWH0aFbKhwErOcz6Eh81khfCrIOLbsKTFi80LJR6ttS39kCzUWVoSbWGuEIOXnH6vGrm6PkXRZI2YZTvttnuThc/+k51jswnHVqvUi8I5amllsQWNGnIW2yzNg7ZNfEYkKEavyP4Tpuaa8PmQf579k+pO/mX6CLZRiBCb7L/tZ8YsM8+HTqGqJh31APgMRUwrTq4tgT1a6rNFx67hg2uys+QORdp+pK/r6pOOOw1U2vo0EG9viZM42j+6lH33yuH/QOnC63saZVVIUNnnoaqhOi1YT+LR/5PSWi9J7lf2AMAK66hQhFcTrZsvI8EfVKAzCSb0TamA58xGzHhT+lMqPJIrSc4QBWdfI4tqXSKAH0YvWkh9Btzhyy1ts4MqM8owm75J52a3DjqBi6bYN75QRL1mjs0Ua6AlCEMfT6Dqisefe5sQMZLnbceNfORAOLvd804UriEBzrONzXb240Y3+i527S/8tyjAWcPmmXZE5xO1H+fKsA1J3TC4eeV+6cPXybPVDVyVhPFkz5JpGYkxjAH3EAngW/EPSMsRHcxY3NqfNc/VO/i4h1ogYNKf/1GWXM00434ybdq+rMlpO/A3poJ06QDglEon3kr97KQkygOXd7MWS4xfiBWQP7dsqpmJZnXtMQ00iCOhaFrMG7AvsedTEoOFmhXvbf/ffd2tD/UAmm01fkyHfpJvi3gu/86NBVxM7eJDLZ1FLdqgNMBorln3R4yfqeb7NlssXo2lCMs5NYM2y7e5pt2BRvAYbcBpLaH+7B8Oiq/YXOkLgl+yLwFomnsD00XLYQ/WcNBCxOlc40DyUnxKplz1qGeMPIaZHiPkBiQ/MnA/aQ+wAdhV2gQxpY4aqiKtC/vFFbOnxuFlcnKKWKXkpu2khXsvSR598v+uip8FRW87Wrxtb/mYqmKnwcruIjzwm9O9XShC7XpmYWNtADfkddfuv+qF4tjGkimaw8zIF3smOGm60rjr2mm2NuKvf9Eez6lwxBtijsAixTd3OUpXQKq9UP2OU9h9cybxxb5Wctbe5UMVCSkSy+Om5sKh2x25MqKdbrjMbtDHOUS5IvHkdminkEyFKFsBi5o77cG8Sa14Ibu3Dq7eRjpNg0NOzTrE0HYdfB0TF8CgiUpXsgPeuXVwHAwkoWqNUxMgW8LTtaKEG22pOYF1BkzUcEeLMnfILQhDwmqUSgOneJ5JtEy+GPSqIJq2XCgSCnPr1C1ihzO2dWU+NfVkxo/qbN9+BSZuDCW30Vmkd3JKsVdANkYf/3kTp3F1K5Au/qgA9BchnHU5WKusFTO/yf8U68UBkufskOby4DsBxG9kZbygOre5SbzI+rdxZVYWgDafhylW/j9q/TlIQGdhQaOfhdiPunTRpZ0UECiyuLv2cUEC7NhTpj0grUyXn2i3DjvdED3K3FbSYJZ0NAJbG94RJSAqNswxJTAjBgkqhkiG9w0BCRUxFgQUKNireaeXb+7TbR3wuaw9Pza3xN8wMTAhMAkGBSsOAwIaBQAEFDFG6JpY3gFhgVF1JsyNo2D+zU5RBAg4bqQ33t/vZAICCAA=";
            var handler = new NativeMessageHandler(customSSLVerification: true, pfxData: Convert.FromBase64String(cert), pfxPassword:"badssl.com");
            var client = new HttpClient(handler);

            currentToken = new CancellationTokenSource();
            var st = new Stopwatch();

            st.Start();
            try {
                handler.DisableCaching = true;
                // var url = "https://github.com/paulcbetts/ModernHttpClient/releases/download/0.9.0/ModernHttpClient-0.9.zip";
                //var url = "https://app.ibodyshop.com";
                var url = "https://client.badssl.com";
             

                var request = new HttpRequestMessage(HttpMethod.Get, url);
                handler.RegisterForProgress(request, HandleDownloadProgress);

                //if using NTLM authentication pass the credentials below
                //handler.Credentials = new NetworkCredential("user","pass");

                resp = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, currentToken.Token);
                result.Text = "Got the headers!";

                Console.WriteLine("Status code: {0}", resp.StatusCode);

                Console.WriteLine("Reason: {0}", resp.ReasonPhrase);

                Console.WriteLine("Headers");
                foreach (var v in resp.Headers) {
                    Console.WriteLine("{0}: {1}", v.Key, String.Join(",", v.Value));
                }

                Console.WriteLine("Content Headers");
                foreach (var v in resp.Content.Headers) {
                    Console.WriteLine("{0}: {1}", v.Key, String.Join(",", v.Value));
                }

                var bytes = await resp.Content.ReadAsByteArrayAsync();
                result.Text = String.Format("Read {0} bytes", bytes.Length);

                var md5 = MD5.Create();
                var md5Result = md5.ComputeHash(bytes);
                md5sum.Text = ToHex(md5Result, false);
            } catch (Exception ex) {
                result.Text = ex.ToString();
            } finally {
                st.Stop();
                result.Text = (result.Text ?? "") + String.Format("\n\nTook {0} milliseconds", st.ElapsedMilliseconds);
            }
        }

        public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
        {
            // Return true for supported orientations
            return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
        }
        
        public static string ToHex(byte[] bytes, bool upperCase)
        {
            var result = new StringBuilder(bytes.Length*2);

            for (int i = 0; i < bytes.Length; i++)
                result.Append(bytes[i].ToString(upperCase ? "X2" : "x2"));

            return result.ToString();
        }
    }
}
