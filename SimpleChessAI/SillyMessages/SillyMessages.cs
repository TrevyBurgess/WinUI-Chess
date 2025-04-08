//
//
namespace CyberFeedForward.SimpleChessAI.SillyMessages;

using CyberFeedForward.SimpleChessAI.Properties;
using System;
using System.Xml;

/// <summary>
/// Manage silly messages to display to user
/// </summary>
public class SillyMessages
{
    private readonly XmlDocument sillyStrings;
    private Random RandJoke = new(DateTime.Now.Second);

    /// <summary>
    /// Initialize silly messages
    /// </summary>
    public SillyMessages()
    {
        sillyStrings = new XmlDocument();
        sillyStrings.LoadXml(Resources.ResourceManager.GetString("SillyMessages"));

        computerInCheck = GetMessages(SillyMessagesType.ComputerInCheck);
        computerCheckMated = GetMessages(SillyMessagesType.ComputerCheckMated);
        computerLossesQueen = GetMessages(SillyMessagesType.ComputerLossesQueen);
        computerLossesRook = GetMessages(SillyMessagesType.ComputerLossesRook);
        computerLossesBishop = GetMessages(SillyMessagesType.ComputerLossesBishop);
        computerLossesKnight = GetMessages(SillyMessagesType.ComputerLossesKnight);
        computerLossesPawn = GetMessages(SillyMessagesType.ComputerLossesPawn);
        computerLossesPawnEnPassant = GetMessages(SillyMessagesType.ComputerLossesPawnEnPassant);

        userInCheck = GetMessages(SillyMessagesType.UserInCheck);
        userCheckMated = GetMessages(SillyMessagesType.UserCheckMated);
        userLossesQueen = GetMessages(SillyMessagesType.UserLossesQueen);
        userLossesRook = GetMessages(SillyMessagesType.UserLossesRook);
        userLossesBishop = GetMessages(SillyMessagesType.UserLossesBishop);
        userLossesKnight = GetMessages(SillyMessagesType.UserLossesKnight);
        userLossesPawn = GetMessages(SillyMessagesType.UserLossesPawn);
        userLossesPawnEnPassant = GetMessages(SillyMessagesType.UserLossesPawnEnPassant);

        draw = GetMessages(SillyMessagesType.Draw);
        stalemate = GetMessages(SillyMessagesType.Stalemate);
    }

    private string[] GetMessages(SillyMessagesType messageType)
    {
        string messageId = SillyMessagesType.ComputerInCheck.ToString();
        XmlElement messageNodes = sillyStrings.GetElementById(messageId);
        if (messageNodes == null || messageNodes.ChildNodes.Count == 0)
            throw new NotSupportedException(messageId + " doesn't have any messages.");

        string[] messages = new string[messageNodes.ChildNodes.Count];
        for (int i = 0; i < messageNodes.ChildNodes.Count; i++)
        {
            messages[i] = messageNodes.ChildNodes[i].InnerText;
        }

        return messages;
    }

    /// <summary>
    /// Get silly message to display to user
    /// </summary>
    public string GetSillyMessage(SillyMessagesType messageType)
    {
        switch (messageType)
        {
            case SillyMessagesType.ComputerInCheck:
                return computerInCheck[RandJoke.Next(computerInCheck.Length)];

            case SillyMessagesType.ComputerCheckMated:
                return computerCheckMated[RandJoke.Next(computerCheckMated.Length)];

            case SillyMessagesType.ComputerLossesQueen:
                return computerLossesQueen[RandJoke.Next(computerLossesQueen.Length)];

            case SillyMessagesType.ComputerLossesRook:
                return computerLossesRook[RandJoke.Next(computerLossesRook.Length)];

            case SillyMessagesType.ComputerLossesBishop:
                return computerLossesBishop[RandJoke.Next(computerLossesBishop.Length)];

            case SillyMessagesType.ComputerLossesKnight:
                return computerLossesKnight[RandJoke.Next(computerLossesKnight.Length)];

            case SillyMessagesType.ComputerLossesPawn:
                return computerLossesPawn[RandJoke.Next(computerLossesPawn.Length)];

            case SillyMessagesType.ComputerLossesPawnEnPassant:
                return computerLossesPawnEnPassant[RandJoke.Next(computerLossesPawnEnPassant.Length)];

            case SillyMessagesType.UserInCheck:
                return userInCheck[RandJoke.Next(userInCheck.Length)];

            case SillyMessagesType.UserCheckMated:
                return userCheckMated[RandJoke.Next(userCheckMated.Length)];

            case SillyMessagesType.UserLossesQueen:
                return userLossesQueen[RandJoke.Next(userLossesQueen.Length)];

            case SillyMessagesType.UserLossesRook:
                return userLossesRook[RandJoke.Next(userLossesRook.Length)];

            case SillyMessagesType.UserLossesBishop:
                return userLossesBishop[RandJoke.Next(userLossesBishop.Length)];

            case SillyMessagesType.UserLossesKnight:
                return userLossesKnight[RandJoke.Next(userLossesKnight.Length)];

            case SillyMessagesType.UserLossesPawn:
                return userLossesPawn[RandJoke.Next(userLossesPawn.Length)];

            case SillyMessagesType.UserLossesPawnEnPassant:
                return userLossesPawnEnPassant[RandJoke.Next(userLossesPawnEnPassant.Length)];

            case SillyMessagesType.Draw:
                return draw[RandJoke.Next(draw.Length)];

            case SillyMessagesType.Stalemate:
                return stalemate[RandJoke.Next(stalemate.Length)];

            default:
                throw new NotSupportedException("Invalid message type");
        }
    }

    // Messages when computer's piece gets captured.
    private readonly string[] computerInCheck;
    private readonly string[] computerCheckMated;
    private readonly string[] computerLossesQueen;
    private readonly string[] computerLossesRook;
    private readonly string[] computerLossesBishop;
    private readonly string[] computerLossesKnight;
    private readonly string[] computerLossesPawn;
    private readonly string[] computerLossesPawnEnPassant;

    // Messages when comp captures pieces.
    private readonly string[] userInCheck;
    private readonly string[] userCheckMated;
    private readonly string[] userLossesQueen;
    private readonly string[] userLossesRook;
    private readonly string[] userLossesBishop;
    private readonly string[] userLossesKnight;
    private readonly string[] userLossesPawn;
    private readonly string[] userLossesPawnEnPassant;

    // Miscellaneous.
    private readonly string[] draw;
    private readonly string[] stalemate;
}

/// <summary>
/// Silly Message Type
/// </summary>
public enum SillyMessagesType
{
    // Messages when computer's piece gets captured.
    ComputerInCheck,
    ComputerCheckMated,
    ComputerLossesQueen,
    ComputerLossesRook,
    ComputerLossesBishop,
    ComputerLossesKnight,
    ComputerLossesPawn,
    ComputerLossesPawnEnPassant,

    // Messages when computer captures pieces.
    UserInCheck,
    UserCheckMated,
    UserLossesQueen,
    UserLossesRook,
    UserLossesBishop,
    UserLossesKnight,
    UserLossesPawn,
    UserLossesPawnEnPassant,

    // Miscellaneous.
    Draw,
    Stalemate
}
