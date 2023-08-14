using AutoMapper;
using cmdAPI.Data;
using cmdAPI.DTOs;
using cmdAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var sqlConnBuilder = new SqlConnectionStringBuilder();

sqlConnBuilder.ConnectionString = builder.Configuration.GetConnectionString("SQLDbConnection");
sqlConnBuilder.UserID = builder.Configuration["UserId"];
sqlConnBuilder.Password = builder.Configuration["Password"];

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(sqlConnBuilder.ConnectionString));
builder.Services.AddScoped<ICommandRepo, SQLCommandRepo>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

/* Endpoints */

app.MapGet("api/v1/commands", async (ICommandRepo repo, IMapper mapper) => 
{
    var commands = await repo.GetallCommands();
    return Results.Ok(mapper.Map<IEnumerable<CommandReadDto>>(commands));
});

app.MapGet("api/v1/commands/{id}", async (ICommandRepo repo, IMapper mapper, [FromRoute]int id) => 
{
    var command = await repo.GetCommandById(id);

    if (command is null)
    {
        return Results.NotFound();
    }
    
    return Results.Ok(mapper.Map<CommandReadDto>(command));
});

app.MapPost("api/v1/commands", async (ICommandRepo repo, IMapper mapper, CommandCreateDto cmdCreateDto) => 
{
    var commandModel = mapper.Map<Command>(cmdCreateDto);

    await repo.CreateCommand(commandModel);
    await repo.SaveChangesAsync();

    var cmdReadDto = mapper.Map<CommandReadDto>(commandModel);
    return Results.Created($"api/v1/commands/{cmdReadDto.Id}", cmdReadDto);
});

app.MapPut("api/v1/commands/{id}", async (ICommandRepo repo, IMapper mapper, [FromRoute]int id, CommandUpdateDto cmdUpdateDto) => 
{
    var command = await repo.GetCommandById(id);

    if (command is null)
    {
        return Results.NotFound();
    }

    mapper.Map(cmdUpdateDto, command);

    await repo.SaveChangesAsync();

    return Results.Ok(command);
}); 

app.MapDelete("api/v1/commands/{id}", async (ICommandRepo repo, IMapper mapper, [FromRoute]int id) => 
{
    var command = await repo.GetCommandById(id);

    if (command is null)
    {
        return Results.NotFound();
    }

    repo.DeleteCommand(command);

    await repo.SaveChangesAsync();

    return Results.NoContent();
});

app.Run();