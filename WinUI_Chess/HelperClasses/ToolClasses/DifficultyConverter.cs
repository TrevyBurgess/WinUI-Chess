// 
// 
namespace TrevyBurgess.Games.TrevyChess.ChessGameUI
{
    using Microsoft.UI.Xaml.Data;
    using System;
    using TrevyBurgess.Games.TrevyChess.ChessGameAI;

    public class DifficultyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            switch ((ChallengeLevel)value)
            {
                case ChallengeLevel.Easy:
                    return 0;
                case ChallengeLevel.Medium:
                    return 1;
                case ChallengeLevel.Hard:
                    return 2;
                case ChallengeLevel.Expert:
                    return 3;
                case ChallengeLevel.GrandMaster:
                    return 4;
                default:
                throw new NotSupportedException("The specified challenge level doesn't exist");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            int challengeLevel = (int)value;
            switch ((int)value)
            {
                case 0:
                    return ChallengeLevel.Easy;
                case 1:
                    return ChallengeLevel.Medium;
                case 2:
                    return ChallengeLevel.Hard;
                case 3:
                    return ChallengeLevel.Expert;
                case 4:
                    return ChallengeLevel.GrandMaster;
                default:
                throw new NotSupportedException("only inputs of 0 and 1 are meaningful.");
            }
        }
    }
}
