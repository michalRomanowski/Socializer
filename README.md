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

# SmartAspNetApp
Publish from Visual Studio Instruction:
https://www.smarterasp.net/support/kb/a2211/core-to-core-converting-a-framework-dependent-app-to-self-contained-in-visual-studio-2022.aspx

# Create cert
dotnet dev-certs https -ep socializerEncryption.pfx -p yourpassword
dotnet dev-certs https -ep socializerSignIn.pfx -p yourpassword
