msbuild ./ /t:Build /p:Configuration=Release
Get-ChildItem -Path ./build -Include *.* -File -Recurse | foreach { $_.Delete()}
mkdir -p ./build/MonoAndroid  -Force
move-item ./src/ModernHttpClient/bin/Release/MonoAndroid/Modern* ./build/MonoAndroid -force
mkdir -p ./build/Xamarin.iOS10  -Force
move-item ./src/ModernHttpClient/bin/Release/Xamarin.iOS10/Modern* ./build/Xamarin.iOS10 -force
mkdir -p ./build/Portable-Net45+WinRT45+WP8+WPA81  -Force
move-item ./src/ModernHttpClient/bin/Release/Portable-Net45+WinRT45+WP8+WPA81/Modern* ./build/Portable-Net45+WinRT45+WP8+WPA81 -force

rm *.nupkg
vendor\nuget\nuget pack ModernHttpClient.nuspec -IncludeReferencedProjects -Prop Configuration=Release
# nuget push *.nupkg <apikey> -source https://www.myget.org/F/orbital-drop-bear/api/v3/ -NonInteractive