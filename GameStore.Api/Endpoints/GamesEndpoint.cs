using GameStore.Api.Dtos;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoint
{
    const string GetGameEndpointName = "GetGame";
    private static readonly List<GameDto> games = [
        new (1, "Street Fighter", "Fighting", 19.99M, new DateOnly(1987, 12, 17)),
        new (2, "Final Fantasy", "Fighting", 69.99M, new DateOnly(2024, 2, 29)),
        new (3, "Mega Man", "Platformer", 29.99M, new DateOnly(1987, 12, 17)),
    ];
    public  static void MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/games");

        //get /games
        group.MapGet("/", () => games);

        //get /games/1
        group.MapGet("/{id}", (int id) =>
        {
            var game = games.Find(game => game.Id == id);
            return game is null ? Results.NotFound() : Results.Ok(game);
        } )
            .WithName(GetGameEndpointName);

        //post /games
        group.MapPost("/",(CreateGameDto newGame) =>
        {
            GameDto game = new(
                games.Count + 1,
                newGame.Name,
                newGame.Genre,
                newGame.Price,
                newGame.ReleaseDate
            );
            games.Add(game);
            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
        });

        //PUT /games/1
        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame) =>
        {
            var index = games.FindIndex(game => game.Id == id);

            if (index == -1)
            {
                return Results.NotFound();
            }

            games[index] = new GameDto(
                id,
                updatedGame.Name,
                updatedGame.Genre,
                updatedGame.Price,
                updatedGame.ReleaseDate
            );
            return Results.NoContent();
        });

        //DELETE /games/1
        group.MapDelete("/{id}", (int id) =>
        {
            games.RemoveAll(game => game.Id == id);
            return Results.NoContent();
        });
    }
}
