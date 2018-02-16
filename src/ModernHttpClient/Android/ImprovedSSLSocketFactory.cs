using System;
using System.Collections.Generic;
using Android.OS;
using Android.Runtime;
using Java.Net;
using Javax.Net.Ssl;
using Square.OkHttp3;

namespace ModernHttpClient
{
    internal class ImprovedSSLSocketFactory : SSLSocketFactory
    {
        private readonly ITrustManager _trustManager;
        private SSLSocketFactory _factory;

        public ImprovedSSLSocketFactory (SSLSocketFactory factory, ITrustManager trustManager)
        {
            _factory = factory;
            _trustManager = trustManager;
        }

        public override string [] GetDefaultCipherSuites () => GetFactory ().GetDefaultCipherSuites ();

        public override string [] GetSupportedCipherSuites () => GetFactory ().GetSupportedCipherSuites ();

        public override Socket CreateSocket (InetAddress address, int port, InetAddress localAddress, int localPort) => GetFactory ().CreateSocket (address, port, localAddress, localPort).PatchSocket ();

        public override Socket CreateSocket (InetAddress host, int port) => GetFactory ().CreateSocket (host, port).PatchSocket ();

        public override Socket CreateSocket (string host, int port, InetAddress localHost, int localPort) => GetFactory ().CreateSocket (host, port, localHost, localPort).PatchSocket ();

        public override Socket CreateSocket (string host, int port) => GetFactory ().CreateSocket (host, port).PatchSocket ();

        public override Socket CreateSocket (Socket s, string host, int port, bool autoClose) => GetFactory ().CreateSocket (s, host, port, autoClose).PatchSocket ();

        public override Socket CreateSocket () => GetFactory ().CreateSocket ().PatchSocket ();

        protected override void Dispose (bool disposing)
        {
            if (disposing) {
                _factory?.Dispose ();
                _factory = null;
            }

            base.Dispose (disposing);
        }


        private SSLSocketFactory GetFactory ()
        {
            if (_factory != null) {
                return _factory;
            }

            Android.Util.Log.Warn ("ModernHttpClient", "ImprovedSSLSocketFactory : creating factory again");
            var context = SSLContext.GetInstance ("TLS");
            context.Init (null, new [] { _trustManager }, null);
            _factory = context.SocketFactory;
            return _factory;
        }

        private Socket PatchSocket (Socket s)
        {
            if (s is SSLSocket sslSocket) {
                sslSocket.SetEnabledProtocols (sslSocket.GetSupportedProtocols ());
            }

            return s;
        }
    }

    internal static class TLSExtensions
    {
        public static Socket PatchSocket (this Socket s)
        {
            if (s is SSLSocket sslSocket) {
                sslSocket.SetEnabledProtocols (sslSocket.GetSupportedProtocols ());
            }

            return s;
        }

        //https://github.com/square/okhttp/issues/2372#issuecomment-244807676
        public static OkHttpClient.Builder EnableTls12OnPreLollipopDevices (this OkHttpClient.Builder builder)
        {
            int currentVersion = (int)Build.VERSION.SdkInt;
            if (currentVersion >= 16 && currentVersion < 22) {
                try {
                    //Creation of X509TrustManager : https://square.github.io/okhttp/3.x/okhttp/okhttp3/OkHttpClient.Builder.html#sslSocketFactory-javax.net.ssl.SSLSocketFactory-javax.net.ssl.X509TrustManager-
                    var trustManagerFactory = TrustManagerFactory.GetInstance (TrustManagerFactory.DefaultAlgorithm);
                    trustManagerFactory.Init ((Java.Security.KeyStore)null);
                    var trustManagers = trustManagerFactory.GetTrustManagers ();

                    if (trustManagers.Length != 1) {
                        throw new Java.Lang.IllegalStateException ($"Unexpected default trust managers: {trustManagers}");
                    }

                    var trustManager = trustManagers [0].JavaCast<IX509TrustManager> ();
                    if (trustManager == null) {
                        throw new Java.Lang.IllegalStateException ($"Unexpected default trust managers: {trustManagers}");
                    }

                    var context = SSLContext.GetInstance ("TLS");
                    context.Init (null, new ITrustManager [] { trustManager }, null);
                    builder.SslSocketFactory (new ImprovedSSLSocketFactory (context.SocketFactory, trustManager), trustManager);

                    ConnectionSpec connectionSpec = new ConnectionSpec.Builder (ConnectionSpec.ModernTls)
                        .TlsVersions (TlsVersion.Tls12)
                        .Build ();

                    List<ConnectionSpec> connexionSpecs = new List<ConnectionSpec>
                    {
                        new ConnectionSpec.Builder(ConnectionSpec.ModernTls).TlsVersions(TlsVersion.Tls12).Build(),
                        ConnectionSpec.ModernTls,
                        ConnectionSpec.CompatibleTls,
                        ConnectionSpec.Cleartext,
                    };

                    builder.ConnectionSpecs (connexionSpecs);
                } catch (Exception ex) {
                    Android.Util.Log.Warn ("ModernHttpClient", $"Unable to enable TLS 1.2 on okhttpclient: {ex}");
                }
            }

            return builder;
        }
    }
}