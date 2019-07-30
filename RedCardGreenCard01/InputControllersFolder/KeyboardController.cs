using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RedCardGreenCard01.InputControllersFolder
{
    public class KeyboardController
    {
        protected string classId = "MouseController";
        protected string fnId = "";

        public bool escapeDown = false;

        public KeyboardController()
        {
        }
        public void KeyDownReader(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) escapeDown = true;
        }
        public void KeyUpReader(KeyEventArgs e)
        {

        }
        public void KeyPressReader(KeyPressEventArgs e)
        {

        }

    } // end of class KeyboardController
}
