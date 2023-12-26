using System;
using System.Text.RegularExpressions;

namespace CoinTrackerHistory.API.Configurations;

public static class Constants {
	public static readonly int DECIMAL_PLACES = 8;
	public static Regex ID = new Regex("^[a-fA-F0-9]+$");
	public static Regex NUM = new Regex("^[0-9]+$");
	public static Regex COMMAND_VALUE = new Regex(@"^[A-Za-z0-9_\.]{1,50}$");
	public static Regex COMMAND_FIELD = new Regex(@"^[A-Za-z\.]{1,50}$");

}
