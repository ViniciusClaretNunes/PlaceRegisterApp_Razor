# PlaceRegisterApp_Razor

Razor Pages app with EF Core (SQLite) for registering places with images.

How to run:

1. Install .NET 7 SDK.
2. Extract the folder and open a terminal in its root.
3. Run:
   dotnet run
4. Open the URL shown in the terminal (e.g. http://localhost:5000)

Notes:
- Database file: data/places.db
- Uploaded images: wwwroot/uploads
- This project ensures DB created at startup (EnsureCreated).
