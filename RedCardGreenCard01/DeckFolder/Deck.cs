using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedCardGreenCard01
{

    public class Deck
    {
        string classId = "Deck";
        Random rnd = new Random();
        public Card[] deck;
        //int cardsDelt = 0;
        

        public Deck()
        {
            DeckBuilderGUI db = new DeckBuilderGUI();
            deck = db.GetNewDeck();
            db.CleanUp();
            Shuffle(deck);
        }
        public void Load()
        {
            string functionId = $"{classId}.Load()";
        }
        public void Reset()
        {
            string fnId = $"{classId}.Reset()";
            DeckBuilderGUI db = new DeckBuilderGUI();
            deck = db.GetNewDeck();
            db.CleanUp();
            Shuffle(deck);
            Helpers.PrintDebug(fnId, $"deck cound after reset: {deck.Length}");
        }
        public void Shuffle(Card[] cards, int shuffleTimes = 200)
        {
            string functionId = $"{classId}.Shuffle(Card[], int<=>)";
            if(cards.Length < 2)
            {
                Helpers.PrintError(functionId, $"Can not shuffle {cards.Length} cards");
                return;
            }

            int temp1 = 0;
            int temp2 = 0;
            for (int i = 0; i < shuffleTimes; i++)
            {
                temp1 = rnd.Next(cards.Length);
                temp2 = rnd.Next(cards.Length);
                SwapPosition(cards, temp1, temp2);
            }
        }
        public void Shuffle(List<Card> arr, int shuffleTimes = 200)
        {
            string functionId = $"{classId}.Shuffle(List<Card>, int<=>)";
            if (arr.Count < 2)
            {
                Helpers.PrintError(functionId, $"Can not shuffle {arr.Count} cards");
                return;
            }
            int temp1 = 0;
            int temp2 = 0;
            for (int i = 0; i < shuffleTimes; i++)
            {
                temp1 = rnd.Next(arr.Count);
                temp2 = rnd.Next(arr.Count);
                SwapPosition(arr, temp1, temp2);
            }
        }
        void SwapPosition(Card[] cards, int idx1, int idx2)
        {
            string functionId = $"{classId}.SwapPosition(Card[], int, int)";
            Card temp = cards[idx1];
            cards[idx1] = cards[idx2];
            cards[idx2] = temp;
        }
        void SwapPosition(List<Card> cards, int idx1, int idx2)
        {
            string functionId = $"{classId}.SwapPosition(List<Card>, int, int)";
            Card temp = cards[idx1];
            cards[idx1] = cards[idx2];
            cards[idx2] = temp;
        }
        public Card[] Deal(int numberOfCards)
        {
            string functionId = $"{classId}.Deal(int)";
            if (CardsRemaining() < numberOfCards)
            {
                Helpers.PrintError(functionId, "Not enough cards in deck");
                return null;
            }
            Card[] cardsToDeal = new Card[numberOfCards];
            int dealIdx = 0;
            for (int i = 0; i < deck.Length; i++)
            {
                if (!deck[i].delt)
                {
                    deck[i].delt = true;
                    cardsToDeal[dealIdx] = deck[i];
                    dealIdx++;
                }
                if (dealIdx == numberOfCards)
                    break;
            }
            return cardsToDeal;
        }

        int CardsRemaining()
        {
            string functionId = $"{classId}.CardsRemaining()";
            int inDeck = 0;
            for (int i = 0; i < deck.Length; i++)
            {
                if (!deck[i].delt)
                    inDeck++;
            }
            return inDeck;
        }
        
        #region debug/test fuctions
        public void PrintCards(Card[] arr)
        {
            string functionId = $"{classId}.PrintCards(Card[])";
            DeckHelpers.PrintCards(arr);
        }
        public void PrintCards()
        {
            string functionId = $"{classId}.PrintCards()";
            DeckHelpers.PrintCards(deck);
        }
        #endregion debug/test fuctions



    }
}


#region moved to own class
//public class DeckBuilder
//{
//    string[] suits = { "Clubs", "Diamonds", "Hearts", "Spades" };
//    string[] names = { "Ace","Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Jack", "Queen", "King" };

//    public DeckBuilder()
//    {
//    }

//    public Card[] GetNewDeck()
//    {
//        int idx = 0;
//        Card[] d = new Card[52];
//        for(int i = 0; i < suits.Length; i++)
//        {
//            Card[] s = BuildSuit(suits[i], (i+1));
//            for(int j = 0; j < s.Length; j++)
//            {
//                d[idx] = s[j];
//                idx++;
//            }
//        }
//        return d;
//    }
//    Card[] BuildSuit(string suit, int suitValue)
//    {
//        Card[] carr = new Card[names.Length];
//        for(int i = 0; i < names.Length; i++)
//        {
//            Card c = new Card();
//            c.Load(suit, suitValue, names[i], (i+1));
//            carr[i] = c;
//        }
//        return carr;
//    }

//    public void PrintCards(Card[] arr)
//    {
//        Card c;
//        for(int i = 0; i < arr.Length; i++)
//        {
//            c = arr[i];
//            Console.WriteLine($"{c.suit, -10} {c.suitValue, -8} {c.name, -10} {c.value, 2}");
//        }
//    }


//} // end of class DeckBuilder
#endregion moved to own class
