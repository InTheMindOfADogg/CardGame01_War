using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace RedCardGreenCard01
{
    // non graphical 
    public static class DeckHelpers
    {
        static string classId = "(static)DeckHelpers";
        static string resourcePath = Environment.CurrentDirectory + @"\..\..\Resources\";
        static string sheetFile = @"decksheet.png";
        static Size cardSize = new Size(81, 117);

        public static Bitmap SetCardImageFromCardSheet(int row, int column)
        {
            string functionId = $"{classId}.SetCardImageFromCardSheet(int, int)";
            Bitmap bmp = null;
            Bitmap cardSheet;
            string file = resourcePath + sheetFile;
            if (File.Exists(file))
            {
                cardSheet = (Bitmap)Image.FromFile(file);
            }
            else
            {
                Helpers.PrintError(functionId, "Unable to open / find card sheet(in [Resources]) to load");
                return bmp;
            }

            Point temp = new Point();
            temp.X = column * cardSize.Width;
            temp.Y = row * cardSize.Height;
            bmp = cardSheet.Clone(new Rectangle(temp, cardSize), cardSheet.PixelFormat);

            cardSheet.Dispose();
            if (bmp == null)
            {
                Helpers.PrintError(functionId, "Loading card image failed");
            }
            return bmp;
        }

        public static Bitmap SetCardImageFromCardSheet(int row, int column, Size size, string filePath)
        {
            //Size size = new Size() ;
            string functionId = $"{classId}.SetCardImageFromCardSheet(int, int)";
            Bitmap bmp = null;
            Bitmap cardSheet;
            //string filePath = resourcePath + sheetFile;
            if (File.Exists(filePath))
            {
                cardSheet = (Bitmap)Image.FromFile(filePath);
            }
            else
            {
                Helpers.PrintError(functionId, "Unable to open / find card sheet(in [Resources]) to load");
                return bmp;
            }

            Point temp = new Point();
            temp.X = column * size.Width;
            temp.Y = row * size.Height;
            bmp = cardSheet.Clone(new Rectangle(temp, size), cardSheet.PixelFormat);

            cardSheet.Dispose();
            if (bmp == null)
            {
                Helpers.PrintError(functionId, "Loading card image failed");
            }
            return bmp;
        }
        public static Bitmap SetCardImageFromCardSheet(int x, int y, int width, int height, string filePath)
        {
            string functionId = $"{classId}.SetCardImageFromCardSheet(int, int, int, int, string)";
            Bitmap bmp = null;
            Bitmap sheet;
            string file = filePath;
            if (!File.Exists(file))
            {
                Helpers.PrintError(functionId, "Unable to open / find card sheet(in [Resources]) to load");
                return bmp;
            }
            sheet = (Bitmap)Image.FromFile(file);
            if (x < 0 || y < 0)
            {
                Helpers.PrintError(functionId, "OutOfMemoryError 01");
                sheet.Dispose();
                return bmp;
            }
            if (x + width > sheet.Size.Width || y + height > sheet.Size.Width)
            {
                Helpers.PrintError(functionId, "OutOfMemoryError 02");
                sheet.Dispose();
                return bmp;
            }
            Point temp = new Point();
            temp.X = y * width;
            temp.Y = x * height;
            Size size = new Size(width, height);
            bmp = sheet.Clone(new Rectangle(temp, size), sheet.PixelFormat);

            sheet.Dispose();
            if (bmp == null)
            {
                Helpers.PrintError(functionId, "Loading card image failed");
            }

            return bmp;
        }

        static public void PrintCards(Card[] arr)
        {
            string functionId = $"{classId}.PrintCards(Card[])";
            Card c;
            for (int i = 0; i < arr.Length; i++)
            {
                c = arr[i];
                Helpers.PrintDebug(functionId, $"{c.name,7} of {c.suit,-10}");
            }
        }
        static public void PrintCardsVerbose(Card[] arr)
        {
            string functionId = $"{classId}.PrintCardsVerbose(Card[])";
            Card c;
            for (int i = 0; i < arr.Length; i++)
            {
                c = arr[i];
                Helpers.PrintDebug(functionId, $"{c.suit,-10} {c.suitValue,-10} {c.name,-10} {c.value,2}");
            }
        }


        #region old code commented
        #region non gui deck - commented out 06/23/2018
        // standard deck
        //static string[] suits = { "Clubs", "Diamonds", "Hearts", "Spades" };    
        //static string[] names = { "Ace", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Jack", "Queen", "King" };

        //static string resourcePath = Environment.CurrentDirectory + @"\..\..\Resources\";
        //static string sheetFile = @"decksheet.png";
        //static string file = resourcePath + sheetFile;

        //static public Card[] GetNewDeck()
        //{
        //    int idx = 0;
        //    Card[] d = new Card[52];
        //    for (int i = 0; i < suits.Length; i++)
        //    {
        //        Card[] s = BuildSuit(suits[i], (i + 1));
        //        for (int j = 0; j < s.Length; j++)
        //        {
        //            d[idx] = s[j];
        //            idx++;
        //        }
        //    }
        //    return d;
        //}
        //static Card[] BuildSuit(string suit, int suitValue)
        //{
        //    Card[] carr = new Card[names.Length];
        //    for (int i = 0; i < names.Length; i++)
        //    {
        //        Card c = new Card();
        //        c.Load(suit, suitValue, names[i], (i + 1));
        //        carr[i] = c;
        //    }
        //    return carr;
        //}
        #endregion non gui deck - commented out 06/23/2018
        #endregion old code commented
    } // end of class DeckBuilder

    public class DeckBuilderGUI
    {
        string classId = "DeckBuilderGUI";
        // standard deck
        string[] suits = { "Clubs", "Diamonds", "Hearts", "Spades" };
        //string[] names = { "Ace", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Jack", "Queen", "King" };
        string[] names = { "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Jack", "Queen", "King", "Ace" };

        string resourcePath = Environment.CurrentDirectory + @"\..\..\Resources\";
        string sheetFile = @"decksheet.png";

        Bitmap cardSheet;

        Size cardSize = new Size(81, 117);

        // temp vars for used in loops
        Point temp = new Point();
        int row = 0;
        int column = 0;
        Bitmap tempbmp;
        Bitmap imageBack;
        bool cardsheetloaded = false;
        public DeckBuilderGUI()
        {
            string functionId = $"{classId}.DeckBuilderGUI()";
            string file = resourcePath + sheetFile;
            if (File.Exists(file))
            {
                cardSheet = (Bitmap)Image.FromFile(file);
                cardsheetloaded = true;
            }
            else
                Helpers.PrintError(functionId, $"Unable to find image file ({file}) to load");
            imageBack = DeckHelpers.SetCardImageFromCardSheet(4, 0);
        }
        public void CleanUp()
        {
            string functionId = $"{classId}.CleanUp()";
            if (cardsheetloaded)
            {
                cardSheet.Dispose();
            }
            cardsheetloaded = false;
        }

        public Card[] GetNewDeck()
        {
            string functionId = $"{classId}.GetNewDeck()";
            int idx = 0;
            Card[] d = new Card[52];
            for (int i = 0; i < suits.Length; i++)
            {
                Card[] s = BuildSuit(suits[i], (i + 1));
                for (int j = 0; j < s.Length; j++)
                {
                    d[idx] = s[j];
                    idx++;
                }
            }
            return d;
        }
        Card[] BuildSuit(string suit, int suitValue)
        {
            string fnId = $"{classId}.BuildSuit(string, int)";
            if (!cardsheetloaded)
            {
                Helpers.PrintError(fnId, $"No image card sheet loaded for deck!, program crash likely unless card images loaded elsewhere");
                //return null;
            }
            string functionId = $"{classId}.BuildSuit(string, int)";
            if (string.Compare(suit, "Clubs") == 0) row = 2;
            if (string.Compare(suit, "Diamonds") == 0) row = 1;
            if (string.Compare(suit, "Hearts") == 0) row = 0;
            if (string.Compare(suit, "Spades") == 0) row = 3;
            Card[] carr = new Card[names.Length];
            for (int i = 0; i < names.Length; i++)
            {
                Card c = new Card();
                c.Load(suit, suitValue, names[i], (i + 2));
                if (cardsheetloaded)
                {
                    column = i;
                    temp.X = column * cardSize.Width;
                    temp.Y = row * cardSize.Height;
                    tempbmp = cardSheet.Clone(new Rectangle(temp, cardSize), cardSheet.PixelFormat);
                    c.imageFront = tempbmp;
                    c.imageBack = imageBack;

                }
                carr[i] = c;
            }
            return carr;
        }



        public void PrintCards(Card[] arr)
        {
            string functionId = $"{classId}.PrintCards(Card[])";
            Card c;
            for (int i = 0; i < arr.Length; i++)
            {
                c = arr[i];
                Console.WriteLine($"{c.suit,-10} {c.suitValue,-10} {c.name,-10} {c.value,2}");
            }
        }


    } // end of class DeckBuilderGUI
}
