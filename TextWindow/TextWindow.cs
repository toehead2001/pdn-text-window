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
        public string Author => ((AssemblyCopyrightAttribute)base.GetType().Assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)[0]).Copyright;
        public string Copyright => ((AssemblyDescriptionAttribute)base.GetType().Assembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)[0]).Description;
        public string DisplayName => ((AssemblyProductAttribute)base.GetType().Assembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false)[0]).Product;
        public Version Version => base.GetType().Assembly.GetName().Version;
        public Uri WebsiteUri => new Uri("http://www.getpaint.net/redirect/plugins.html");
    }

    [PluginSupportInfo(typeof(PluginSupportInfo), DisplayName = "Text Window")]
    public class TextWindowEffectPlugin : PropertyBasedEffect
    {
        private const string StaticName = "Text Window";
        private static readonly Image StaticIcon = new Bitmap(typeof(TextWindowEffectPlugin), "TextWindow.png");
        private const string SubmenuName = "Text Formations";

        public TextWindowEffectPlugin()
            : base(StaticName, StaticIcon, SubmenuName, EffectFlags.Configurable)
        {
        }

        public enum PropertyNames
        {
            Amount1,
            Amount2,
            Amount3,
            Amount4,
            Amount5,
            Amount6,
            Amount7,
            Amount8,
            Amount9,
            Amount10
        }


        protected override PropertyCollection OnCreatePropertyCollection()
        {
            List<Property> props = new List<Property>();

            props.Add(new StringProperty(PropertyNames.Amount1, "", 255));
            props.Add(new Int32Property(PropertyNames.Amount2, 100, 1, 1000));
            props.Add(new Int32Property(PropertyNames.Amount3, 12, 6, 250));
            FontFamily[] intstalledFontFamilies = new InstalledFontCollection().Families;
            List<FontFamily> usableFonts = new List<FontFamily>();
            foreach (FontFamily font in intstalledFontFamilies)
            {
                if (font.IsStyleAvailable(FontStyle.Regular))
                    usableFonts.Add(font);
            }
            FontFamily[] Amount4FontFamilies = usableFonts.ToArray();
            props.Add(new StaticListChoiceProperty(PropertyNames.Amount4, Amount4FontFamilies, 0, false));
            props.Add(new BooleanProperty(PropertyNames.Amount5, false));
            props.Add(new BooleanProperty(PropertyNames.Amount6, false));
            props.Add(new BooleanProperty(PropertyNames.Amount7, false));
            props.Add(new BooleanProperty(PropertyNames.Amount8, false));
            props.Add(new DoubleVectorProperty(PropertyNames.Amount9, Pair.Create(0.0, 0.0), Pair.Create(-1.0, -1.0), Pair.Create(+1.0, +1.0)));
            props.Add(new Int32Property(PropertyNames.Amount10, ColorBgra.ToOpaqueInt32(ColorBgra.FromBgra(EnvironmentParameters.PrimaryColor.B, EnvironmentParameters.PrimaryColor.G, EnvironmentParameters.PrimaryColor.R, 255)), 0, 0xffffff));

            return new PropertyCollection(props);
        }

        protected override ControlInfo OnCreateConfigUI(PropertyCollection props)
        {
            ControlInfo configUI = CreateDefaultConfigUI(props);

            configUI.SetPropertyControlValue(PropertyNames.Amount1, ControlInfoPropertyNames.DisplayName, "Text");
            configUI.SetPropertyControlValue(PropertyNames.Amount2, ControlInfoPropertyNames.DisplayName, "Text Repeat");
            configUI.SetPropertyControlValue(PropertyNames.Amount3, ControlInfoPropertyNames.DisplayName, "Font Size");
            configUI.SetPropertyControlValue(PropertyNames.Amount4, ControlInfoPropertyNames.DisplayName, "Font");
            PropertyControlInfo Amount4FontFamilyControl = configUI.FindControlForPropertyName(PropertyNames.Amount4);
            FontFamily[] Amount4FontFamilies = new InstalledFontCollection().Families;
            foreach (FontFamily ff in Amount4FontFamilies)
            {
                Amount4FontFamilyControl.SetValueDisplayName(ff, ff.Name);
            }
            configUI.SetPropertyControlValue(PropertyNames.Amount5, ControlInfoPropertyNames.DisplayName, string.Empty);
            configUI.SetPropertyControlValue(PropertyNames.Amount5, ControlInfoPropertyNames.Description, "Bold");
            configUI.SetPropertyControlValue(PropertyNames.Amount6, ControlInfoPropertyNames.DisplayName, string.Empty);
            configUI.SetPropertyControlValue(PropertyNames.Amount6, ControlInfoPropertyNames.Description, "Italic");
            configUI.SetPropertyControlValue(PropertyNames.Amount7, ControlInfoPropertyNames.DisplayName, string.Empty);
            configUI.SetPropertyControlValue(PropertyNames.Amount7, ControlInfoPropertyNames.Description, "Underline");
            configUI.SetPropertyControlValue(PropertyNames.Amount8, ControlInfoPropertyNames.DisplayName, string.Empty);
            configUI.SetPropertyControlValue(PropertyNames.Amount8, ControlInfoPropertyNames.Description, "Strikeout");
            configUI.SetPropertyControlValue(PropertyNames.Amount9, ControlInfoPropertyNames.DisplayName, "Offset");
            configUI.SetPropertyControlValue(PropertyNames.Amount9, ControlInfoPropertyNames.SliderSmallChangeX, 0.05);
            configUI.SetPropertyControlValue(PropertyNames.Amount9, ControlInfoPropertyNames.SliderLargeChangeX, 0.25);
            configUI.SetPropertyControlValue(PropertyNames.Amount9, ControlInfoPropertyNames.UpDownIncrementX, 0.01);
            configUI.SetPropertyControlValue(PropertyNames.Amount9, ControlInfoPropertyNames.SliderSmallChangeY, 0.05);
            configUI.SetPropertyControlValue(PropertyNames.Amount9, ControlInfoPropertyNames.SliderLargeChangeY, 0.25);
            configUI.SetPropertyControlValue(PropertyNames.Amount9, ControlInfoPropertyNames.UpDownIncrementY, 0.01);
            Rectangle selection9 = EnvironmentParameters.GetSelection(EnvironmentParameters.SourceSurface.Bounds).GetBoundsInt();
            ImageResource imageResource9 = ImageResource.FromImage(EnvironmentParameters.SourceSurface.CreateAliasedBitmap(selection9));
            configUI.SetPropertyControlValue(PropertyNames.Amount9, ControlInfoPropertyNames.StaticImageUnderlay, imageResource9);
            configUI.SetPropertyControlValue(PropertyNames.Amount10, ControlInfoPropertyNames.DisplayName, "Background Color");
            configUI.SetPropertyControlType(PropertyNames.Amount10, PropertyControlType.ColorWheel);

            return configUI;
        }

        protected override void OnSetRenderInfo(PropertyBasedEffectConfigToken newToken, RenderArgs dstArgs, RenderArgs srcArgs)
        {
            Amount1 = newToken.GetProperty<StringProperty>(PropertyNames.Amount1).Value;
            Amount2 = newToken.GetProperty<Int32Property>(PropertyNames.Amount2).Value;
            Amount3 = newToken.GetProperty<Int32Property>(PropertyNames.Amount3).Value;
            FontFamily Amount4FontFamily = (FontFamily)newToken.GetProperty<StaticListChoiceProperty>(PropertyNames.Amount4).Value;
            Amount4 = new FontFamily(Amount4FontFamily.Name);
            Amount5 = newToken.GetProperty<BooleanProperty>(PropertyNames.Amount5).Value;
            Amount6 = newToken.GetProperty<BooleanProperty>(PropertyNames.Amount6).Value;
            Amount7 = newToken.GetProperty<BooleanProperty>(PropertyNames.Amount7).Value;
            Amount8 = newToken.GetProperty<BooleanProperty>(PropertyNames.Amount8).Value;
            Amount9 = newToken.GetProperty<DoubleVectorProperty>(PropertyNames.Amount9).Value;
            Amount10 = ColorBgra.FromOpaqueInt32(newToken.GetProperty<Int32Property>(PropertyNames.Amount10).Value);


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

            using (RenderArgs ra = new RenderArgs(textSurface))
            {
                Graphics textGraphics = ra.Graphics;
                textGraphics.TextRenderingHint = TextRenderingHint.AntiAlias;

                using (SolidBrush fontBrush = new SolidBrush(Color.Black))
                using (Font font = new Font(Amount4, Amount3, fontStyles()))
                {
                    textGraphics.DrawString(textRepeated.ToString(), font, fontBrush, textRect);
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

        FontStyle fontStyles()
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

        Surface textSurface;

        void Render(Surface dst, Surface src, Rectangle rect)
        {
            ColorBgra CurrentPixel = Amount10;
            ColorBgra textPixel;

            for (int y = rect.Top; y < rect.Bottom; y++)
            {
                if (IsCancelRequested) return;
                for (int x = rect.Left; x < rect.Right; x++)
                {
                    textPixel = textSurface[x, y];
                    CurrentPixel.A = Int32Util.ClampToByte(255 - textPixel.A);

                    dst[x, y] = CurrentPixel;
                }
            }
        }

    }
}
