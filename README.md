ModernHttpClient
================

This library brings the latest platform-specific networking libraries to
Xamarin applications via a custom HttpClient handler. Write your app using
System.Net.Http, but drop this library in and it will go drastically faster.
This is made possible by two native libraries:

* On iOS, [NSURLSession](https://developer.apple.com/library/ios/documentation/Foundation/Reference/NSURLSession_class/Introduction/Introduction.html)
* On Android, via [OkHttp](http://square.github.io/okhttp/)

## Usage

The good news is, you don't have to know either of these two libraries above,
using ModernHttpClient is the most boring thing in the world. Here's how
it works:

```cs
var httpClient = new HttpClient(new NativeMessageHandler());
```

## How can I use this in a PCL?

Just reference the Portable version of ModernHttpClient in your Portable
Library, and it will use the correct version on all platforms.

## How to build and update Nuget package

Currently there is no CI set up for this pacakge, it's all done by hand.
There are a few bits of software required to build Nuget packages from terminal on the Mac:
Visual Studio Core (includes the Nuget software)
Powershell (for building the iFit.Analytics source)

There is a handy powershell script included in this repo that can be used to create the nuget package:
1. Update the version number in ModernHttpClient.nuspec
2. Open ModernHttpClient.sln in Visual Studio and update the software version numbers.
3. In terminal, navigate to the ModernHttpClient directory.
4. Run the following commands:
to start powershell:
```
pwsh
```
from the powershell prompt:
```
./buildNuget.ps1
```
5. This will create iFit.ModernHttpClient.version.nupkg in the ModernHttpClient directory.
6. You can add this directory as a Nuget stream in Visual Studio if you'd like to test the package before uploading it.
7. Upload the .nupkg to the iFit myget.org private nuget stream.