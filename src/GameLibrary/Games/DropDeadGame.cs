﻿// New way to declare a namespace for the following types/classes, called "file-scoped namespace declaration".
namespace Games;
using Games.CommonObjects;
public class DropDeadGame
{
    #region Event-Driven Functionality
    // My class will publish two events: TurnStarted and RollFinished.
    // Anyone using an instance of DropDeadGame can subscribe to these events
    // and they will be notified whenever they occur.
    public event EventHandler<string> TurnStarted;
    public event EventHandler<DropDeadTurnResult> RollFinished;

    /// <summary>Notifies subscribers of the TurnStarted event that a player has started their turn.</summary>
    private void RaiseTurnStarted(string playerName)
    {
        if(TurnStarted != null) // The if is needed because it's possible that nobody is listening
            TurnStarted.Invoke(this, playerName);
            //                 this is a reference to the instance of DropDeadGame which is sending
            //                  the message
            //                       playerName is the "payload" of the message
    }
    private void RaiseRollFinished(string name, int score, int numberOfDie, Die[] dice)
    {
        if(RollFinished != null)
        {
            TurnStatus status;
            if(numberOfDie == 0)
                status = TurnStatus.Finished;
            else
                status = TurnStatus.InProcess;
            List<int> rollResults = new List<int>();
            foreach(var die in dice)
                rollResults.Add(die.FaceValue);
            DropDeadTurnResult details = new(name, score, status, rollResults.ToArray());

            RollFinished.Invoke(this, details);
        }
    }
    #endregion

    #region Fields
    // Have an array of strings for player names
    private readonly string[] PlayerNames;
    // Have an array of integers for player scores
    private readonly int[] PlayerScores;
    // I should have a constructor that gets the players
    #endregion

    public DropDeadGame(string[] players)
    {
        // TODO: Validation - if there's a problem, I'll be throwing an exception
        //  - Not enough players for the game (min 2)
        //  - Make sure the individual names are not "empty"

        PlayerNames = players;
        PlayerScores = new int[players.Length]; // an array the same size as the # of players
        // Each element in the array has the default value for the int data type
        // (which is zero)
    }

    public void Play()
    {
        // Each player runs their turn for rolling the die
        for(int index = 0; index < PlayerNames.Length; index++)
        {
            RaiseTurnStarted(PlayerNames[index]); // Let the world know who's turn it is
            TakeTurn(index);
        }
        // Then I will know the scores and can determine the winner
    }

    private void TakeTurn(int index)
    {
        // Game ends when the players have finished their turns
        int numberOfDie = 5;
        Die[] dice;
        do
        {
            dice = CreateDice(numberOfDie);

            PlayerScores[index] += RollDie(dice);
            numberOfDie = CheckRemainingDie(dice);
            // Let the world know that this roll in the player's turn is complete
            RaiseRollFinished(PlayerNames[index], PlayerScores[index], numberOfDie, dice);
        } while (numberOfDie > 0);
    }

    private static Die[] CreateDice(int numberOfDie)
    {
        Die[] dice;
        // Create an array of die objects
        dice = new Die[numberOfDie]; // Create an array with 5 empty slots
                                     // Put a new die in each slot of the array
        for (int count = 0; count < dice.Length; count++)
            dice[count] = new Die();
        return dice;
    }

    /// <summary>
    /// Roll all the supplied die and return an appropriate score.
    /// </summary>
    public int RollDie(Die[] dice)
    {
        int score = 0;
        foreach(Die die in dice)
            die.Roll();
        
        if(CheckRemainingDie(dice) == dice.Length)
            foreach(Die die in dice)
                score += die.FaceValue;
        return score;
    }

    /// <summary>
    /// Determines how many die can be used in the next roll (by excluding all die with face values of 2 and 5)
    /// </summary>
    public int CheckRemainingDie(Die[] dice)
    {
        int count = 0;
        foreach(Die die in dice)
            if(die.FaceValue != 2 && die.FaceValue != 5)
                count++;
        return count;
    }
}
