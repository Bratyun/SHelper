using System;
using System.Drawing;
using System.Windows.Forms;

namespace TrackWebsite
{
    class ShadowTextBox : TextBox
    {
        public Color ShadowTextColor { get; set; }
        private string shadowText;
        public string ShadowText
        {
            get { return shadowText; }
            set
            {
                changeByClass = true;
                shadowText = value;
                if (IsShadowMode)
                {
                    Text = shadowText;
                }
                changeByClass = false;
            }
        }

        private Color realForeColor;
        private bool changeByClass = false;
        private bool isShadowMode;
        private bool IsShadowMode
        {
            get
            {
                return isShadowMode;
            }
            set
            {
                if (isShadowMode == value) return;
                isShadowMode = value;
                InitMode();
            }
        }

        public ShadowTextBox()
        {
            ShadowTextColor = Color.FromArgb(100, 100, 100);
            realForeColor = ForeColor;
            IsShadowMode = true;
        }

        private void InitMode()
        {
            changeByClass = true;
            if (IsShadowMode)
            {
                ForeColor = ShadowTextColor;
                Text = ShadowText;
            }
            else if (ForeColor == ShadowTextColor)
            {
                ForeColor = realForeColor;
                Text = "";
            }
            changeByClass = false;
        }

        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                if (changeByClass)
                {
                    base.ForeColor = value;
                }
                else
                {
                    realForeColor = value;
                    if (!IsShadowMode)
                    {
                        base.ForeColor = realForeColor;
                    }
                }
            }
        }

        protected override void OnEnter(EventArgs e)
        {
            IsShadowMode = false;
            base.OnEnter(e);
        }

        protected override void OnLeave(EventArgs e)
        {
            if (Text == "")
            {
                IsShadowMode = true;
            }
            base.OnLeave(e);
        }

        public override string Text
        {
            get
            {
                if (IsShadowMode)
                {
                    return "";
                }
                else
                {
                    return base.Text;
                }
            }
            set
            {
                if (IsShadowMode && value != "" && !changeByClass)
                {
                    IsShadowMode = false;
                }
                base.Text = value;
            }
        }
    }
}
