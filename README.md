# StarSonata Market Board

StarSonata Market Board continuously /mc's all known items in the game, and provides useful functionality using that data.
This project served as a toy to get used to using server-side Blazor, so sorry for any weridness! I've not touched web development in a long time.

### Requires .NET Core 3 Preview

## To Use

- Open a command prompt in SSMB.Blazor
- Enter `docker run`
- The server should start up, and your browser should open at `http://localhost:5050`.
- It will take a while before items begin to populate as it has to first go through all known items and check whether they're on the market before they will appear. You can view the background task process at http://localhost:5050/hangfire. I recommend just leaving it running in the background constantly.

## Known Issues

- When the server goes down, SSMB will likely crash at the moment.
- Alerts are not implemented
- Certain views can take a VERY long time to display information, be patient!
