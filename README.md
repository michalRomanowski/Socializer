# Socializer

# Database
Visual Studio -> Package Manager Console -> Set Default project to: Socializer.Database project
To create migration: Add-Migration -Context SocializerDbContext MigrationName
To apply migration: Update-Database -Context SocializerDbContext

# Debugging
To debug Blazor MAUI start in emulator and in Chrome go to 
	chrome://inspect/#devices

# HTTPS in dev
NGROK Instruction:
	https://dashboard.ngrok.com/get-started/setup/windows
	
To start NGROK tunneling:
	ngrok http https://localhost:443
	
# Running on my Mobile
In case of problems: adb devices
and see if it is listed at all

# Huggingface
To try chat with different models: https://huggingface.co/chat/models

https://huggingface.co/chat/models/meta-llama/Llama-3.3-70B-Instruct

# dbpedia.org
DBPedia resource link:
http://dbpedia.org/resource/<resource>

# Create dev cert
dotnet dev-certs https -ep socializerEncryption.pfx -p yourpassword
dotnet dev-certs https -ep socializerSignIn.pfx -p yourpassword

# Publish testing app to GooglePlay

https://learn.microsoft.com/en-us/dotnet/maui/android/deployment/publish-google-play?view=net-maui-8.0

# Debug Profiles
In C:\git\Socializer\Socializer.BlazorHybrid\Socializer.BlazorHybrid.csproj.user

Emulator:
    <ActiveDebugProfile>Pixel 7 - API 34 (Android 14.0 - API 34)</ActiveDebugProfile>
    <ActiveDebugFramework>net8.0-android</ActiveDebugFramework>
    <IsFirstTimeProjectOpen>False</IsFirstTimeProjectOpen>
    <SelectedPlatformGroup>Emulator</SelectedPlatformGroup>
    <DefaultDevice>pixel_7_-_api_34</DefaultDevice>
Windows:
    <ActiveDebugProfile>Windows Machine</ActiveDebugProfile>
    <ActiveDebugFramework>net8.0-windows10.0.19041.0</ActiveDebugFramework>
    <IsFirstTimeProjectOpen>False</IsFirstTimeProjectOpen>
    <SelectedPlatformGroup>Emulator</SelectedPlatformGroup>
    <DefaultDevice>pixel_7_-_api_34</DefaultDevice>

