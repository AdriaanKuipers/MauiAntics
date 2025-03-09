# MauiAntics

Combined mobile (Android) and web experiments

## Initial setup
- .NET 8
- .NET 8 MAUI ready for Android development. (JAVA + Android)
- MudBlazor Templates
- Solution directory - ex. `~/Projects/MauiAntics` - start point for all commands

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