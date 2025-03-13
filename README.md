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


### 2 - Authentication
Create AntCore, join all projects in one solution and setup basic local authentication for both applications

### AntCore
- Create `AntCore` directory and with AntCore.csproj
- AntUser
- Counter

### AntWeb
- AntCore reference
- AuthController & configuration
- Update Routes, _Import & Home


### AntApp
- AntCore reference
- AntAuthStateProvider (HttpClientHandler to ignore cert error)