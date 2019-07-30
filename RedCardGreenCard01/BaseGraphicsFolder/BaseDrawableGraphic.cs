using System.Drawing;


namespace RedCardGreenCard01.BaseGraphicsFolder
{
    abstract class BaseDrawableGraphic
    {
        protected bool debugMode = false;
        protected string classId = "BaseDrawableGraphic";
        protected string fnId = "";
        protected Font font;
        public Point startingPosition;
        public Size startingSize;
        public Point position;
        public Size size;
        public int renderOrder = 0;
        public bool visible = false;
        public bool updatable = false;
        
        public BaseDrawableGraphic()
        {
            fnId = $"{classId}.{classId}()";
            debugMode = Helpers.Debug;
            position = new Point();
            size = new Size(10, 10);
            visible = true;
            updatable = true;
        }
        public virtual void Load(Point startingPosition, Size startingSize)
        {
            this.startingPosition = startingPosition;
            this.startingSize = startingSize;
        }
        public abstract void Update();
        public abstract void Render(Graphics g);
        
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
        protected void DrawString(Graphics g, string text, Point position)
        {
            if (font == null) font = ReadyFontDefault();
            g.DrawString(text, font, Brushes.Black, position);
        }
        protected void DrawStringInBox(Graphics g, Font f, string text, ref Point position, ref SizeF boxSize, int rightpadding = 2)
        {
            SizeF temp = g.MeasureString(text, f);
            boxSize.Height += temp.Height;
            if (temp.Width > boxSize.Width) boxSize.Width = temp.Width + rightpadding;
            DrawString(g, text, position);
            position.Y += (int)(temp.Height + 0.49f);
        }
    }
}
