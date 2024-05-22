using System;
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
	
	// I want to define a static attribute, called _score2:
	private static int _score2 = 0;



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

	
	[ConsoleAction("Collect", "Collects more items , therefore SCORE for Player one...", "amount")]
	public static void Collect(int amount)
	{
		_score += amount;

	}// End Collect

	#endregion Collect Point to win the game
	
	
	#region Other Test Methods
	
	//[ConsoleAction("Swap", "Swap two numbers...", "number1", "number2")]
	public static bool Swap(ref int a, ref int b)
	{
		(a, b) = (b, a);

		Debug.Log($"Performing Swap( {b}, {a} ) a = {a} \n\n... y b = {b}");

		return true;

	}//End Swap
    
	
	//[ConsoleAction("Swap", "Swap two Quaternions...", "myQuaternion1", "myQuaternion2")]
	public static void Swap(ref Quaternion myQuaternion1, ref Quaternion myQuaternion2)
	{
		(myQuaternion1, myQuaternion2) = (myQuaternion2, myQuaternion1);

	}//End Swap
	
	
	//[ConsoleAction("Max", "Returns the Maximum, between two numbers, of any Type <T>...", "number1", "number2")]
	public static T Max<T>(T number1, T number2) where T : IComparable<T>
	{
		return (number1.CompareTo(number2) > 0) ? number1 : number2;
		
	}//End Max


	/// <summary>
	/// Swap, Using the Generics Type T.
	/// </summary>
	/// <param name="myNumber1"></param>
	/// <param name="myNumber2"></param>
	//[ConsoleAction("Swap", "Swaps two numbers, of any Type <T>...", "myNumber1", "myNumber2")]
	public static void Swap<T>(ref T myNumber1, ref T myNumber2)
	{
		(myNumber1, myNumber2) = (myNumber2, myNumber1);

	}//End Swap

	/// <summary>
	/// GPT-3.5 Generated suggestion for a function that prints the Score of the Game.
	/// </summary>
	[ConsoleAction("PrintScore", "Prints the Score", "")]
	public static void PrintScore()
	{
		Debug.Log($"Score: {Score}");

	}//End PrintScore
	
	#endregion Other Test Methods
	
	#endregion My Custom Methods
}
