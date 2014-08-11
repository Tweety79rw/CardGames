using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Threading;
using System.Windows.Media;


namespace pokerGame
{
    
    class pokerViewModel : ViewModelBase
    {
        static Thread mainThread;
        private Thread flipThread;
        private PokerModel _poker;
        private bool _firstCardHold;
        private bool _secondCardHold;
        private bool _thirdCardHold;
        private bool _fourthCardHold;
        private bool _fifthCardHold;
        private bool cardsFliped;
        private String _winCond;
        private int _winMultiplier;
        private int draw;
        public pokerViewModel()
        {
            Winner = "This is Where a win is shown";
            mainThread = Thread.CurrentThread;
            mainThread.Name = "mainThread";
            cardsFliped = false;
            _colorSwitch = (Brush)new BrushConverter().ConvertFromString("black");
            DealCards = new DelegateCommand(onDealCardsClicked);
            AddFive = new DelegateCommand(onAddFiveClicked);
            RemoveFive = new DelegateCommand(onRemoveFiveClicked);
            MaxBet = new DelegateCommand(onMaxBetClicked);
            poker = new PokerModel();
            draw = 0;
            if (!cardsFliped)
                new Thread(flipCards).Start();
            
        }

        private void onMaxBetClicked()
        {
            //bool thread = false;
            //if (!cardsFliped)
            //{
            //    thread = true;
            //    flipThread = new Thread(flipCards);
                new Thread(flipCards).Start();
            //}
            
            if(poker._bet < 50 && poker._cash >= 50)
            {
                firstCardHold = secondCardHold = thirdCardHold = fourthCardHold = fifthCardHold = false;
                poker._bet = 50;
                poker._cash -= 50;
                OnPropertyChanged("Bet");
                OnPropertyChanged("Cash");
                //if (thread && Thread.CurrentThread.Name == "mainThread" && flipThread.ThreadState != ThreadState.Unstarted)
                //    flipThread.Join();
               // onDealCardsClicked();
            }
            
        }

        private void onRemoveFiveClicked()
        {
            if (!cardsFliped)
                new Thread(flipCards).Start();
            if(poker._bet > 0)
            {
                firstCardHold = secondCardHold = thirdCardHold = fourthCardHold = fifthCardHold = false;
                poker._bet -= 5;
                poker._cash += 5;
                OnPropertyChanged("Bet");
                OnPropertyChanged("Cash");
            }
        }

        private void onAddFiveClicked()
        {
            if(!cardsFliped)
                new Thread(flipCards).Start();
            if (poker._bet < 50 && poker._cash >= 5)
            {
                firstCardHold = secondCardHold = thirdCardHold = fourthCardHold = fifthCardHold = false;
               poker._bet += 5;
               poker._cash -= 5;
               OnPropertyChanged("Bet");
               OnPropertyChanged("Cash");
            }
        }
        private void flipCards()
        {
            cardsFliped = true;
            firstCard = poker.cards.getCardImage(52);
            Thread.Sleep(50);
            secondCard = poker.cards.getCardImage(52);
            Thread.Sleep(50);
            thirdCard = poker.cards.getCardImage(52);
            Thread.Sleep(50);
            fourthCard = poker.cards.getCardImage(52);
            Thread.Sleep(50);
            fifthCard = poker.cards.getCardImage(52);
            Thread.Sleep(50);

        }
        public void onDealCardsClicked()
        {
            if(poker._bet > 0)
            {
                
                switch (draw)
                {
                    case 0:
                        
                        poker.cards.shuffle();
                        firstCardHold = secondCardHold = thirdCardHold = fourthCardHold = fifthCardHold = false;
                        firstCard = poker.cards.getCardImage(poker.cards.dealCard());
                        secondCard = poker.cards.getCardImage(poker.cards.dealCard());
                        thirdCard = poker.cards.getCardImage(poker.cards.dealCard());
                        fourthCard = poker.cards.getCardImage(poker.cards.dealCard());
                        fifthCard = poker.cards.getCardImage(poker.cards.dealCard());
                        ++draw;
                        Winner = "Draw " + draw.ToString();
                        break;
                    case 2:
                        if(!_firstCardHold)
                            firstCard = poker.cards.getCardImage(poker.cards.dealCard());
                        if(!_secondCardHold)
                            secondCard = poker.cards.getCardImage(poker.cards.dealCard());
                        if(!_thirdCardHold)
                            thirdCard = poker.cards.getCardImage(poker.cards.dealCard());
                        if(!_fourthCardHold)
                            fourthCard = poker.cards.getCardImage(poker.cards.dealCard());
                        if(!_fifthCardHold)
                            fifthCard = poker.cards.getCardImage(poker.cards.dealCard());
    //                    if(_firstCardHold && _secondCardHold && _thirdCardHold && _fourthCardHold && _fifthCardHold)
                            ++draw;
                        Winner = "Draw " + draw.ToString();
                        break;
                    case 1:
                        cardsFliped = false;
                        if(!_firstCardHold)
                            firstCard = poker.cards.getCardImage(poker.cards.dealCard());
                        if(!_secondCardHold)
                            secondCard = poker.cards.getCardImage(poker.cards.dealCard());
                        if(!_thirdCardHold)
                            thirdCard = poker.cards.getCardImage(poker.cards.dealCard());
                        if(!_fourthCardHold)
                            fourthCard = poker.cards.getCardImage(poker.cards.dealCard());
                        if(!_fifthCardHold)
                            fifthCard = poker.cards.getCardImage(poker.cards.dealCard());
                        draw = 0;
                        firstCardHold = secondCardHold = thirdCardHold = fourthCardHold = fifthCardHold = false;
     //                   Winner = poker.checkHand();
                        isWinner();
                        payout();

                        break;

                }
            }
            else
            {
                for(int i = 0; i < 10; i++)
                {
                    new Thread(flash).Start();
                }
            }
        }
        private void payout()
        {
            poker._cash += poker._bet * _winMultiplier;
            OnPropertyChanged("Bet");
            OnPropertyChanged("Cash");
            Bet = "0";
        }
        public PokerModel poker
        {
            get
            {
                return _poker;
            }
            set
            {
                _poker = value;
            }
        }
        public String Bet
        {
            get
            {
                return poker._bet.ToString("C");
            }
            set
            {
                poker._bet = int.Parse(value);
                OnPropertyChanged("Bet");
            }

        }
        public String Cash
        {
            get
            {
                return poker._cash.ToString("C");
            }
            set
            {
                poker._cash = Convert.ToInt32(value);
                OnPropertyChanged("Cash");
            }
        }
        public bool firstCardHold
        {
            get
            {
                return _firstCardHold;
            }
            set
            {
                _firstCardHold = value;
                OnPropertyChanged("firstCardHold");
            }
        }
        public bool secondCardHold
        {
            get
            {
                return _secondCardHold;
            }
            set
            {
                _secondCardHold = value;
                OnPropertyChanged("secondCardHold");
            }
        }
        public bool thirdCardHold
        {
            get
            {
                return _thirdCardHold;
            }
            set
            {
                _thirdCardHold = value;
                OnPropertyChanged("thirdCardHold");
            }
        }
        public bool fourthCardHold
        {
            get
            {
                return _fourthCardHold;
            }
            set
            {
                _fourthCardHold = value;
                OnPropertyChanged("fourthCardHold");
            }
        }
        public bool fifthCardHold
        {
            get
            {
                return _fifthCardHold;
            }
            set
            {
                _fifthCardHold = value;
                OnPropertyChanged("fifthCardHold");
            }
        }
        private void isWinner()
        {
            int result = poker.checkHand();
            String StringResults = "";
            if (result >= 0 && result <= 12)
            {
                _winMultiplier = 0;
                StringResults = "Jacks or Better to Win, High card is ";
                if (result == 0)
                    StringResults += "Ace";
                else if (result == 12)
                    StringResults += "King";
                else if (result == 11)
                    StringResults += "Queen";
                else if (result == 10)
                    StringResults += "Jack";
                else
                {
                    int temp = result + 1;
                    StringResults += temp.ToString();
                }
                    
            }
            else
            {
                switch (result)
                {
                    
                    case 13:
                        StringResults = "Winner One Pair";
                        _winMultiplier = 1;
                        break;
                    case 14:
                        StringResults = "Winner Two Pair";
                        _winMultiplier = 2;
                        break;
                    case 15:
                        StringResults = "Winner Three of a Kind";
                        _winMultiplier = 3;
                        break;
                    case 16:
                        StringResults = "Winner Full House";
                        _winMultiplier = 9;
                        break;
                    case 17:
                        StringResults = "Winner Four of a Kind";
                        _winMultiplier = 25;
                        break;
                    case 18:
                        StringResults = "Winner Flush";
                        _winMultiplier = 6;
                        break;
                    case 19:
                        StringResults = "Winner Streight";
                        _winMultiplier = 4;
                        break;
                    case 20:
                        StringResults = "Winner Streight Flush";
                        _winMultiplier = 50;
                        break;
                    case 21:
                        StringResults = "Winner Royal Flush, good job";
                        _winMultiplier = 250;
                        break;
                    default:
                        StringResults = "Error With Checking Cards";
                        _winMultiplier = 1;
                        break;

                }
            }



            Winner = StringResults;
        }
        public void onFirstCardHoldClicked()
        {

            //new Thread(runLoop).Start();
            firstCard = poker.cards.getCardImage(1);
            secondCard = poker.cards.getCardImage(2);
            thirdCard = poker.cards.getCardImage(4);
            fourthCard = poker.cards.getCardImage(5);
            fifthCard = poker.cards.getCardImage(3);
        }
        public String Winner
        {
            get
            {
                return _winCond;   
            }
            set
            {
                _winCond = value;
                OnPropertyChanged("Winner");
            }
        }
        public CroppedBitmap firstCard 
        {
            get
            {
                return poker.cards.getCardImage(poker._firstCard);
            }
            set
            {
                poker._firstCard = poker.cards.getImageNumber(value);
                OnPropertyChanged("firstCard");
            }
        }
        public CroppedBitmap secondCard 
        {
            get
            {
                return poker.cards.getCardImage(poker._secondCard);
            }
            set
            {
                poker._secondCard = poker.cards.getImageNumber(value);
                OnPropertyChanged("secondCard");
            }
        }
        public CroppedBitmap thirdCard 
        { 
            get
            {
                return poker.cards.getCardImage(poker._thirdCard);
            }
            set
            {
                poker._thirdCard = poker.cards.getImageNumber(value);
                OnPropertyChanged("thirdCard");
            }
        }
        public CroppedBitmap fourthCard 
       { 
            get
            {
                return poker.cards.getCardImage(poker._fourthCard);
            }
            set
            {
                poker._fourthCard = poker.cards.getImageNumber(value);
                OnPropertyChanged("fourthCard");
            }
        }
        public CroppedBitmap fifthCard 
        { 
            get
            {
                return poker.cards.getCardImage(poker._fifthCard);
            }
            set
            {
                poker._fifthCard = poker.cards.getImageNumber(value);
                OnPropertyChanged("fifthCard");
            }
        }
        public void runLoop()
        {
            for (int i = 0; i < 52; i++)
            {
                
                    firstCard = poker.cards.getCardImage(poker.cards.dealCard());
                secondCard = poker.cards.getCardImage(poker.cards.dealCard());
                
                    thirdCard = poker.cards.getCardImage(poker.cards.dealCard());
                    fourthCard = poker.cards.getCardImage(poker.cards.dealCard());
                    fifthCard = poker.cards.getCardImage(poker.cards.dealCard());
               
                Thread.Sleep(220);
                //i++;
            }
        }
        private void flash()
        {
            for (int i = 0; i < 10; i++)
            {
                _colorSwitch = (Brush)new BrushConverter().ConvertFromString("red");
                OnPropertyChanged("colorSwitch");
                Thread.Sleep(100);
                _colorSwitch = (Brush)new BrushConverter().ConvertFromString("black");
                OnPropertyChanged("colorSwitch");
                Thread.Sleep(100);
            }
        }
        private Brush _colorSwitch;
        public Brush colorSwitch
        {
            get
            {
                return _colorSwitch;
            }
            set
            {
                _colorSwitch = value;
                OnPropertyChanged("colorSwitch");
            }
        }
        public ICommand DealCards { get; set; }
        public ICommand AddFive { get; set; }
        public ICommand RemoveFive { get; set; }
        public ICommand MaxBet { get; set; }
    }
}
