# Socializer

# Database
Visual Studio -> Package Manager Console -> Set Default project to: Socializer.Database project
To create migration: Add-Migration -Context SocializerDbContext MigrationName
To apply migration: Update-Database -Context SocializerDbContext

# Debugging
To debug Blazor MAUI start in emulator and go to chrome://inspect/#devices in Chrome

# HTTPS in dev
NGROK Instruction:
	https://dashboard.ngrok.com/get-started/setup/windows
	
To start NGROK tunneling:
	ngrok http https://localhost:7063
	
# Running on my Mobile
In case of problems: adb devices
and see if it is listed at all