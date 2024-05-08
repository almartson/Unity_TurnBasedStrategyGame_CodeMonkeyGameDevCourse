using UnityEngine;
//using DragynGames;
using DragynGames.Commands; // DGConsole

/// <summary>
/// A simple State Pattern Class, that controls the Main Flow of some Random Tests.
/// All that is controlled here. An Orchestra Director.
/// </summary>
public static class RandomTestsManager
{
	#region Attributes
	
	#region Exposed "public" fields

	private static int _score = 0;

	#endregion Exposed "public" fields

	
	#endregion Attributes

	
	#region My Custom Methods
	
	#region Field's PROPERTIES (Get, Set functions)

	public static int Score
	{
		get => _score;
		set => _score = value;
	}

	#endregion Field's PROPERTIES (Get, Set functions)


	#region Collect Point to win the game

	
	public static void Collect(int amount)
	{
		_score += amount;

	}// End Collect


	#endregion Collect Point to win the game
	
	#region Other Test Mehods
	
	[ConsoleAction("Swap", "Swap two numbers...", "number1", "number2")]
	public static bool Swap(int a, int b)
	{
		(a, b) = (b, a);

		Debug.Log($"Performing Swap( {b}, {a} ) a = {a} \n\n... y b = {b}");

		return true;

	}//End Swap
    
	#endregion
	
	#endregion My Custom Methods
}
