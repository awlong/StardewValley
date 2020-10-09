using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TAS.Inputs
{
    public class TextBoxInput
    {
        public static bool SelectAndWrite<T>(T obj, string name, string text)
        {
            if (!SetSelected(obj, name, true))
                return false;
            Write(obj, name, text);
            if (!SetSelected(obj, name, false))
                return false;
            return true;
        }

        public static bool SetSelected<T>(T obj, string name, bool selected=true)
        {
            try
            {
                TextBox textBox = Reflector.GetValue<T, TextBox>(obj, name);
                textBox.Selected = selected;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static TextBox GetSelected()
        {
            return GetSelected(out string _);
        }

        public static TextBox GetSelected(out string name)
        {
            TextBox textBox = null;
            name = "";
            if (Game1.activeClickableMenu != null)
            {
                if (Game1.activeClickableMenu is TitleMenu)
                {
                    if (TitleMenu.subMenu is CharacterCustomization characterCustomization)
                    {
                        textBox = Reflector.GetValue<CharacterCustomization, TextBox>(characterCustomization, "nameBox");
                        name = "nameBox";
                        if(!textBox.Selected)
                        {
                            textBox = Reflector.GetValue<CharacterCustomization, TextBox>(characterCustomization, "farmnameBox");
                            name = "farmnameBox";
                            if(!textBox.Selected)
                            {
                                textBox = Reflector.GetValue<CharacterCustomization, TextBox>(characterCustomization, "favThingBox");
                                name = "favThingBox";
                            }
                        }
                    }
                }
            }
            // TODO: Other textbox based events
            if (textBox != null)
                return textBox.Selected ? textBox : null;
            return null;
        }

        public static void Write(string text)
        {
            Write(GetSelected(), text);
        }

        public static void Write(TextBox textBox, string text)
        {
            if (textBox != null)
            {
                foreach (char c in text)
                {
                    textBox.RecieveTextInput(c);
                }
            }
        }

        public static void Write<T>(T obj, string name, string text)
        {
            TextBox textBox = Reflector.GetValue<T, TextBox>(obj, name);
            Write(textBox, text);
        }

        public static string GetText<T>(T obj, string name)
        {
            TextBox textBox = Reflector.GetValue<T, TextBox>(obj, name);
            return textBox.Text;
        }
    }
}
