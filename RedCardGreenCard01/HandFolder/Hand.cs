using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RedCardGreenCard01
{
    public class Hand
    {
        string classId = "Hand";
        public string playerName;
        public Card[] cards;
        
        public Size cardSize;
        public int maxCards = -1;
        public Card patternCard;
        Card emptyCard;
        public Point deckPos;
        public Point playPos;

        public Hand()
        {
            string functionId = $"{classId}.Hand()";
            playerName = "Not Sure";
            deckPos = new Point();
            playPos = new Point();

            emptyCard = new Card();
            patternCard = new Card();
            emptyCard.name = $"emptyCard";
            patternCard.name = $"patternCard";

            emptyCard.SetImageFront(DeckHelpers.SetCardImageFromCardSheet(4, 6));
            patternCard.SetImageFront(DeckHelpers.SetCardImageFromCardSheet(4, 0));
            cardSize = patternCard.imageFront.Size;
        }
        public void Reset()
        {
            cards = null;
        }
        public void AddCardToHand(Card[] c)
        {
            string functionId = $"{classId}.AddCardToHand(Card[])";
            if(c == null)
            {
                Helpers.PrintError(functionId, "NullPassedInError");
                return;
            }
            if (cards == null || cards.Length == 0 )
                cards = c;
            else
            {
                Card[] temp = cards;
                cards = new Card[cards.Length + c.Length];
                for(int i = 0; i < temp.Length; i++)
                {
                    cards[i] = temp[i];
                }
                for(int i = temp.Length; i < cards.Length; i++)
                {
                    cards[i] = c[i - temp.Length];
                }
            }
        }
        void AddCardToHand(Card c)
        {
            string functionId = $"{classId}.AddCardToHand(Card)";
            Card[] temp = new Card[cards.Length + 1];
            for (int i = 0; i < cards.Length; i++)
                temp[i] = cards[i];
            temp[temp.Length - 1] = c;
            cards = temp;
            return;
        }
        public Card EmptyCard()
        {
            string functionId = $"{classId}.EmptyCard()";
            if (emptyCard == null)
                Helpers.PrintError(functionId, $"Card is null");
            if (playPos == null)
                Helpers.PrintError(functionId, $"Player position is null");
            emptyCard.position = playPos;
            return emptyCard;
        }

        public Card PlayCard()
        {
            Card c = null;
            string functionId = $"{classId}.PlayCard()";
            if (CardsInHand() < 1)
            {
                Helpers.PrintDebug(functionId, $"{playerName} has no cards to play, returning null");
                return c;
            }

            c = cards[0];
            c.position = playPos;
            Card[] temp = new Card[cards.Length - 1];
            for (int i = 1; i < cards.Length; i++)
                temp[i-1] = cards[i];
            cards = temp;
            return c;
        }
        
        public int CardsInHand()
        {
            string functionId = $"{classId}.CardsInHand()";
            return cards.Length;   
        }


        // not used atm
        public void SetPatternCardImage(Bitmap image)
        {
            string functionId = $"{classId}.SetPatternCardImage(Bitmap)";
            patternCard.SetImageFront(image);
        }
        public void PrintCards(Card[] cards)
        {
            DeckHelpers.PrintCards(cards);
        }
    }
}
