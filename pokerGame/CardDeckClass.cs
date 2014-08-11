using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using pokerGame;


namespace pokerGame
{
    class CardDeckClass
    {
        Dictionary<int,CroppedBitmap> cardImage = new Dictionary<int, CroppedBitmap>();
        Dictionary<CroppedBitmap, int> reverseCardImage = new Dictionary<CroppedBitmap, int>();
        private SpriteSheet _image = new SpriteSheet(new BitmapImage(new Uri("pack://application:,,,/images/windows-playing-cards.png")));
        private WriteableBitmap cardBack;
        private CroppedBitmap image;
        private List<int> cardDeck = new List<int>();
        Random random = new Random();
        int currentCard;
        public CardDeckClass()
        {
            cardBack = new WriteableBitmap(new BitmapImage(new Uri("pack://application:,,,/images/animal-grab-back.jpg")));
            currentCard = 0;
            buildDeck();
            reverseCardImage = cardImage.ToDictionary(x => x.Value, x => x.Key);
            shuffle();
        }
        
        private void buildDeck()
        {
            image = new CroppedBitmap(cardBack, new System.Windows.Int32Rect(0, 0, cardBack.PixelWidth, cardBack.PixelHeight));
            int count = 0;
            for(int i = 0; i < 4; i++)
            {
                for(int j = 0; j < 13; j++)
                {
                    cardDeck.Add(count);
                    cardImage.Add(count++, _image.GetBitmap(j * (_image.imageWidth() / 13), i * (_image.imageHeight() / 4), (_image.imageWidth() / 13), (_image.imageHeight() / 4)));

                }
            }
            cardImage.Add(count++, image);
        }
        public void shuffle()
        {
            currentCard = 0;
            for(int i = 0; i < 500; i++)
            {   
                Stack<int> left = new Stack<int>();
                Stack<int> right = new Stack<int>();
                int split = random.Next(20,30);
                for(int j=0; j< split; j++)
                {
                    left.Push(cardDeck.ElementAt(j));
                }
                for(int j= split; j < 52; j++)
                {
                    right.Push(cardDeck.ElementAt(j));
                }
                cardDeck.Clear();
                while(left.Count > 0 && right.Count > 0)
                {
                    int side = random.Next(0, 2);
                    if (side == 0)
                        cardDeck.Add(left.Pop());
                    else
                        cardDeck.Add(right.Pop());
                }
                while(left.Count > 0)
                {
                    cardDeck.Add(left.Pop());
                }
                while(right.Count > 0)
                {
                    cardDeck.Add(right.Pop());
                }
            }
        }
        public int dealCard()
        {
            if (currentCard < 45)
                return cardDeck.ElementAt(currentCard++);
            else
            {
                shuffle();
                return dealCard();
            }
        }
        public int size { get { return cardDeck.Count; } }
        public CroppedBitmap getCardImage(int value)
        {
            return cardImage[value];
        }
        public int getImageNumber(CroppedBitmap value)
        {
            return reverseCardImage[value];
        }
    }
    
}
