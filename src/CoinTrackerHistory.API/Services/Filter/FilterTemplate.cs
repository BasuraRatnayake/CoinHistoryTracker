﻿using System;
namespace CoinTrackerHistory.API.Services.Filter;

public class FilterTemplate {
	public required FilterCommands Command {
		get; set;
	}
	public required string Field {
		get; set;
	}
	public string Value {
		get; set;
	}
}
