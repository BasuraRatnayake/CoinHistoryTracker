﻿using System.Reflection;
using CoinTrackerHistory.API.Services;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options => {
	options.SwaggerDoc("v1", new OpenApiInfo {
		Version = "v1",
		Title = "CoinTrackerHistory API",
		Description = "Manage Coin Purchases and Rewards"
	});
});

builder.Services.AddSingleton<DatabaseService>();

builder.Services.AddControllers().AddJsonOptions(options => {
	options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment()) {
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
