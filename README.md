# StarSonata Market Board

StarSonata Market Board continuously /mc's all known items in the game, and provides useful functionality using that data.
This project served as a toy to get used to using server-side Blazor, so sorry for any weridness! I've not touched web development in a long time.

### Requires .NET Core 3 Preview

## To Use

- Setup a SqlServer and modify the appsettings.json to point towards it
- Open a command prompt in `./SSMB.SQL`
- Run `dotnet ef update database`
- Create an account at Star Sonata for the purpose of market checking
- Enter the account details in appsettings.json
- Open the .sln and launch the SSML.Blazor application

## Known Issues

- When the server goes down, SSMB will likely crash at the moment.
- Alerts are not implemented
- Certain views can take a VERY long time to display information, be patient!
