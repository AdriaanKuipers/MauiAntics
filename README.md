# MauiAntics

Combined mobile (Android) and web experiments

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
- Change project structure
- Simplify MainLayout
- Custom favicon

### AntApp
- Android only
- Setup MudBlazor
- Disable nullable
- Move `Components/Pages` to project root & remove extras