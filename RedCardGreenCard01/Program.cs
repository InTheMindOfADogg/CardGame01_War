using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
/// <summary>
#region 06/09/2018
/// 06/09/2018
/// Added classes in DeckAndCard folder
///     -Card
///     -Deck
///     -DeckBuilder
#endregion 06/09/2018
/// 06/12/2018
/// Added
///     -GameModesFolder
///         -BaseCardCame
///     -HandFolder
///         -Hand
/// 
/// 06/19/2018
/// 
/// 
/// 06/23/2018
/// Added DeckBuilderGUI for inserting card images - in DeckBuilder.cs
/// Added Bitmap image to Card.cs
/// Loaded cards with images
/// Cards are now successfuly holding their own image.
/// ** load time took impact on adding images, might need to address this later.
/// 
/// commenting out functions in static DeckBuilder
///     GetNewDeck()
///     BuildSuit()
/// static DeckBuilder now only contains a function to print the deck
///
/// 
/// 06/26/2018
/// started working on war.cs - not working
/// 
/// 
/// 06/30/2018
/// added classId(s) and functionId(s) to:
///     Deck.cs
///     BaseForm.cs (located in Program.cs)
///     Hand.cs
///     War.cs
///     Deckbuilder.cs
///    * I added these in attempt to help
///    *   better visualize call locations and
///    *   help with debugging and building.
///   
/// Hand.cs
///     cleaned up some print to console messages
/// War.cs
///     added grid drawing functions to for placement of objects
/// Added static Helpers
///     Added PrintError
///     Added PrintDebug
///     Added Debug boolean
///     Added PrintSizeCustom
/// BaseForm.cs
///     Added mouse click events(empty)
///     Starting to build out mouse controls for the game
///     Added function AddPlayCardButton()
///         - Current thought: This will be the starting point 
///                            for each round of War
/// 
/// 
/// 
/// 07/03/2018
/// Started adding game logic for war card game
/// 
/// 
/// 07/04/2018
/// 
/// 
/// 07/09/2018
/// War.cs
///     
/// 
/// 07/12/2018
/// Created backup - Copy (2)
/// War.cs
///     Commented out p1PlayCard and p2PlayCard, planning to remove later
///     commented out bool winnerSet
///     TODO: if p1 has 5 cards and p2 has more and the game goes into war logic
///           and the cards match at the end of war logic, p2 will go into war logic again
///           - need to figure out how to handle this, probably going to look up rules
///             and see how it works, or I might just leave it.
///
/// 07/17/2018
/// Moved PrintControlStats(Control, string) to Helpers
/// Created back up - Copy (3)
/// Started building BaseForm2
///     building in mouse controls
///     
/// Started building BaseGame01
///     building drawing
///     
/// 
/// 07/21/2018
/// Building BaseGame01 and BaseForm2
/// BaseForm02
///     added bool debugMode and this will set Helpers.Debug
///     
/// 
/// 07/24/2018
/// Added CardGame01 which will be the base class for card games
/// 
/// 
/// 08/03/2018
/// Added mouse moving logic to MouseControllers
/// [MouseController]
///     Playing with GraphicsPath
/// Moved mouse.SetMouseLastState(); from 
///     BaseGame01.Update() just after base call 
///     to 
///     BaseForm02.Update() just after mouse.SetCurrentState
/// [MouseController]
///     Renamed SetMouseLastState to SetLastState
///     Renamed SetMouseCurrentState to SetCurrentState
///     Created these functions to prevent having to set mouse controls in multiple places
///     - this allows for me to only have to make one call from the events in the main form
///     -  and I can set all the finer details in the mouse controller
///         MoveReader(MouseEventArgs)
///         ButtonDownReader(MouseEventArgs)
///         ButtonUpReader(MouseEventArgs)
///         ButtonWheelReader(MouseEventArgs)
///     Removed
///         lastLeftClick
///         lastRightClick
///         lastMiddleClick
/// 08/13/2018
/// [MouseController]
///     Added additional button release after being held for left and right
/// Created InputControllersFolder
/// Moved [MouseController] to InputControllersFolder
/// Created back up Copy(4)
/// 
/// Created [KeyboardController]
///     Set Escape to exit
/// Created BaseUIControl
/// Created Line
/// Created Square
/// Set rotation for objects around origin
/// 
///     TODO: 
///         Set up basic keyboard controls
///         Look into why key press to exit stops working randomly
///     NOTES:
///         [MouseController] I might need to look at mouse moving later.
///             
/// 09/03/2018
/// Added reset game to war.cs
/// 
/// 
/// </summary>


namespace RedCardGreenCard01
{
    using InputControllersFolder;
    using BaseUserInterfaceFolder;
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            #region CardGame01 test
            BaseGame01 g = new BaseGame01();
            g.Load();
            while (!g.ExitProgram())
            {
                Application.DoEvents();
                g.Update();
                g.RenderGame();
            }
            g.CleanUpOnExit();
            #endregion CardGame01 test
        }
    }
    public static class Helpers
    {
        static string resourcePath = Environment.CurrentDirectory + @"\..\..\Resources\";
        static string cardSheet = @"decksheet.png";

        public static bool Debug = true;
        public static void PrintError(string functionId, string msg)
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("ERROR: " + "[" + functionId + "]");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" " + msg);
            Console.ForegroundColor = originalColor;
        }
        public static void PrintDebug(string functionId, string msg)
        {
            if (Debug)
            {
                ConsoleColor originalColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("DEBUG: " + "[" + functionId + "]");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(" " + msg);
                Console.ForegroundColor = originalColor;
            }

        }
        public static string PrintSizeCustom(Size s)
        {
            return $"({s.Width.ToString()},{s.Height.ToString()})";
        }
        public static string ResourcePath()
        {
            return resourcePath;
        }
        public static string CardSheet()
        {
            return cardSheet;
        }
        public static void PrintControlStats(Control c, string classId)
        {
            string functionId = $"{classId}.PrintControlStats(Control)";
            const int typeColWidth = -10;
            const int colWidth = -15;
            Console.WriteLine($"{"Type",typeColWidth}{"Name",colWidth}{"Text",colWidth}{"Position",colWidth}{"Size",colWidth}");
            Console.WriteLine($"{c.GetType().Name,typeColWidth}{c.Name,colWidth}{c.Text,colWidth}{c.Location.ToString(),colWidth}{c.Size.ToString(),colWidth} ");
        }
    } // end of static class Helpers

    /// <summary>
    /// BaseForm02
    /// 
    /// 07/21/2018
    /// 
    /// I am planning for this class to cover the basic parts
    ///     that will be used in the game:
    ///     - creates basic form 
    ///         - sets title
    ///         - sets solid background
    ///         - sets width and height of the game
    ///         - handles basic drawing
    ///     
    ///     - controls basic exit function
    ///     - sets paths to resources that will be used
    ///     - controls debug mode
    ///     - main functions include
    ///         - Load()
    ///         - Update()
    ///         - RenderGame()
    ///     - Sets up basic user controls
    ///         - collects basic mouse input for later use
    ///         - TODO: set up to collect basic keyboard input
    ///     
    ///
    /// </summary>
    public abstract class BaseForm02
    {
        protected bool debugMode = true;

        protected string classId = "BaseForm02";
        protected string formTitle = "BaseForm02";
        protected string fnId = "";

        protected Form f = new Form();
        protected bool exitProgram = false;
        public bool ExitProgram() { return exitProgram; }

        string resourcePath = Helpers.ResourcePath();
        protected int gameWidth = 900;
        protected int gameHeight = 820;
        protected Size gameSize;
        protected MouseController mouse = new MouseController();
        protected KeyboardController kb = new KeyboardController();

        //protected List<BaseUIControl> ui = new List<BaseUIControl>();
        protected List<BaseDrawableObject> ui = new List<BaseDrawableObject>();
        protected List<CustomButton> buttons = new List<CustomButton>();

        protected Bitmap frame = null;
        protected Font font = null;

        virtual protected void LoadFunctionIds()
        {
            fnId = $"{classId}.LoadFunctionIds()";
        }
        virtual public void Load()
        {
            Helpers.Debug = debugMode;
            if (debugMode) fnId = $"{classId}.Load()";
            f.Text = formTitle;
            int extraWidth = 16;
            int extraHeight = 39;
            gameSize = new Size(gameWidth, gameHeight);
            f.Size = new Size((gameWidth + extraWidth), (gameHeight + extraHeight));
            f.StartPosition = FormStartPosition.CenterScreen;

            #region basic mouse controls
            f.MouseMove += F_MouseMove;
            f.MouseDown += F_MouseDown;
            f.MouseWheel += F_MouseWheel;
            f.MouseUp += F_MouseUp;
            #endregion basic mouse controls

            #region basic keyboard controls
            f.KeyDown += F_KeyDown;
            f.KeyUp += F_KeyUp;
            f.KeyPress += F_KeyPress;
            #endregion basic keyboard controls

            f.FormClosing += F_FormClosing;
            f.Show();
        }
        virtual public void Update()
        {
            if (debugMode) fnId = $"{classId}.Update()";
            mouse.SetCurrentState();
            mouse.SetLastState();
            if (kb.escapeDown) f.Close();
        }
        virtual protected Bitmap CreateFrame()
        {
            if (debugMode) fnId = $"{classId}.CreateFrame()";
            Bitmap frame;
            frame = new Bitmap(gameSize.Width, gameSize.Height);
            Graphics g = Graphics.FromImage(frame);
            g.FillRectangle(Brushes.CornflowerBlue, new RectangleF(0, 0, frame.Width, frame.Height));
            g.Dispose();
            return frame;
        }
        virtual protected Bitmap ReadyFrame()
        {
            if (debugMode) fnId = $"{classId}.ReadyFrame()";
            return CreateFrame();
        }
        virtual protected Font ReadyFontDefault()
        {
            if (debugMode) fnId = $"{classId}.ReadyFontDefault()";
            return new Font("Arial", 12, FontStyle.Regular);
        }
        virtual protected void CleanupAfterRender(ref Font font)
        {
            if (font != null)
            {
                font.Dispose();
                font = null;
            }
        }
        virtual protected void CleanupAfterRender(ref Bitmap frame)
        {
            if (frame != null)
            {
                frame.Dispose();
                frame = null;
            }
        }
        public void RenderGame()
        {
            if (debugMode) fnId = $"{classId}.RenderGame()";
            if (!exitProgram)
            {
                Graphics g = f.CreateGraphics();
                Bitmap frame = ReadyFrame();
                if (frame != null)
                {
                    g.DrawImage(frame, 0, 0);
                    CleanupAfterRender(ref frame);
                    CleanupAfterRender(ref font);
                }

                g.Dispose();
            }
        }
        virtual public void CleanUpOnExit()
        {
            if (debugMode) fnId = $"{classId}.CleanUp()";

            Helpers.PrintDebug(fnId, $"Disposing of bitmap");
            CleanupAfterRender(ref frame);
            Helpers.PrintDebug(fnId, $"Disposing of font");
            CleanupAfterRender(ref font);
            Helpers.PrintDebug(fnId, $"Disposing of form");
            f.Dispose();
        }
        private void F_FormClosing(object sender, FormClosingEventArgs e)
        {
            exitProgram = true;
        }
        #region Mouse Controls
        private void F_MouseUp(object sender, MouseEventArgs e)
        {
            mouse.ButtonUpReader(e);
        }
        private void F_MouseDown(object sender, MouseEventArgs e)
        {
            mouse.ButtonDownReader(e);
        }
        private void F_MouseWheel(object sender, MouseEventArgs e)
        {
            mouse.WheelScrollReader(e);
        }
        private void F_MouseMove(object sender, MouseEventArgs e)
        {
            mouse.PositionReader(e);
        }
        #endregion Mouse Controls
        #region Keyboard Controls
        private void F_KeyUp(object sender, KeyEventArgs e)
        {
            kb.KeyUpReader(e);
        }
        private void F_KeyDown(object sender, KeyEventArgs e)
        {
            kb.KeyDownReader(e);
        }
        private void F_KeyPress(object sender, KeyPressEventArgs e)
        {
            kb.KeyPressReader(e);
        }
        #endregion Keyboard Controls
    } // end of class BaseForm02

    public class BaseGame01 : BaseForm02
    {
        War war;
        //Button btnPlayCard;
        CustomButton resetGame;
        CustomButton playRound;

        public BaseGame01()
        {
            classId = "CardGame01";
            formTitle = "Card Game 01";
            string functionId = $"{classId}.CardGame01()";
            Helpers.PrintDebug(functionId, $"testing {functionId}");
            resetGame = new CustomButton();
            playRound = new CustomButton();
        }
        new public void Load()
        {
            base.Load();
            war = new War(f);
            war.Load();
            LoadCustomButtonControls();
            //AddPlayCardButton();
            f.Focus();
            f.Show();
            
        }
        protected void LoadCustomButtonControls()
        {
            //cbutton.Load(new Point(400, 300), new Size(150, 150));
            resetGame.Load(new Point(300, 300), "Reset Game");
            buttons.Add(resetGame);

            playRound.Load(new Point(300, 350), "Play Round");
            buttons.Add(playRound);
        }
        //void AddPlayCardButton()
        //{
        //    string functionId = $"{classId}.AddPlayCardButton()";
        //    Button b = new Button();
        //    Point pos = new Point();
        //    b.Name = "btnPlayCard";
        //    b.Text = "Play Card";
        //    b.Size = new Size(75, 23);
        //    pos.X = ((gameSize.Width / 10) * 3);
        //    pos.Y = ((gameSize.Height / 2) - b.Height / 2);
        //    b.Location = pos;
        //    b.Show();
        //    btnPlayCard = b;
        //    f.Controls.Add(btnPlayCard);
        //    Helpers.PrintDebug(functionId, $"{b.Name}.Size:{Helpers.PrintSizeCustom(b.Size)}");
        //    btnPlayCard.Click += BtnPlayCard_Click;
        //}

        //private void BtnPlayCard_Click(object sender, EventArgs e)
        //{
        //    string functionId = $"{classId}.BtnPlayCard_Click(object, EventArgs)";
        //    war.PlayCardClickController();
        //}

        new public void Update()
        {
            base.Update();
            UpdateCustomButtonControls();
            war.Update();

            MapMousePath();

        }
        protected void UpdateCustomButtonControls()
        {
            fnId = $"{classId}.UpdateCustomButtonControls()";
            bool mouseOver = false;
            if(mouseOver = playRound.CheckIfMouseOver(mouse.position))
            {
                if (mouseOver && mouse.leftClick)
                {
                    playRound.Clicked();
                    Helpers.PrintDebug(fnId, $"play round button clicked");
                    PlayRoundClicked();
                }
                playRound.Update();
            }
            if (mouseOver = resetGame.CheckIfMouseOver(mouse.position))
            {
                if (mouseOver && mouse.leftClick)
                {
                    resetGame.Clicked();
                    Helpers.PrintDebug(fnId, $"reset game button clicked");
                    ResetGameClicked();
                }
                resetGame.Update();
            }

            //for(int i = 0; i < buttons.Count; i++)
            //{
            //    mouseOver = buttons[i].CheckIfMouseOver(mouse.position);
            //    if(mouseOver && mouse.leftClick)
            //    {
            //        buttons[i].Clicked();
            //    }
            //    buttons[i].Update();
            //}
        }
        protected void PlayRoundClicked()
        {
            war.PlayCardClickController();
        }
        protected void ResetGameClicked()
        {
            war.ResetGame();
            
        }
        protected void MapMousePath()
        {
            if (mouse.leftHeld)
                mouse.StartDrawPath();
            else if (mouse.leftRelease)
                mouse.EndDrawPath();

            if (mouse.rightClick)
                mouse.ClearPath();
        }

        protected override Bitmap ReadyFrame()
        {
            Bitmap frame = CreateFrame();
            Graphics g = Graphics.FromImage(frame);
            war.RenderGame(g);
            DrawMouseParts(g);
            DrawCustomButtonsControls(g);
            DrawString(g, $"Rounds Played: {war.RoundsPlayed()}", new Point(200, 400));
            return frame;
        }
        protected void DrawCustomButtonsControls(Graphics g)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                if (buttons[i].visible)
                    buttons[i].Render(g);
            }
        }
        protected void DrawMouseParts(Graphics g)
        {
            if (font == null) font = ReadyFontDefault();
            //mouse.WriteStats(g, font);
            mouse.DrawPositionString(g, font);
            mouse.DrawPaths(g);
        }
        protected void DrawMousePosition(Graphics g)
        {
            if (font == null) font = ReadyFontDefault();
            mouse.DrawPositionString(g, font);
        }

        protected void DrawString(Graphics g, string text, Point position)
        {
            if (font == null) font = ReadyFontDefault();
            g.DrawString(text, font, Brushes.Black, position);
        }
    } // end of BaseGame01


} // end of namespace RedCardGreenCard01



#region not being used, going to remove for now 08/13/2018
// This class was being used as base for CardGame01, but it added no extra function
// so I am removing this for now.
//public class BaseGame01 : BaseForm02
//{
//protected bool test = false;
//public BaseGame01()
//{
//    classId = "BaseGame01";
//    formTitle = "Base Game Title Placeholder";
//    string functionId = $"{classId}.BaseGame01()";
//    Helpers.PrintDebug(functionId, "testing new class");
//}
//new public void Load()
//{
//    base.Load();
//}
//new public void Update()
//{
//    base.Update();
//}
//protected override Bitmap ReadyFrame()
//{
//    Bitmap frame = CreateFrame();
//    Graphics g = Graphics.FromImage(frame);
//    g.DrawRectangle(Pens.Red, new Rectangle(100, 100, 100, 100));
//    return frame;
//}
//} // end of class BaseGame01
#endregion not being used, going to remove for now 08/13/2018
#region commented out 08/13/2018 original working BaseForm
//public class BaseForm
//{
//    string classId = "BaseForm";
//    protected Form f = new Form();
//    bool exitProgram = false;
//    public bool ExitProgram() { return exitProgram; }
//    string resourcePath = Environment.CurrentDirectory + @"\..\..\Resources\";
//    War war;
//    Button btnPlayCard;
//    int gameWidth = 800;
//    int gameHeight = 690;
//    Size gameSize;
//    public BaseForm()
//    {
//        string functionId = $"{classId}.BaseForm()";
//        f.Text = "BaseForm";
//        int extraWidth = 16;
//        int extraHeight = 39;
//        gameSize = new Size(gameWidth, gameHeight);
//        f.Size = new Size((gameWidth + extraWidth), (gameHeight + extraHeight));
//        war = new War(f);
//        f.StartPosition = FormStartPosition.CenterScreen;
//        f.MouseMove += F_MouseMove;
//        f.MouseDown += F_MouseDown;
//        f.FormClosing += F_FormClosing;
//    }
//    private void F_FormClosing(object sender, FormClosingEventArgs e)
//    {
//        exitProgram = true;
//    }
//    public void Load()
//    {
//        string functionId = $"{classId}.Load()";
//        war.Load();
//        AddPlayCardButton();
//        f.Show();
//    }
//    void AddPlayCardButton()
//    {
//        string functionId = $"{classId}.AddPlayCardButton()";
//        Button b = new Button();
//        Point pos = new Point();
//        b.Name = "btnPlayCard";
//        b.Text = "Play Card";
//        b.Size = new Size(75, 23);
//        pos.X = ((gameSize.Width / 10) * 3);
//        pos.Y = ((gameSize.Height / 2) - b.Height / 2);
//        b.Location = pos;
//        b.Show();
//        btnPlayCard = b;
//        f.Controls.Add(btnPlayCard);
//        Helpers.PrintDebug(functionId, $"{b.Name}.Size:{Helpers.PrintSizeCustom(b.Size)}");
//        btnPlayCard.Click += BtnPlayCard_Click;
//    }
//    private void F_MouseUp(object sender, MouseEventArgs e)
//    {
//        //string functionId = $"{classId} F_MouseUp(object, MouseEventArgs)";
//    }
//    private void F_MouseDown(object sender, MouseEventArgs e)
//    {
//        //string functionId = $"{classId} F_MouseDown(object, MouseEventArgs)";
//    }
//    private void F_MouseMove(object sender, MouseEventArgs e)
//    {
//        //string functionId = $"{classId} F_MouseMove(object, MouseEventArgs)";
//    }
//    private void BtnNextCard_Click(object sender, EventArgs e)
//    {
//        string functionId = $"{classId}.BtnNextCard_Click(object, EventArgs)";
//    }
//    private void BtnPlayCard_Click(object sender, EventArgs e)
//    {
//        string functionId = $"{classId}.BtnPlayCard_Click(object, EventArgs)";
//        war.PlayCardClickController();
//    }
//    public void Update()
//    {
//        string functionId = $"{classId}.Update()";
//        war.Update();
//    }
//    public void RenderGame()
//    {
//        string functionId = $"{classId}.RenderGame()";
//        if (!exitProgram)
//        {
//            Graphics g = f.CreateGraphics();
//            war.RenderGame(g);
//            g.Dispose();
//        }
//    }
//    public void CleanUp()
//    {
//        string functionId = $"{classId}.CleanUp()";
//        f.Dispose();
//    }
//}
#endregion commented out 08/13/2018 original working BaseForm
#region BaseGame01 test
//BaseGame01 g = new BaseGame01();
//g.Load();
//while (!g.ExitProgram())
//{
//    Application.DoEvents();
//    g.Update();
//    g.RenderGame();
//}
//g.CleanUp();
#endregion BaseGame01 test
#region BaseForm test
//BaseForm f = new BaseForm();
//f.Load();
//while (!f.ExitProgram())
//{
//    Application.DoEvents();
//    f.Update();
//    f.RenderGame();
//}
//f.CleanUp();
#endregion BaseForm test
#region moved to InputControllersFolder 08/13/2018
//public class KeyboardController
//{
//    protected string classId = "MouseController";
//    protected string fnId = "";

//    public bool escapeDown = false;

//    public KeyboardController()
//    {
//    }
//    public void KeyDownReader(KeyEventArgs e)
//    {
//        if(e.KeyCode == Keys.Escape) escapeDown = true;
//    }
//    public void KeyUpReader(KeyEventArgs e)
//    {

//    }
//    public void KeyPressReader(KeyPressEventArgs e)
//    {

//    }

//} // end of class KeyboardController
#endregion moved to InputControllersFolder 08/13/2018
#region initial MouseController, moved to InputControllersFolder 08/13/2018
//public class MouseController
//{
//    protected string classId = "MouseController";
//    protected string fnId = "";

//    bool leftButtonDown = false;            //lbd = leftButtonDown
//    bool rightButtonDown = false;           //rbd = rightButtonDown
//    bool middleButtonDown = false;          //mbd = middleButtonDown
//    bool lastLeftButtonDown = false;        //llbd = lastLeftButtonDown
//    bool lastRightButtonDown = false;       //lrbd = lastRightButtonDown
//    bool lastMiddleButtonDown = false;      //lmbd = lastMiddleButtonDown

//    int wheelDelta = 0;
//    int totalWheelDelta = 0;
//    public bool scrollUp = false;
//    public bool scrollDown = false;

//    public bool leftClick = false;
//    public bool rightClick = false;
//    public bool middleClick = false;

//    public bool leftHeld = false;
//    public bool rightHeld = false;
//    public bool middleHeld = false;

//    public bool leftRelease = false;
//    public bool rightRelease = false;

//    public Point position = new Point();
//    public Point lastPosition = new Point();
//    public bool moving = false;


//    List<GraphicsPath> pathList = new List<GraphicsPath>();
//    GraphicsPath currentPath;

//    public MouseController()
//    {
//        currentPath = new GraphicsPath();
//    }
//    public void PositionReader(MouseEventArgs e)
//    {
//        position = e.Location;
//    }
//    public void ButtonDownReader(MouseEventArgs e)
//    {
//        if (e.Button == MouseButtons.Left)
//            leftButtonDown = true;
//        if (e.Button == MouseButtons.Right)
//            rightButtonDown = true;
//        if (e.Button == MouseButtons.Middle)
//            middleButtonDown = true;
//    }
//    public void ButtonUpReader(MouseEventArgs e)
//    {
//        if (e.Button == MouseButtons.Left)
//            leftButtonDown = false;
//        if (e.Button == MouseButtons.Right)
//            rightButtonDown = false;
//        if (e.Button == MouseButtons.Middle)
//            middleButtonDown = false;
//    }
//    public void WheelScrollReader(MouseEventArgs e)
//    {
//        wheelDelta = e.Delta;
//    }
//    public void SetCurrentState()
//    {
//        leftClick = (!lastLeftButtonDown && leftButtonDown) ? true : false;
//        rightClick = (!lastRightButtonDown && rightButtonDown) ? true : false;
//        middleClick = (!lastMiddleButtonDown && middleButtonDown) ? true : false;

//        leftHeld = (lastLeftButtonDown && leftButtonDown) ? true : false;
//        rightHeld = (lastRightButtonDown && rightButtonDown) ? true : false;
//        middleHeld = (lastMiddleButtonDown && middleButtonDown) ? true : false;

//        leftRelease = (!leftHeld && lastLeftButtonDown) ? true : false;
//        rightRelease = (!rightHeld && lastRightButtonDown) ? true : false;

//        moving = (lastPosition != position) ? true : false;
//        scrollUp = (wheelDelta > 0) ? true : false;
//        scrollDown = (wheelDelta < 0) ? true : false;
//        if (wheelDelta != 0)
//            totalWheelDelta += wheelDelta;
//        wheelDelta = 0;
//    }
//    public void SetLastState()
//    {
//        lastPosition = position;
//        lastLeftButtonDown = leftButtonDown;
//        lastRightButtonDown = rightButtonDown;
//        lastMiddleButtonDown = middleButtonDown;
//    }
//    public void WriteStats(Graphics g)
//    {
//        Font f = new Font("Arial", 12, FontStyle.Regular);
//        Point strPos = new Point();
//        int deltaY = 20;
//        g.DrawString($"mouse position: {position.ToString()}", f, Brushes.Black, strPos);
//        strPos.Y += deltaY;
//        g.DrawString($"last mouse position: {lastPosition.ToString()}", f, Brushes.Black, strPos);
//        strPos.Y += deltaY;
//        g.DrawString($"mouse moving: {moving.ToString()}", f, Brushes.Black, strPos);
//        strPos.Y += deltaY;
//        g.DrawString($"left button down: {leftButtonDown.ToString()}", f, Brushes.Black, strPos);
//        strPos.Y += deltaY;
//        g.DrawString($"right button down: {rightButtonDown.ToString()}", f, Brushes.Black, strPos);
//        strPos.Y += deltaY;
//        g.DrawString($"middle button down: {middleButtonDown.ToString()}", f, Brushes.Black, strPos);
//        strPos.Y += deltaY;
//        g.DrawString($"last left button down: {lastLeftButtonDown.ToString()}", f, Brushes.Black, strPos);
//        strPos.Y += deltaY;
//        g.DrawString($"last right button down: {lastRightButtonDown.ToString()}", f, Brushes.Black, strPos);
//        strPos.Y += deltaY;
//        g.DrawString($"last middle button down: {lastMiddleButtonDown.ToString()}", f, Brushes.Black, strPos);
//        strPos.Y += deltaY;
//        g.DrawString($"left button released: {leftRelease.ToString()}", f, Brushes.Black, strPos);
//        strPos.Y += deltaY;
//        g.DrawString($"scroll up: {scrollUp.ToString()}", f, Brushes.Black, strPos);
//        strPos.Y += deltaY;
//        g.DrawString($"last scroll up: {scrollDown.ToString()}", f, Brushes.Black, strPos);
//        strPos.Y += deltaY;
//        g.DrawString($"Total wheel scroll: {totalWheelDelta.ToString()}", f, Brushes.Black, strPos);
//        strPos.Y += deltaY;
//    }

//    public void StartDrawPath()
//    {
//        currentPath.AddLine(position.X, position.Y, position.X, position.Y);
//        //g.DrawPath(Pens.Red, currentPath);
//    }
//    public void EndDrawPath()
//    {
//        if (Helpers.Debug) fnId = $"{classId}.EndDrawPath()";
//        pathList.Add(currentPath);
//        Helpers.PrintDebug(fnId, $"pathList.Count: {pathList.Count}");
//        currentPath = new GraphicsPath();
//    }
//    public void ClearPath()
//    {
//        if (Helpers.Debug) fnId = $"{classId}.ClearPath()";
//        currentPath.ClearMarkers();
//        pathList.Clear();
//        Helpers.PrintDebug(fnId, $"pathList.Count: {pathList.Count}");
//    }
//    public void DrawPaths(Graphics g)
//    {
//        g.DrawPath(Pens.Red, currentPath);
//        for (int i = 0; i < pathList.Count; i++)
//        {
//            g.DrawPath(Pens.Red, pathList[i]);
//        }
//    }

//} // end of class MouseController
#endregion initial MouseController, moved to InputControllersFolder 08/13/2018
#region old add button code
//Button AddButton(string text, Control parent, Size size, Point location, string name = "")
//{
//    Button b = new Button();
//    b.Name = name;
//    b.Text = text;
//    b.Size = size;
//    b.Location = location;
//    b.Show();
//    parent.Controls.Add(b);
//    Console.WriteLine(b.Size);
//    return b;
//}
#endregion old add button code
#region first attempt at BaseGame - replaced with BaseCardGame
//public class BaseGame
//{
//    Deck deck;// = new Deck();
//    Hand h;// = new Hand();
//    public BaseGame()
//    {
//        deck = new Deck();
//        h = new Hand();

//        h.AddCardToHand(deck.Deal(5));
//        deck.PrintCards(h.cards);
//        int numberOfCards = 5;
//        Console.WriteLine($"Main] Adding {numberOfCards} to hand\n");
//        h.AddCardToHand(deck.Deal(numberOfCards));
//    }
//}
#endregion first attempt at BaseGame - replaced with BaseCardGame