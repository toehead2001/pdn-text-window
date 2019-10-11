using System;
using System.Drawing;
using System.Reflection;
using System.Drawing.Text;
using System.Collections.Generic;
using PaintDotNet;
using PaintDotNet.Effects;
using PaintDotNet.IndirectUI;
using PaintDotNet.PropertySystem;

namespace TextWindowEffect
{
    public class PluginSupportInfo : IPluginSupportInfo
    {
        public string Author => base.GetType().Assembly.GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright;
        public string Copyright => L10nStrings.EffectDescription;
        public string DisplayName => L10nStrings.EffectName;
        public Version Version => base.GetType().Assembly.GetName().Version;
        public Uri WebsiteUri => new Uri("https://forums.getpaint.net/index.php?showtopic=32208");

        public string plugin_browser_Keywords => L10nStrings.EffectKeywords;
        public string plugin_browser_Description => L10nStrings.EffectDescription;
    }

    [PluginSupportInfo(typeof(PluginSupportInfo))]
    public class TextWindowEffectPlugin : PropertyBasedEffect
    {
        private static readonly Image StaticIcon = new Bitmap(typeof(TextWindowEffectPlugin), "TextWindow.png");

        public TextWindowEffectPlugin()
            : base(L10nStrings.EffectName, StaticIcon, L10nStrings.EffectMenu, new EffectOptions { Flags = EffectFlags.Configurable })
        {
        }

        private enum PropertyNames
        {
            Text,
            TextRepeat,
            FontSize,
            Font,
            Bold,
            Italic,
            Underline,
            Strikeout,
            Offset,
            BackColor
        }

        protected override PropertyCollection OnCreatePropertyCollection()
        {
            List<Property> props = new List<Property>
            {
                new StringProperty(PropertyNames.Text, "", 255),
                new Int32Property(PropertyNames.TextRepeat, 100, 1, 1000),
                new Int32Property(PropertyNames.FontSize, 12, 6, 250),
                new StaticListChoiceProperty(PropertyNames.Font, FontUtil.UsableFontFamilies, FontUtil.FindFontIndex("Arial"), false),
                new BooleanProperty(PropertyNames.Bold, false),
                new BooleanProperty(PropertyNames.Italic, false),
                new BooleanProperty(PropertyNames.Underline, false),
                new BooleanProperty(PropertyNames.Strikeout, false),
                new DoubleVectorProperty(PropertyNames.Offset, Pair.Create(0.0, 0.0), Pair.Create(-1.0, -1.0), Pair.Create(+1.0, +1.0)),
                new Int32Property(PropertyNames.BackColor, ColorBgra.ToOpaqueInt32(ColorBgra.FromBgra(EnvironmentParameters.PrimaryColor.B, EnvironmentParameters.PrimaryColor.G, EnvironmentParameters.PrimaryColor.R, 255)), 0, 0xffffff)
            };

            return new PropertyCollection(props);
        }

        protected override ControlInfo OnCreateConfigUI(PropertyCollection props)
        {
            ControlInfo configUI = CreateDefaultConfigUI(props);

            configUI.SetPropertyControlValue(PropertyNames.Text, ControlInfoPropertyNames.DisplayName, L10nStrings.Text);
            configUI.SetPropertyControlValue(PropertyNames.TextRepeat, ControlInfoPropertyNames.DisplayName, L10nStrings.TextRepeat);
            configUI.SetPropertyControlValue(PropertyNames.FontSize, ControlInfoPropertyNames.DisplayName, L10nStrings.FontSize);
            configUI.SetPropertyControlValue(PropertyNames.Font, ControlInfoPropertyNames.DisplayName, L10nStrings.Font);
            PropertyControlInfo Amount4FontFamilyControl = configUI.FindControlForPropertyName(PropertyNames.Font);
            foreach (FontFamily ff in FontUtil.UsableFontFamilies)
            {
                Amount4FontFamilyControl.SetValueDisplayName(ff, ff.Name);
            }
            configUI.SetPropertyControlValue(PropertyNames.Bold, ControlInfoPropertyNames.DisplayName, string.Empty);
            configUI.SetPropertyControlValue(PropertyNames.Bold, ControlInfoPropertyNames.Description, L10nStrings.Bold);
            configUI.SetPropertyControlValue(PropertyNames.Italic, ControlInfoPropertyNames.DisplayName, string.Empty);
            configUI.SetPropertyControlValue(PropertyNames.Italic, ControlInfoPropertyNames.Description, L10nStrings.Italic);
            configUI.SetPropertyControlValue(PropertyNames.Underline, ControlInfoPropertyNames.DisplayName, string.Empty);
            configUI.SetPropertyControlValue(PropertyNames.Underline, ControlInfoPropertyNames.Description, L10nStrings.Underline);
            configUI.SetPropertyControlValue(PropertyNames.Strikeout, ControlInfoPropertyNames.DisplayName, string.Empty);
            configUI.SetPropertyControlValue(PropertyNames.Strikeout, ControlInfoPropertyNames.Description, L10nStrings.Strikeout);
            configUI.SetPropertyControlValue(PropertyNames.Offset, ControlInfoPropertyNames.DisplayName, L10nStrings.Offset);
            configUI.SetPropertyControlValue(PropertyNames.Offset, ControlInfoPropertyNames.SliderSmallChangeX, 0.05);
            configUI.SetPropertyControlValue(PropertyNames.Offset, ControlInfoPropertyNames.SliderLargeChangeX, 0.25);
            configUI.SetPropertyControlValue(PropertyNames.Offset, ControlInfoPropertyNames.UpDownIncrementX, 0.01);
            configUI.SetPropertyControlValue(PropertyNames.Offset, ControlInfoPropertyNames.SliderSmallChangeY, 0.05);
            configUI.SetPropertyControlValue(PropertyNames.Offset, ControlInfoPropertyNames.SliderLargeChangeY, 0.25);
            configUI.SetPropertyControlValue(PropertyNames.Offset, ControlInfoPropertyNames.UpDownIncrementY, 0.01);
            Rectangle selection9 = EnvironmentParameters.GetSelection(EnvironmentParameters.SourceSurface.Bounds).GetBoundsInt();
            ImageResource imageResource9 = ImageResource.FromImage(EnvironmentParameters.SourceSurface.CreateAliasedBitmap(selection9));
            configUI.SetPropertyControlValue(PropertyNames.Offset, ControlInfoPropertyNames.StaticImageUnderlay, imageResource9);
            configUI.SetPropertyControlValue(PropertyNames.BackColor, ControlInfoPropertyNames.DisplayName, L10nStrings.BackColor);
            configUI.SetPropertyControlType(PropertyNames.BackColor, PropertyControlType.ColorWheel);

            return configUI;
        }

        protected override void OnSetRenderInfo(PropertyBasedEffectConfigToken newToken, RenderArgs dstArgs, RenderArgs srcArgs)
        {
            Amount1 = newToken.GetProperty<StringProperty>(PropertyNames.Text).Value;
            Amount2 = newToken.GetProperty<Int32Property>(PropertyNames.TextRepeat).Value;
            Amount3 = newToken.GetProperty<Int32Property>(PropertyNames.FontSize).Value;
            Amount4 = (FontFamily)newToken.GetProperty<StaticListChoiceProperty>(PropertyNames.Font).Value;
            Amount5 = newToken.GetProperty<BooleanProperty>(PropertyNames.Bold).Value;
            Amount6 = newToken.GetProperty<BooleanProperty>(PropertyNames.Italic).Value;
            Amount7 = newToken.GetProperty<BooleanProperty>(PropertyNames.Underline).Value;
            Amount8 = newToken.GetProperty<BooleanProperty>(PropertyNames.Strikeout).Value;
            Amount9 = newToken.GetProperty<DoubleVectorProperty>(PropertyNames.Offset).Value;
            Amount10 = ColorBgra.FromOpaqueInt32(newToken.GetProperty<Int32Property>(PropertyNames.BackColor).Value);

            Rectangle selection = EnvironmentParameters.GetSelection(srcArgs.Surface.Bounds).GetBoundsInt();
            RectangleF textRect = new RectangleF((float)Amount9.First * selection.Width + selection.Left, (float)Amount9.Second * selection.Height + selection.Top, selection.Width, selection.Height);

            string text = Amount1 + " ";
            System.Text.StringBuilder textRepeated = new System.Text.StringBuilder();
            for (int i = 0; i < Amount2; i++)
            {
                textRepeated.Append(text);
            }

            if (textSurface == null)
                textSurface = new Surface(srcArgs.Surface.Size);
            else
                textSurface.Clear(Color.Transparent);

            using (Graphics g = new RenderArgs(textSurface).Graphics)
            {
                g.TextRenderingHint = TextRenderingHint.AntiAlias;

                FontStyle fontStyle = FontStyle.Regular;
                if (Amount5) fontStyle |= FontStyle.Bold;
                if (Amount6) fontStyle |= FontStyle.Italic;
                if (Amount7) fontStyle |= FontStyle.Underline;
                if (Amount8) fontStyle |= FontStyle.Strikeout;

                using (SolidBrush fontBrush = new SolidBrush(Color.Black))
                using (Font font = new Font(Amount4, Amount3, fontStyle))
                {
                    g.DrawString(textRepeated.ToString(), font, fontBrush, textRect);
                }
            }

            base.OnSetRenderInfo(newToken, dstArgs, srcArgs);
        }

        protected override void OnRender(Rectangle[] renderRects, int startIndex, int length)
        {
            if (length == 0) return;
            for (int i = startIndex; i < startIndex + length; ++i)
            {
                Render(DstArgs.Surface, SrcArgs.Surface, renderRects[i]);
            }
        }

        private string Amount1 = ""; // [0,255] Text
        private int Amount2 = 100; // [1,1000] Text Repeat
        private int Amount3 = 12; // [6,250] Font Size
        private FontFamily Amount4 = new FontFamily("Arial"); // Font
        private bool Amount5 = false; // [0,1] Bold
        private bool Amount6 = false; // [0,1] Italic
        private bool Amount7 = false; // [0,1] Underline
        private bool Amount8 = false; // [0,1] Strikeout
        private Pair<double, double> Amount9 = Pair.Create(0.0, 0.0); // Offset
        private ColorBgra Amount10 = ColorBgra.FromBgr(0, 0, 0); // Background Color

        private Surface textSurface;

        private void Render(Surface dst, Surface src, Rectangle rect)
        {
            ColorBgra CurrentPixel = Amount10;

            for (int y = rect.Top; y < rect.Bottom; y++)
            {
                if (IsCancelRequested) return;
                for (int x = rect.Left; x < rect.Right; x++)
                {
                    CurrentPixel.A = Int32Util.ClampToByte(byte.MaxValue - textSurface[x, y].A);
                    dst[x, y] = CurrentPixel;
                }
            }
        }
    }

    internal static class FontUtil
    {
        internal static readonly FontFamily[] UsableFontFamilies;

        internal static int FindFontIndex(string familyName)
        {
            for (int i = 0; i < UsableFontFamilies.Length; i++)
            {
                if (UsableFontFamilies[i].Name.Equals(familyName, StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }

            return 0;
        }

        static FontUtil()
        {
            List<FontFamily> usableFonts = new List<FontFamily>();
            using (InstalledFontCollection intstalledFonts = new InstalledFontCollection())
            {
                foreach (FontFamily font in intstalledFonts.Families)
                {
                    if (font.IsStyleAvailable(FontStyle.Regular & FontStyle.Bold & FontStyle.Italic & FontStyle.Underline & FontStyle.Strikeout))
                        usableFonts.Add(font);
                }
            }
            UsableFontFamilies = usableFonts.ToArray();
        }
    }
}
