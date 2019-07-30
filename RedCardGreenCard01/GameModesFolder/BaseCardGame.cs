using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedCardGreenCard01
{
    public class BaseCardGame
    {
        string classId = "BaseCardGame";
        public Deck deck;
        public Hand[] hands;    // players
        protected int cardsRequired = 52;
        protected int minlayers = 1;
        protected int maxPlayers = -1;
        protected int numberOfPlayers = 1;
        protected int playerTurn = 0;

        protected BaseCardGame()
        {
            string functionId = $"{classId}.BaseCardGame()";
            deck = new Deck();
        }
        
        protected void LoadPlayerHands(int numberOfPlayers)
        {
            string functionId = $"{classId}.LoadPlayerHands(int)";
            hands = new Hand[numberOfPlayers];
            for (int i = 0; i < numberOfPlayers; i++)
                hands[i] = new Hand();
        }

        protected void DealCardsToPlayer(ref Hand player, int numberOfCards)
        {
            string functionId = $"{classId}.DealCardsToPlayer(ref Hand, int)";
            player.AddCardToHand(deck.Deal(numberOfCards));
        }

    } // end of class BaseCardGame
}
