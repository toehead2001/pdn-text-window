using System.Globalization;

namespace TextWindowEffect
{
    internal static class L10nStrings
    {
        private static readonly string UICulture = CultureInfo.CurrentUICulture.Name;

        internal static string EffectName
        {
            get
            {
                switch (UICulture)
                {
                    case "ru":
                        return "Оконный текст";
                    default:
                        return "Text Window";
                }
            }
        }

        internal static string EffectMenu
        {
            get
            {
                switch (UICulture)
                {
                    default:
                        return "Text Formations";
                }
            }
        }

        internal static string EffectDescription
        {
            get
            {
                switch (UICulture)
                {
                    default:
                        return string.Empty;
                }
            }
        }

        internal static string EffectKeywords
        {
            get
            {
                switch (UICulture)
                {
                    case "ru":
                        return "текст|прозрачность";
                    default:
                        return "text|transparent";
                }
            }
        }

        internal static string Text
        {
            get
            {
                switch (UICulture)
                {
                    case "ru":
                        return "Текст";
                    default:
                        return "Text";
                }
            }
        }

        internal static string TextRepeat
        {
            get
            {
                switch (UICulture)
                {
                    case "ru":
                        return "Повтор текста";
                    default:
                        return "Text Repeat";
                }
            }
        }

        internal static string FontSize
        {
            get
            {
                switch (UICulture)
                {
                    case "ru":
                        return "Размер шрифта";
                    default:
                        return "Font Size";
                }
            }
        }

        internal static string Font
        {
            get
            {
                switch (UICulture)
                {
                    case "ru":
                        return "Шрифт";
                    default:
                        return "Font";
                }
            }
        }

        internal static string Bold
        {
            get
            {
                switch (UICulture)
                {
                    case "ru":
                        return "Жирный";
                    default:
                        return "Bold";
                }
            }
        }

        internal static string Italic
        {
            get
            {
                switch (UICulture)
                {
                    case "ru":
                        return "Курсив";
                    default:
                        return "Italic";
                }
            }
        }

        internal static string Underline
        {
            get
            {
                switch (UICulture)
                {
                    case "ru":
                        return "Подчеркнутый";
                    default:
                        return "Underline";
                }
            }
        }

        internal static string Strikeout
        {
            get
            {
                switch (UICulture)
                {
                    case "ru":
                        return "Зачеркнутый";
                    default:
                        return "Strikeout";
                }
            }
        }

        internal static string Offset
        {
            get
            {
                switch (UICulture)
                {
                    case "ru":
                        return "Смещение";
                    default:
                        return "Offset";
                }
            }
        }

        internal static string BackColor
        {
            get
            {
                switch (UICulture)
                {
                    case "ru":
                        return "Цвет фона";
                    default:
                        return "Background Color";
                }
            }
        }
    }
}
