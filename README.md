# StarSonata Market Board

StarSonata Market Board continuously /mc's all known items in the game, and provides useful functionality using that data.
This project served as a toy to get used to using server-side Blazor, so sorry for any weridness! I've not touched web development in a long time.

### Requires .NET Core 3 Preview

## To Use

- Enter your Game login credentials in 'SSMB.Blazor/appsettings.json' (Probably best to make a separate account specifically for this purpose, both for security and to let you login to the game using an ordinary client)
- Open a command prompt in SSMB.Blazor
- Enter `dotnet run`
- The server should start up, and your browser should open at `http://localhost:5050`.
- It will take a while before items begin to populate as it has to first go through all known items and check whether they're on the market before they will appear. You can view the background task process at http://localhost:5050/hangfire. I recommend just leaving it running in the background constantly.

## Endpoints

I'll add an index at some point, but these are the main functions:

* Main - http://localhost:5050
    * Basic page showing the most recently updated items. Ignore the Popular Items list. It's a lie.
    
    * This page also shows the most "profitable" items. These are all the items which have the largest disparity between sale prices and purchase prices. You should ignore Industrial Commodities in this list as it doesn't take into account variable pricing.
    
* Appraise - http://localhost:5050/Appraise
    * Enter an item list (in the same format as copy/pasting your ingame inventory provides) and it will check against the most recent /mc results for each item to determine the best sale prices you can fetch for those items. It will display the quantity each shop will take as of the last check, and the order in which to sell to maximise profits. Don't trust it for items who's prices vary on supply like ICs as it'll be wrong.
    
* UnderCut - http://localhost:5050/UnderCut
    * Helps you undercut everyone else! Paste an inventory copy + paste, recieve a tradebay import formatted list of prices
    
* Scrappable - http://localhost:5050/Scrappable
    * Gives you a tradebay import text which contains every single scrappable item, set to buy at 75% of the actual scrap price. I realise now this tool is out I probably can't use this for profit anymore...

## Known Issues

- When the server goes down, SSMB will likely crash at the moment.
- Alerts are not implemented
- Certain views can take a VERY long time to display information, be patient!
