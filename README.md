# Codejam6

### How to run

1) Navigate to **\codejam5b.client\** and run `npm i`
2) Navigate to **\Codejam5b.Server\** and delete migrations folder if present
3) Run the following EF Migration commands:
- `dotnet tool restore`
- `dotnet ef migrations add InitialCreate`
- `dotnet ef database update`

4) Verify Migrations ran successfully
5) Run `dotnet run` from **\Codejam5b.Server\** directory to run the webapp
6) Run `npm test` to run unit tests
8) Navigate to the localhost URL prompted

Prompt another workflow ya heard
