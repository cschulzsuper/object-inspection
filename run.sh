ASPNETCORE_ENVIRONMENT=Development

dotnet watch run --project "./src/server/ServerApp" &
dotnet watch run --project "./src/blazor/BlazorServerSideHost" &