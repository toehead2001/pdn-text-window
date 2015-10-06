// Name: Text Window
// Submenu: Text Formations
// Author: toe_head2001
// Title: 
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

void InvalidFontMessage(string msg, string caption)
{
    PaintDotNet.Threading.PdnSynchronizationContext.Instance.Send(
        new System.Threading.SendOrPostCallback(delegate (object state)
        {
            System.Windows.Forms.MessageBox.Show(msg, caption);
        }), null);
}

void Render(Surface dst, Surface src, Rectangle rect)
{
    if (!Amount4.IsStyleAvailable(FontStyle.Regular))
    {
        InvalidFontMessage("You can not use the font '" + this.Amount4.Name + "'.\n\nPlease choose a different font.", "Font Error");
        Amount4 = new FontFamily("Arial");
    }

    Rectangle selection = EnvironmentParameters.GetSelection(src.Bounds).GetBoundsInt();

    Bitmap textBitmap = new Bitmap(selection.Width, selection.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
    Graphics g = Graphics.FromImage(textBitmap);

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

    Surface textSurface = Surface.CopyFromBitmap(textBitmap);

    ColorBgra CurrentPixel = new ColorBgra();
    ColorBgra textPixel;

    for (int y = rect.Top; y < rect.Bottom; y++)
    {
        if (IsCancelRequested) return;
        for (int x = rect.Left; x < rect.Right; x++)
        {
            textPixel = textSurface.GetBilinearSample((x - selection.Left), (y - selection.Top));
            
            CurrentPixel.A = Int32Util.ClampToByte((int)(255 - textPixel.A));
            CurrentPixel.R = Amount6.R;
            CurrentPixel.G = Amount6.G;
            CurrentPixel.B = Amount6.B;

            dst[x, y] = CurrentPixel;
        }
    }
}