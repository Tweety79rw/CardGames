using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace pokerGame
{
    class PokerModel
    {
        public CardDeckClass cards;
        public int _firstCard { get; set; }
        public int _secondCard { get; set; }
        public int _thirdCard { get; set; }
        public int _fourthCard { get; set; }
        public int _fifthCard { get; set; }
        public int _cash { get; set; }
        public int _bet { get; set; }
        public int _win { get; set; }
        public PokerModel()
        {
            _firstCard = new int();
            _secondCard = new int();
            _thirdCard = new int();
            _fourthCard = new int();
            _fifthCard = new int();
            cards = new CardDeckClass();
            _cash = 500;
            _bet = 0;
        }
        List<int> cardNumbers = new List<int>();
        List<int> cardSuits = new List<int>();
        public int checkHand()
        {
            bool exitInner = false;
            bool jacks = false;
            bool pair = false;
            bool firstPair = false;
            bool twoPair = false;
            bool threeKind = false;
            bool firstThreeKind = false;
            bool fullHouse = false;
            bool fourKind = false;
            bool flush = true;
            bool streight = true;
            bool streightFlush = false;
            bool royalFlush = false;
            cardNumbers.Clear();
            cardSuits.Clear();
            cardNumbers.Add(_firstCard % 13);
            cardNumbers.Add(_secondCard % 13);
            cardNumbers.Add(_thirdCard % 13);
            cardNumbers.Add(_fourthCard % 13);
            cardNumbers.Add(_fifthCard % 13);
            cardSuits.Add(_firstCard / 13);
            cardSuits.Add(_secondCard / 13);
            cardSuits.Add(_thirdCard / 13);
            cardSuits.Add(_fourthCard / 13);
            cardSuits.Add(_fifthCard / 13);
            List<int> skipCards = new List<int>();
            int results;
            skipCards.Clear();
            for (int i = 0; i < 5; i++)
            {
                exitInner = false;
                for(int j = i+1; j<5; j++)
                {
                    for (int k = 0; k < skipCards.Count; k++ )
                    {
                        if (i == skipCards.ElementAt(k))
                        {
                            exitInner = true;
                            break;
                        }
                        else if (j == skipCards.ElementAt(k) && skipCards.ElementAt(k) < 4)
                        {
                            j++;
                            break;
                        }
                    }
                    if (exitInner)
                        break;
                        if (cardNumbers.ElementAt(i) == cardNumbers.ElementAt(j))
                        {
                            if (cardNumbers.ElementAt(i) > 9 || cardNumbers.ElementAt(i) == 0)
                                jacks = true;
                            flush = false;
                            streight = false;
                            if (fourKind)
                            {
                                break;
                            }
                            else if (threeKind)
                            {
                                fourKind = true;
                                threeKind = false;
                            }
                            else if (pair)
                            {
                                if (firstPair)
                                {
                                    fullHouse = true;
                                    firstPair = false;
                                }
                                else
                                {
                                    threeKind = true;
                                    pair = false;
                                }
                            }
                            else if (firstPair)
                            {
                                firstPair = false;
                                twoPair = true;
                            }
                            else if(twoPair)
                            {
                                fullHouse = true;
                                twoPair = false;
                            }
                            else
                            {
                                if (firstThreeKind)
                                {
                                    fullHouse = true;
                                    firstThreeKind = false;
                                    
                                }
                                else
                                    pair = true;
                            }
                            skipCards.Add(j);
                        }
                }
                if(pair)
                {
                    pair = false;
                    firstPair = true;
                }
                else if(threeKind)
                {
                    threeKind = false;
                    firstThreeKind = true;
                }
            }
            if(flush)
                for (int i = 0; i < 1; i++)
                {
                    for(int j = i + 1; j < 5; j++)
                    {
                        if(cardSuits.ElementAt(i) != cardSuits.ElementAt(j))
                        {
                            flush = false;
                            break;
                        }
                    }
                }
            if (streight)
            {
                cardNumbers.Sort();
                int streightCount = cardNumbers.ElementAt(4) - cardNumbers.ElementAt(0);
                bool aceHigh = (cardNumbers.ElementAt(4) == 12 && cardNumbers.ElementAt(3) == 11 && cardNumbers.ElementAt(2) == 10 && cardNumbers.ElementAt(1) == 9 && cardNumbers.ElementAt(0) == 0);
                if (streightCount != 4)
                    streight = false;
                if (aceHigh)
                    streight = true;
            }
            if(flush && streight)
            {
                flush = false;
                streight = false;
                streightFlush = true;
            }
            if(streightFlush)
            {
                if(cardNumbers.ElementAt(4) == 12)
                {
                    streightFlush = false;
                    royalFlush = true;
                }
            }

            if(firstPair && !fullHouse)
            {
                if (jacks)
                    results = 13;
                else
                    results = GetHighCard();
            }
            else if(twoPair)
            {
                results = 14;
            }
            else if(firstThreeKind)
            {
                results = 15;
            }
            else if(fullHouse)
            {
                results = 16;
            }
            else if(fourKind)
            {
                results = 17;
            }
            else if (flush)
            {
                results = 18;
            }
            else if(streight)
            {
                results = 19;
            }
            else if (streightFlush)
            {
                results = 20;
            }
            else if (royalFlush)
            {
                results = 21;
            }
            else
            {

                results = GetHighCard();
            }

                return results;
        }
        
        int GetHighCard()
        {
            int resultsHighCard;
            cardNumbers.Sort();
            if (cardNumbers.ElementAt(0) == 0)
                    resultsHighCard = 0;
                else
                    resultsHighCard = cardNumbers.ElementAt(4);
            return resultsHighCard;
        }
        
    }
}
