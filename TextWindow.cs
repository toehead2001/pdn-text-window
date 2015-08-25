// Name: Text Window
// Submenu: Text Formations
// Author: toe_head2001
// Title: Text Window
// Desc: 
// Keywords: Text|Transparent
// URL: http://www.getpaint.net/redirect/plugins.html
// Help:
#region UICode
string Amount1 = ""; // [0,255] Text
int Amount2 = 100; // [1,1000] Text Repeat
int Amount3 = 12; // [6,250] Font Size
FontFamily Amount4 = new FontFamily("Arial"); // Font
Pair<double, double> Amount5 = Pair.Create(0.0, 0.0); // Offset
ColorBgra Amount6 = ColorBgra.FromBgr(0, 0, 0); // Background Color
#endregion

void Render(Surface dst, Surface src, Rectangle rect)
{
    if (!Amount4.IsStyleAvailable(FontStyle.Regular))
    {
        MessageBox.Show("You can not use the font '" + this.Amount4.Name + "'.\n\nPlease choose a different font.", "Font Error");
        Amount4 = new FontFamily("Arial");
    }

    Rectangle selection = EnvironmentParameters.GetSelection(src.Bounds).GetBoundsInt();

    Bitmap windowBitmap = new Bitmap(selection.Width, selection.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
    Graphics g = Graphics.FromImage(windowBitmap);

    Rectangle backgroundRect = new Rectangle((int)Amount5.First, 0, selection.Width, selection.Height);
    g.FillRectangle(new SolidBrush(Color.White), backgroundRect);

    RectangleF textRect = new RectangleF((float)Amount5.First * selection.Width, (float)Amount5.Second * selection.Height, selection.Width, selection.Height);
    Font font = new Font(Amount4, Amount3);
    g.TextRenderingHint = TextRenderingHint.AntiAlias;

    string text = Amount1 + " ";
    System.Text.StringBuilder textRepeated = new System.Text.StringBuilder();
    for (int i = 0; i < Amount2; i++)
    {
        textRepeated.Append(text);
    }

    g.DrawString(textRepeated.ToString(), font, new SolidBrush(Color.Black), textRect);

    Surface windowSurface = Surface.CopyFromBitmap(windowBitmap);

    ColorBgra CurrentPixel;
    double alpha;

    for (int y = rect.Top; y < rect.Bottom; y++)
    {
        if (IsCancelRequested) return;
        for (int x = rect.Left; x < rect.Right; x++)
        {
            CurrentPixel = windowSurface.GetBilinearSample((x - selection.Left), (y - selection.Top));

            alpha = 0.59 * Math.Sqrt(CurrentPixel.R * CurrentPixel.R + CurrentPixel.G * CurrentPixel.G + CurrentPixel.B * CurrentPixel.B);

            if (alpha <= 0)
                CurrentPixel.A = 0;

            else if (alpha < CurrentPixel.A)
            {
                CurrentPixel.A = (byte)alpha;

                alpha = alpha / 255;

                CurrentPixel.R = Int32Util.ClampToByte((int)(CurrentPixel.R / alpha));
                CurrentPixel.G = Int32Util.ClampToByte((int)(CurrentPixel.G / alpha));
                CurrentPixel.B = Int32Util.ClampToByte((int)(CurrentPixel.B / alpha));
            }

            CurrentPixel.R = Amount6.R;
            CurrentPixel.G = Amount6.G;
            CurrentPixel.B = Amount6.B;

            dst[x, y] = CurrentPixel;
        }
    }
}