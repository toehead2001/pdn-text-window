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
bool Amount5 = false; // [0,1] Bold
bool Amount6 = false; // [0,1] Italic
bool Amount7 = false; // [0,1] Underline
bool Amount8 = false; // [0,1] Strikeout
Pair<double, double> Amount9 = Pair.Create(0.0, 0.0); // Offset
ColorBgra Amount10 = ColorBgra.FromBgr(0, 0, 0); // Background Color
#endregion

void InvalidFontMessage(string msg, string caption)
{
    PaintDotNet.Threading.PdnSynchronizationContext.Instance.Send(
        new System.Threading.SendOrPostCallback(delegate (object state)
        {
            System.Windows.Forms.MessageBox.Show(msg, caption);
        }), null);
}

private FontStyle fontStyles()
{
    List<FontStyle> styleList = new List<FontStyle>();
    if (Amount5)
        styleList.Add(FontStyle.Bold);
    if (Amount6)
        styleList.Add(FontStyle.Italic);
    if (Amount7)
        styleList.Add(FontStyle.Underline);
    if (Amount8)
        styleList.Add(FontStyle.Strikeout);

    FontStyle styles;
    switch (styleList.Count)
    {
        case 0:
            styles = FontStyle.Regular;
            break;
        case 1:
            styles = styleList[0];
            break;
        case 2:
            styles = styleList[0] | styleList[1];
            break;
        case 3:
            styles = styleList[0] | styleList[1] | styleList[2];
            break;
        case 4:
            styles = styleList[0] | styleList[1] | styleList[2] | styleList[3];
            break;
        default:
            styles = FontStyle.Regular;
            break;
    }
    return styles;
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

    RectangleF textRect = new RectangleF((float)Amount9.First * selection.Width, (float)Amount9.Second * selection.Height, selection.Width, selection.Height);
    Font font = new Font(Amount4, Amount3, fontStyles());
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
            CurrentPixel.R = Amount10.R;
            CurrentPixel.G = Amount10.G;
            CurrentPixel.B = Amount10.B;

            dst[x, y] = CurrentPixel;
        }
    }
}