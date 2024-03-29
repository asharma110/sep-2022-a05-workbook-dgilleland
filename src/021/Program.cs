﻿// See https://aka.ms/new-console-template for more information
using static System.Console;

// Main Method body
WriteLine("Simple Looping Math");
WriteLine("===================\n");

double total = 0;
int count = 0;
double value;
do
{
    value = PromptForNumber();
    total += value;
    count++;
}while(value != 0);
count--; // to correct the count for a zero value (exits the loop)

char choice;
do
{
    choice = GetMenuChoice();
    choice = char.ToUpper(choice); // convert to upper character
    switch(choice)
    {
        case 'A':
            WriteLine($"Count: {count}");
            break;
        case 'B':
            WriteLine($"Total: {total}");
            break;
        case 'C':
            WriteLine($"Average: {total / count}");
            break;
        case 'D':
            WriteLine("\n\nGoodbye");
            break;
        default:
            WriteLine($"INVALID INPUT: You entered '{choice}'"); // QnD output
            break;
    }
} while(choice != 'D');

// Methods ============================================
static char GetMenuChoice()
{
    // Display the menu: A) Count, B) Total, C) Average, D) exit, (anything else is "invalid")
    WriteLine("\nChoose your calculation");
    WriteLine("\tA) Count");
    WriteLine("\tB) Total");
    WriteLine("\tC) Average");
    WriteLine("\tD) Exit");
    Write("\nYour choice: ");
    // Get the user input
    string userInput = ReadLine();
    // Return the user input
    char choice;
    if(userInput.Length > 0)
        choice = userInput.First();
    else
        choice = 'X';
    return choice;
}

static double PromptForNumber()
{
    // With simple validation
    Write("Enter a number (leave empty to finish): ");
    double result;
    string userInput = ReadLine();
    while(!double.TryParse(userInput, out result) && !string.IsNullOrEmpty(userInput))
    {
        ForegroundColor = ConsoleColor.DarkRed;
        WriteLine($"The value {userInput} is not a number. Try again.");
        ResetColor();
        Write("Enter a number: ");
        userInput = ReadLine();
    }
    return result;
}
