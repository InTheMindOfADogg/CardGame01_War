using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

/// <summary>
/// 07/03/2018
/// 
/// 07/09/2018
/// TODO: make game run off 1 position instead of war pos and player normal pos
/// 
/// 07/12/2018
/// Commented out p1PlayCard and p2PlayCard, planning to remove later
/// commented out bool winnerSet
/// commented out Stopwatch for now
/// commented SetPlayCardsToEmpty() - no longer used
/// 
/// 
/// </summary>
namespace RedCardGreenCard01
{
    public class War
    {
        string classId = "War";
        Size formSize = new Size();
        
        Deck deck;
        Hand p1;
        Hand p2;
        
        List<Card> p1WarCards = new List<Card>();
        List<Card> p2WarCards = new List<Card>();

        Point p1WarPosition = new Point();
        Point p2WarPosition = new Point();

        int cardsPerPlayer = 26;

        Color bgColor;
        //Stopwatch watch = new Stopwatch();
        int roundsPlayed = 0;
        bool gameOver = false;
        bool inWar = false;
        bool roundStarted = false;

        public bool InWar() { return inWar; }
        public bool GameOver() { return gameOver; }
        public bool RoundStarted() { return roundStarted; }
        public int RoundsPlayed() { return roundsPlayed; }

        int roundsOfWar = 3;
        int warRoundCount = 0;

        int warCardOffsetX = 15;

        int winner = -1;
        public int Winner() { return winner; }

        public War(Size formSize)
        {
            string functionId = $"{classId}.War(Size)";
            this.formSize = formSize;
            Init();
        }
        public War(Form formRef)
        {
            string functionId = $"{classId}.War(Size)";
            this.formSize = formRef.ClientSize;
            this.bgColor = formRef.BackColor;
            Init();
        }
        void Init()
        {
            string functionId = $"{classId}.Init()";
            deck = new Deck();
            p1 = new Hand();
            p2 = new Hand();

            p1.playerName = "player1";

            //SetPlayCardsToEmpty();
            SetPlayersDeckPosition();
            SetPlayersPlayPosition();
            SetWarPositions();
        }

        void SetPlayersDeckPosition()
        {
            int p1x = (formSize.Width / 2) - (p1.patternCard.Size().Width / 2);
            int p2x = (formSize.Width / 2) - (p2.patternCard.Size().Width / 2);

            //int p1y = (int)((formSize.Height / 10) * 0.5);
            //int p2y = (int)((formSize.Height / 10) * 9.5) - (p2.patternCard.Size().Height);
            int p1y = (int)((formSize.Height / 10) * 9.5) - (p1.patternCard.Size().Height);
            int p2y = (int)((formSize.Height / 10) * 0.5);

            p1.deckPos = new Point(p1x, p1y);
            p2.deckPos = new Point(p2x, p2y);
        }
        void SetPlayersPlayPosition()
        {
            int p1x = (formSize.Width / 2) - (p1.patternCard.Size().Width / 2);
            int p2x = (formSize.Width / 2) - (p2.patternCard.Size().Width / 2);
            int p1y = ((formSize.Height / 2)) + 5;
            int p2y = ((formSize.Height / 2)) - (p2.patternCard.Size().Height) - 5;
            p1.playPos = new Point(p1x, p1y);
            p2.playPos = new Point(p2x, p2y);
        }
        void SetWarPositions()
        {
            p1WarPosition.X = p1.playPos.X + p1.cardSize.Width + 10;
            p2WarPosition.X = p2.playPos.X + p2.cardSize.Width + 10;

            p1WarPosition.Y = p1.playPos.Y;
            p2WarPosition.Y = p2.playPos.Y;
        }
        public void Load()
        {
            string functionId = $"{classId}.Load()";
            p1.AddCardToHand(Deal(cardsPerPlayer));
            p2.AddCardToHand(Deal(cardsPerPlayer));
        }
        public void ResetGame()
        {
            deck.Reset();
            p1.Reset();
            p2.Reset();
            p1.AddCardToHand(Deal(cardsPerPlayer));
            p2.AddCardToHand(Deal(cardsPerPlayer));
            inWar = false;
            winner = -1;
            roundStarted = false;
            warRoundCount = 0;
            roundsPlayed = 0;
            ClearWarCards();
            
            
        }

        public Card[] Deal(int numberOfCards)
        {
            string functionId = $"{classId}.Deal(int)";
            return deck.Deal(numberOfCards);
        }

        public void PlayCardClickController()
        {
            if (!RoundStarted())
                StartRound();
            
        }
        public void StartRound()
        {
            string functionId = $"{classId}.StartRound()";
            
            bool p1OutOfCards = false;
            bool p2OutOfCards = false;
            p1OutOfCards = (p1.CardsInHand() < 1) ? true : false;
            p2OutOfCards = (p2.CardsInHand() < 1) ? true : false;
            gameOver = (p1OutOfCards || p2OutOfCards) ? true : false;
            
            if (gameOver)
            {
                Helpers.PrintDebug(functionId, $"Checking if the game is over");
                GiveCardsToWinner(winner); // clears the last set of cards for now
                gameOver = (p1OutOfCards || p2OutOfCards) ? true : false;
                if(gameOver)
                {
                    Helpers.PrintDebug(functionId, "Game over... Play Again?");
                }

                return;
            }
            if (!p1OutOfCards && !p2OutOfCards)
            {
                Helpers.PrintDebug(functionId, $"Lets Play ");
                roundsPlayed++;
            }

            if (roundStarted == false)
                roundStarted = true;
        }
        
        public void Update()
        {
            string functionId = $"{classId}.Update()";
            if (roundStarted)
            {
                EndRound();
                PlayNormalRound();
                SetGamePhase();
                //return;
            }
            if (inWar)
            {
                if (warRoundCount < roundsOfWar)
                {
                    PlayWarRounds();
                    warRoundCount++;
                }
                if (warRoundCount == roundsOfWar)
                {
                    PlayNormalRound();
                    warRoundCount = 0;
                    inWar = false;
                    SetGamePhase();
                }
            }
        } // end of Update()
        
        void PlayNormalRound()
        {
            if(!inWar)
            {
                PlayCard(p1, p1.playPos, false, p1WarCards);
                PlayCard(p2, p2.playPos, false, p2WarCards);
            }
            if(inWar)
            {
                PlayCard(p1, p1WarPosition, false, p1WarCards);
                PlayCard(p2, p2WarPosition, false, p2WarCards);
                p1WarPosition.X += warCardOffsetX;
                p2WarPosition.X += warCardOffsetX;
            }
        }
        void PlayWarRounds()
        {
            if (p1.CardsInHand() > 1)
            {
                PlayCard(p1, p1WarPosition, true, p1WarCards);
                //Console.WriteLine($"p1WarPosisiton: {p1WarPosition.ToString()} {P1LastPlayedCardValue()}\n");
                p1WarPosition.X += warCardOffsetX;
            }
            if (p2.CardsInHand() > 1)
            {
                PlayCard(p2, p2WarPosition, true, p2WarCards);
                //Console.WriteLine($"p2WarPosisiton: {p2WarPosition.ToString()} {P2LastPlayedCardValue()}\n");
                p2WarPosition.X += warCardOffsetX;
            }
        }
        

        void SetGamePhase()
        {
            int p1val = P1LastPlayedCardValue();
            int p2val = P2LastPlayedCardValue();
            if (p1val > 0 && p2val > 0)
            {
                if (p1val > p2val)
                    winner = 1;
                if (p1val < p2val)
                    winner = 2;
                if (p1val == p2val)
                    inWar = true;
            }
        }
        //void SetGamePhase(ref int p1val, ref int p2val)
        //{
        //    p1val = P1LastPlayedCard();
        //    p2val = P2LastPlayedCard();
        //    if (p1val > 0 && p2val > 0)
        //    {
        //        if (p1val > p2val)
        //            winner = 1;
        //        if (p1val < p2val)
        //            winner = 2;
        //        if (p1val == p2val)
        //            inWar = true;
        //    }
        //}
        public void EndRound()
        {
            GiveCardsToWinner(winner);
            ResetRound();
        }
        void ResetRound()
        {
            inWar = false;
            winner = -1;
            roundStarted = false;
            warRoundCount = 0;
            SetWarPositions();
        }
        void GiveCardsToWinner(int playerNumber)
        {
            List<Card> temp = new List<Card>();
            if (p1WarCards.Count > 0 && p2WarCards.Count > 0)
            {
                Hand player = p1;
                if (playerNumber == 2) player = p2;
                temp.AddRange(p1WarCards);
                temp.AddRange(p2WarCards);
                deck.Shuffle(temp);
                player.AddCardToHand(temp.ToArray());
                ClearWarCards();
            }
        }
        void GiveCardsToWinner(Hand player)
        {
            List<Card> temp = new List<Card>();
            if (p1WarCards.Count > 0 && p2WarCards.Count > 0)
            {
                temp.AddRange(p1WarCards);
                temp.AddRange(p2WarCards);
                deck.Shuffle(temp);
                player.AddCardToHand(temp.ToArray());
                ClearWarCards();
            }
        }
        int P1LastPlayedCardValue()
        {
            int val = -1;

            Card c = LastPlayedCard(p1WarCards);

            if (c != null && !c.faceDown)
                val = c.value;
            return val;
        }
        int P2LastPlayedCardValue()
        {
            int val = -1;
            Card c = LastPlayedCard(p2WarCards);
            if (c != null && !c.faceDown)
                val = c.value;
            return val;
        }
        Card LastPlayedCard(List<Card> cards)
        {
            string functionId = $"{classId}.LastPlayedCard(List<Card>)";
            if (cards.Count > 0)
                return cards[cards.Count - 1];
            Helpers.PrintError(functionId, $"The passed in list has no cards.");
            return null;
        }

        Card PlayCard(Hand player, Point pos, bool faceDown, List<Card> addToStack)
        {
            string functionId = $"{classId}.PlayCard(Hand, List<Card>)";
            if (player.CardsInHand() > 0)
            {
                Card c = player.PlayCard();
                c.position = pos;
                c.faceDown = faceDown;
                addToStack.Add(c);
                return c;
            }
            Helpers.PrintError(functionId, $"{player.playerName} does not have any cards to play!");
            return null;

        }

        void ClearWarCards()
        {
            string functionId = $"{classId}.ClearWarCards()";
            p1WarCards = new List<Card>();
            p2WarCards = new List<Card>();
        }



        public Bitmap ReadyFrameForRender()
        {
            string functionId = $"{classId}.ReadyFrameForRender()";
            Bitmap frame;
            frame = new Bitmap(formSize.Width, formSize.Height);
            Graphics g = Graphics.FromImage(frame);
            g.FillRectangle(Brushes.CornflowerBlue, new RectangleF(0, 0, frame.Width, frame.Height));


            DrawCardCountInHand(g, p1);
            DrawCardCountInHand(g, p2);


            DrawImage(g, p1.patternCard.imageFront, p1.deckPos);
            DrawImage(g, p2.patternCard.imageFront, p2.deckPos);

            RenderCards(g, p1WarCards);
            RenderCards(g, p2WarCards);

            g.Dispose();
            return frame;
        }

        public void RenderGame(Graphics g)
        {
            string functionId = $"{classId}.RenderGame(Graphics)";
            Bitmap frame = ReadyFrameForRender();
            g.DrawImage(frame, 0, 0);
            frame.Dispose();
        }

        void DrawCardCountInHand(Graphics g, Hand playerHand)
        {
            string functionId = $"{classId}.DrawCardCountInHand(Hand)";
            string str = $"Cards In Hand: {playerHand.CardsInHand().ToString()}";
            Point drawLoc = new Point();
            drawLoc.X = playerHand.deckPos.X + (playerHand.cardSize.Width);
            drawLoc.Y = playerHand.deckPos.Y;
            DrawLabel(g, str, drawLoc);
        }
        public void RenderCards(Graphics g, Card[] cards)
        {
            for (int i = 0; i < cards.Length; i++)
            {
                cards[i].RenderCard(g);
            }
        }
        public void RenderCards(Graphics g, List<Card> cards)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                cards[i].RenderCard(g);
            }
        }
        void RenderCard(Graphics g, Card card)
        {
            string functionId = $"{classId}.DrawCard(Graphics, Card)";
            if (card != null && card.imageFront != null)
            {
                DrawImage(g, card.imageFront, card.position);
                return;
            }
            if (card == null)
                Helpers.PrintError(functionId, $"unable to draw card ({card.name}) - card is null");
            if (card.imageFront == null)
                Helpers.PrintError(functionId, $"unable to draw card ({card.name}) - card image is null");
        }


        void DrawImage(Graphics g, Bitmap bitmap, Point location)
        {
            string functionId = $"{classId}.DrawImage(Graphics, Bitmap, Point)";
            if (bitmap != null)
            {
                g.DrawImage(bitmap, location);
                return;
            }
            if (bitmap == null)
                Helpers.PrintError(functionId, $"Bitmap is null");
            if (location == null)
                Helpers.PrintError(functionId, $"Location is null");
        }
        void DrawLabel(Graphics g, string str, Point location)
        {
            string functionId = $"{classId}.Label(Graphics, string, Point)";
            Font font = new Font("Arial", 14);
            SizeF s = g.MeasureString(str, font);
            g.DrawString(str, font, Brushes.Black, location);
            font.Dispose();
        }


        #region Functions to draw grid lines
        void DrawTestGrid(Graphics g)
        {
            string functionId = $"{classId}.DrawTestGrid(Graphics)";
            DrawTestLinesHorizontal(g);
            DrawTestLinesVerticle(g);
        }
        void DrawTestLinesHorizontal(Graphics g)
        {
            string functionId = $"{classId}.DrawTestLinesHorizontal(Graphics)";
            Point startPos = new Point();
            Point endPos = new Point();
            startPos.X = 0;
            endPos.X = formSize.Width;

            while (startPos.Y <= formSize.Height)
            {
                g.DrawLine(Pens.Black, startPos, endPos);
                startPos.Y += formSize.Height / 10;
                endPos.Y += formSize.Height / 10;
            }
        }
        void DrawTestLinesVerticle(Graphics g)
        {
            string functionId = $"{classId}.DrawTestLinesVerticle(Graphics)";
            Point startPos = new Point();
            Point endPos = new Point();
            startPos.Y = 0;
            endPos.Y = formSize.Height;

            while (startPos.X <= formSize.Width)
            {
                g.DrawLine(Pens.Black, startPos, endPos);
                startPos.X += formSize.Width / 10;
                endPos.X += formSize.Width / 10;
            }
        }
        #endregion Functions to draw grid lines

        void PrintPlayersCardCountToConsole()
        {
            string functionId = $"{classId}.PrintPlayersCardCount()";
            Helpers.PrintDebug(functionId, $"Player 1 number of cards: {p1.CardsInHand()}");
            Helpers.PrintDebug(functionId, $"Player 2 number of cards: {p2.CardsInHand()}");
        }


    } // end of class War
}

#region removed 07/12/2018
// -Class lvl
//Card p1PlayCard;  
//Card p2PlayCard;  
//
// -In SetPlayCardsToEmpty()
//p1PlayCard = p1.EmptyCard();
//p2PlayCard = p2.EmptyCard();

//p1PlayCard.name = "p1PlayCard";
//p2PlayCard.name = "p2PlayCard";

//if (p1PlayCard.imageFront == null)
//    Helpers.PrintError(functionId, $"{p1PlayCard.name}.image is null");
//if (p2PlayCard.imageFront == null)
//    Helpers.PrintError(functionId, $"{p2PlayCard.name}.image is null");
#endregion removed 07/12/2018
#region old war logic
//if (inWar)
//{
//    while (warRoundCount < roundsOfWar)
//    {
//        if (p1.CardsInHand() > 1)
//        {
//            PlayCard(p1, p1WarPosition, true, p1WarCards);
//            p1WarPosition.X += 15;
//        }
//        if (p1.CardsInHand() > 1)
//        {
//            PlayCard(p2, p2WarPosition, true, p2WarCards);
//            p2WarPosition.X += 15;
//        }
//        warRoundCount++;
//    }
//    PlayCard(p1, p1WarPosition, false, p1WarCards);
//    PlayCard(p2, p2WarPosition, false, p2WarCards);
//    SetGamePhase(ref p1val, ref p2val);
//}
#endregion old war logic
#region warlogic 02
// Warlogic 02
//if (p1PlayCard.value == p2PlayCard.value)
//{
//    Helpers.PrintDebug(functionId, $"WARRRR!!! TODO: add war logic");
//    goingToWar = true;
//    // TODO: Add war logic here
//    Point p1WarPosition = p1.playPos;
//    Point p2WarPosition = p2.playPos;

//    p1WarPosition.X += p1.cardSize.Width;
//    p2WarPosition.X += p2.cardSize.Width;
//    int xSpace = 15;

//    int warcards = 3;
//    int counter = 0;
//    // WarLogic 02
//    counter = 3;
//    while (counter < warcards)
//    {
//        if (p1.CardsInHand() > 1)
//        {
//            temp = p1.PlayCard();
//            temp.faceDown = true;
//            temp.position = p1WarPosition;
//            p1WarCards.Add(temp);
//            p1WarPosition.X += xSpace;
//        }
//        if (p2.CardsInHand() > 1)
//        {
//            temp = p2.PlayCard();
//            temp.faceDown = true;
//            temp.position = p2WarPosition;
//            p2WarCards.Add(temp);
//            p2WarPosition.X += xSpace;
//        }
//        counter++;
//    }
//    temp = p1.PlayCard();
//    temp.faceDown = false;
//    temp.position = p1WarPosition;
//    p1WarCards.Add(temp);
//    p1WarPosition.X += xSpace;

//    temp = p2.PlayCard();
//    temp.faceDown = false;
//    temp.position = p2WarPosition;
//    p2WarCards.Add(temp);
//    p2WarPosition.X += xSpace;

//    if (IsWinner(p1WarCards[p1WarCards.Count - 1], p2WarCards[p2WarCards.Count - 1]))
//        goto CheckIfGameOver;
//}
#endregion warlogic 02
#region  StartRound logic 02 - snippit
//p1OutOfCards = (p1.CardsInHand() < 1) ? true : false;
//            p2OutOfCards = (p2.CardsInHand() < 1) ? true : false;
//            gameOver = (p1OutOfCards || p2OutOfCards) ? true : false;
//            if(gameOver)
//            {
//                Helpers.PrintDebug(functionId, $"Game is over, need to set up logic for next steps");
//                return;
//            }

//            #region old game over region, trying new logic
//            //if (p1OutOfCards && p2OutOfCards)
//            //{
//            //    Helpers.PrintDebug(functionId, $"Both players out of cards!!!");
//            //    return;
//            //}
//            //if (!p1OutOfCards && p2OutOfCards)
//            //{
//            //    Helpers.PrintDebug(functionId, $"Game Over! Player 1 Wins!!!");
//            //    return;
//            //}
//            //if (p1OutOfCards && !p2OutOfCards)
//            //{
//            //    Helpers.PrintDebug(functionId, $"Game Over! Player 2 Wins!!!");
//            //    return;
//            //}
//            #endregion old game over region, trying new logic
//            if (!p1OutOfCards && !p2OutOfCards)
//                Helpers.PrintDebug(functionId, $"Lets Play ");

//            // Play cards to start the round
//            p1PlayCard = p1.PlayCard();
//            p2PlayCard = p2.PlayCard();

//            p1PlayedCards.Add(p1PlayCard);
//            p2PlayedCards.Add(p2PlayCard);

//            //WarLogic:
//            if(p1PlayCard.value == p2PlayCard.value && !goingToWar)
//            {
//                Helpers.PrintDebug(functionId, $"WARRRR!!! TODO: add war logic");
//                goingToWar = true;

//            }

//            if (p1PlayCard.value == p2PlayCard.value)
//            {
//                Helpers.PrintDebug(functionId, $"WARRRR!!! TODO: add war logic");
//                goingToWar = true;
//                // TODO: Add war logic here
//                Point p1WarPosition = p1.playPos;
//Point p2WarPosition = p2.playPos;

//p1WarPosition.X += p1.cardSize.Width;
//                p2WarPosition.X += p2.cardSize.Width;
//                int xSpace = 15;
//Card temp;
//int warcards = 3;
//int counter = 0;
//WarLogic:
//                counter = 3;
//                while(counter<warcards)
//                {
//                    if (p1.CardsInHand() > 1)
//                    {
//                        temp = p1.PlayCard();
//                        temp.faceDown = true;
//                        temp.position = p1WarPosition;
//                        p1WarCards.Add(temp);
//                        p1WarPosition.X += xSpace;
//                    }
//                    if (p2.CardsInHand() > 1)
//                    {
//                        temp = p2.PlayCard();
//                        temp.faceDown = true;
//                        temp.position = p2WarPosition;
//                        p2WarCards.Add(temp);
//                        p2WarPosition.X += xSpace;
//                    }
//                    counter++;
//                }
//                temp = p1.PlayCard();
//                temp.faceDown = false;
//                temp.position = p1WarPosition;
//                p1WarCards.Add(temp);
//                p1WarPosition.X += xSpace;

//                temp = p2.PlayCard();
//                temp.faceDown = false;
//                temp.position = p2WarPosition;
//                p2WarCards.Add(temp);
//                p2WarPosition.X += xSpace;

//                if (IsWinner(p1WarCards[p1WarCards.Count - 1], p2WarCards[p2WarCards.Count - 1]))
//                    goto CheckIfGameOver;
//                else
//                    goto WarLogic;
//            }

//            IsWinner(p1PlayCard, p2PlayCard);
//#region trying to fit this into a function
////// Determine Winner
////if (p1PlayCard.value > p2PlayCard.value)
////{
////    Helpers.PrintDebug(functionId, $"Player 1 ({p1.playerName}) wins the round!");
////    //p1.AddCardToHand(p1PlayedCards.ToArray());
////    //p1.AddCardToHand(p2PlayedCards.ToArray());
////    //ClearPlayedCards();
////    SetWinnings(p1);

////}
////if (p1PlayCard.value < p2PlayCard.value)
////{
////    Helpers.PrintDebug(functionId, $"Player 2 ({p2.playerName}) wins the round!");
////    //p2.AddCardToHand(p2PlayedCards.ToArray());
////    //p2.AddCardToHand(p1PlayedCards.ToArray());
////    //ClearPlayedCards();
////    SetWinnings(p2);
////}
//#endregion trying to fit this into a function
//CheckIfGameOver:
//            #region game over logic
//            // Game Over Logic
//            if (p2.CardsInHand() == 0)
//            {
//                Helpers.PrintDebug(functionId, $"Game Over! Player 1 Wins!!! TODO: add end game logic");
//                SetPlayCardsToEmpty();
//                // end game logic here
//            }
//            if (p1.CardsInHand() == 0)
//            {
//                Helpers.PrintDebug(functionId, $"Game Over! Player 2 Wins!!! TODO: add end game logic");
//                SetPlayCardsToEmpty();
//                // end game logic here
//            }
//            #endregion game over logic
#endregion StartRound() logic 02 - snippit
#region moved drawing to buffer
//DrawTestGrid(g);
//g.DrawImage(p1.EmptyCard().image, p1.playPos);
//g.DrawImage(p2.EmptyCard().image, p2.playPos);

//DrawCardCountInHand(g, p1);
//DrawCardCountInHand(g, p2);

//if (p1PlayCard != null && p1PlayCard.image != null)
//{
//    DrawImage(g, p1PlayCard.image, p1PlayCard.position);
//}


//if (p2PlayCard != null && p2PlayCard.image != null)
//{
//    DrawImage(g, p2PlayCard.image, p2PlayCard.position);
//}
//DrawImage(g, p1.patternCard.image, p1.deckPos);
//DrawImage(g, p2.patternCard.image, p2.deckPos);
#endregion moved drawing to buffer
#region code merged into DrawCardCountInHand()
//string str = $"Cards In Hand: {p1.CardsInHand().ToString()}";
//Point drawLoc = new Point();
//drawLoc.X = p1.deckPos.X + (p1.cardSize.Width);
//Helpers.PrintDebug(functionId, $"p1.cardSize.Width: {p1.cardSize.Width}");
//drawLoc.Y = p1.deckPos.Y;
//DrawString(g, str, drawLoc);

//str = $"Cards In Hand: {p2.CardsInHand().ToString()}";
//drawLoc.X = p2.deckPos.X + (p2.cardSize.Width);
//drawLoc.Y = p2.deckPos.Y;
//DrawString(g, str, drawLoc);
#endregion code merged into DrawCardCountInHand()
#region StartRound() logic 01
//p1PlayCard = p1.PlayCard();
//if (p1PlayCard == null)
//{
//    Helpers.PrintDebug(functionId, $"p1PlayCard image is null (p1 out of cards)");
//    p1OutOfCards = true;
//    //return;
//}
//else
//    p1PlayCard.position = p1.playPos;


//p2PlayCard = p2.PlayCard();
//if (p2PlayCard == null)
//{
//    Helpers.PrintDebug(functionId, $"p2PlayCard image is null (p2 out of cards)");
//    p2OutOfCards = true;
//}
//else
//    p2PlayCard.position = p2.playPos;

//if (!p1OutOfCards && !p2OutOfCards)
//{
//    Helpers.PrintDebug(functionId, $"Both players out of cards!!!");
//    // play game
//}
//if (!p1OutOfCards && p2OutOfCards)
//{
//    Helpers.PrintDebug(functionId, $"Player 1 Wins!!!");
//    // player 1 wins
//}
//if (p1OutOfCards && !p2OutOfCards)
//{
//    Helpers.PrintDebug(functionId, $"Player 2 Wins!!!");
//    // player 2 wins
//}
#endregion StartRound() logic 01
#region old set players deck position and set play position
//Moved to SetPlayersDeckPosition
// set deckPos
//int p1x = (formSize.Width / 2) - (p1.patternCard.Size().Width / 2);
//int p2x = (formSize.Width / 2) - (p2.patternCard.Size().Width / 2);

//int p1y = ((formSize.Height / 10) /*) - (p1.patternCard.Size().Height*/ );
//int p2y = ((formSize.Height / 10) * 6) - (p2.patternCard.Size().Height);

//p1.deckPos = new Point(p1x, p1y);
//p2.deckPos = new Point(p2x, p2y);

//Moved to SetPlayersPlayPosition
// setting play position
//p1x = (formSize.Width / 2) - (p1.patternCard.Size().Width / 2);
//p2x = (formSize.Width / 2) - (p2.patternCard.Size().Width / 2);

//p1y = ((formSize.Height / 2)) - (p1.patternCard.Size().Height) - 5;
//p2y = ((formSize.Height / 2)) + 5;

//p1.playPos = new Point(p1x, p1y);
//p2.playPos = new Point(p2x, p2y);
#endregion old set players deck position and set play position

#region debug prints to console
//Helpers.PrintDebug(functionId, $"Player 1 position {p1.playPos}");
//Helpers.PrintDebug(functionId, $"Player 2 position {p2.playPos}");
//Console.WriteLine($"p1WarPosisiton: {p1WarPosition.ToString()} {P1LastPlayedCardValue()}\n");
//Console.WriteLine($"p2WarPosisiton: {p2WarPosition.ToString()} {P2LastPlayedCardValue()}\n");
#endregion debug prints to console