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
	ngrok http https://localhost:7063
	
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
