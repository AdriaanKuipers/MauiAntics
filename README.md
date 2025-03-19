# MauiAntics

Combined mobile (Android) and web experiments

## TODO
- MAUI fonts?
- Move this to VersionHistory.md and use README to describe features
- Style login page
- Logout maybe?

## Initial setup
- .NET 8
- .NET 8 MAUI ready for Android development. (JAVA + Android)
- MudBlazor Templates
- Solution directory - ex. `~/Projects/MauiAntics` - start point for all commands

### Random commands repo
- initiate emulator `sdkmanager "emulator" "system-images;android-34;google_apis;x86_64"`
- list emulator device definitions `avdmanager list device`
- create emulator `avdmanager create avd -n pixel4a -d 24 -k "system-images;android-34;google_apis;x86_64"`
- run MAUI app `dotnet build -t:Run -f net8.0-android`


## 0 - Solution setup
- `dotnet new mudblazor --interactivity Server --all-interactive -o AntWeb`
- `dotnet new maui-blazor -o AntApp`


## 1 - Align Web and Android project
Create two basic MudBlazor applications with a counter page and a home page

### AntWeb
- Upgrade MudBlazor
- Disable nullable & prerendering
- Change project structure & remove extras
- Simplify MainLayout
- Custom favicon

### AntApp
- Android only
- Setup MudBlazor
- Disable nullable
- Change project structure & remove extras
- Align MainLayout, Navmenu, Home and Counter
- Icon antics


## 2 - Authentication and double debug
Create AntCore, join all projects in one solution and setup basic local authentication for both applications.
Debug both AntApp and AntWeb from a single command.

### AntCore
- Create `AntCore` directory and with AntCore.csproj
- AntUser
- Counter

### MauiAntics
- Create solution and add all projects
- launch.json & tasks.json

### AntWeb
- AntCore reference & reference cleanup
- AuthController & configuration & Login page
- Update Program and add settings
- Update MainLayout, Routes, _Import & Home


### AntApp
- AntCore reference & reference cleanup
- MauiProgram, MainActivity, AndroidManifest & AntAuthStateProvider (HttpClientHandler to ignore cert error)
- Update MainLayout, Routes, _Import & Home

## 3 - Biometrics
Force fingerprint authentication for certain pages.
Renew after suspend and app switch

- Xamarin.AndroidX.Biometric