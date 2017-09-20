using System.Collections.Generic;
using TaskManagerClient.BusinessObjects.TaskManager;

namespace ProcessStats.Dev
{
    public enum CardSize
    {
        S = 0,
        M = 1,
        L = 2,
        XL = 3,
        NoSize = 4
    }

    public class CardSizeParser
    {
        private static readonly CardLabel S = new CardLabel { Color = CardLabelColor.Black, Name = "S" };
        private static readonly CardLabel M = new CardLabel { Color = CardLabelColor.Black, Name = "M" };
        private static readonly CardLabel L = new CardLabel { Color = CardLabelColor.Black, Name = "L" };
        private static readonly CardLabel XL = new CardLabel { Color = CardLabelColor.Black, Name = "XL" };

        public static CardSize TryParse(HashSet<CardLabel> labelsSet)
        {
            if (labelsSet.Contains(S))
                return CardSize.S;
            if (labelsSet.Contains(M))
                return CardSize.M;
            if (labelsSet.Contains(L))
                return CardSize.L;
            if (labelsSet.Contains(XL))
                return CardSize.XL;
            return CardSize.NoSize;
        }
    }
}