﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMenu {
    public class Menu {
        /*
         * Types to help make values human-readable.
         */
        public enum MType {
            Horizontal = 0,
            Vertical = 1
        }
        public enum MAlignment {
            Left = 0,
            Center = 1,
            Right = 2
        }

        /*
         * Properties
         */
        public List<string> Items { get; set; } = new List<string>();
        public MType Type { get; set; } = MType.Horizontal;
        public MAlignment Alignment { get; set; } = MAlignment.Left;
        public MAlignment MenuAlignment { get; set; } = MAlignment.Left;
        public int ItemWidth { get; set; } = 15;
        public int XPadding { get; set; } = 2;
        public int YPadding { get; set; } = 0;
        public bool CycleOptions { get; set; } = false;
        public bool CharacterSelection { get; set; } = true;
        public ConsoleColor Color1 { get; set; } = ConsoleColor.Gray;
        public ConsoleColor Color2 { get; set; } = ConsoleColor.Black;
        /*
         * Draw the menu. 
         */
        public int DisplayMenu() {
            int selectedIndex = 0;
            bool selected = false;

            // Align the menu as per the property.
            int y = Console.CursorTop;

            // Define the keys that increments and decrements
            // the selectedIndex value.
            var plus = Type == MType.Horizontal ?
                ConsoleKey.RightArrow : ConsoleKey.DownArrow;
            var minus = Type == MType.Horizontal ?
                ConsoleKey.LeftArrow : ConsoleKey.UpArrow;

            while (!selected) {
                DrawList(true, selectedIndex, y);

                var keyInfo = Console.ReadKey(true);
                var key = keyInfo.Key;
                var keyChar = keyInfo.KeyChar;

                if (CharacterSelection && Char.IsDigit(keyChar)) {
                    int v = (int)Char.GetNumericValue(keyChar);
                    if (v < Items.Count) {
                        selectedIndex = v;
                        selected = true;
                    }
                }

                // Handle key press.
                if (key == ConsoleKey.Enter) selected = true;

                if (CycleOptions) {
                    if (key == plus) {
                        // If it's at the end, go to the first.
                        if (selectedIndex == Items.Count - 1) selectedIndex = 0;
                        // The usual.
                        else { selectedIndex++; }
                    } else if (key == minus) {
                        // If it's at zero, go to the right end.
                        if (selectedIndex == 0) selectedIndex = Items.Count - 1;
                        // The usual.
                        else { selectedIndex--; }
                    }
                } else {
                    // If it's not at the end, increment.
                    if (key == plus && selectedIndex < Items.Count - 1)
                        selectedIndex++;
                    // If it's not at the end, decrement.
                    else if (key == minus && selectedIndex > 0)
                        selectedIndex--;
                }

            }

            Console.CursorTop++;
            return selectedIndex;
        }
        public void DrawList(bool interaction, int selectedIndex, int y) {
            // Restore the original cursor position for menu rewriting.
            Console.SetCursorPosition(getLeftBuffer(), y);

            for (int i = 0; i < Items.Count; i++) {
                Console.CursorLeft =
                    Type == MType.Vertical ? getLeftBuffer() :
                    Console.CursorLeft;
                // Get the current item.
                string item = Items[i];

                // If it's the selected item, highlight it.
                if (interaction) colorScheme(i == selectedIndex);

                // Write the item to the console.
                Console.Write(alignString(item) +
                    (Type == MType.Vertical ? "\n" : ""));

                // Unhighlight the text.
                if (interaction) colorScheme(false);

                // Add padding.
                if (i + 1 < Items.Count) {
                    Console.CursorLeft +=
                        Type == MType.Horizontal ? XPadding : 0;
                    Console.CursorTop +=
                        Type == MType.Vertical ? YPadding : 0;
                }
            }
        }

        /*
         * Helper methods
         */
        private void colorScheme(bool highlighted) {
            // For the love of god, fine. Optional colours.
            if (highlighted) {
                Console.ForegroundColor = Color2;
                Console.BackgroundColor = Color1;
            } else {
                Console.ForegroundColor = Color1;
                Console.BackgroundColor = Color2;
            }
        }
        private string alignString(string str) {
            // Get the spacing difference.
            int diff = ItemWidth - str.Length;

            // If it's less than 0, then the string is too long.
            if (diff < 0) {
                // Remove the end and add "..." to indicate trimming.
                str = str.Remove(str.Length + diff - 3) + "...";
                // Recalculate the difference.
                diff = ItemWidth - str.Length;
            }
            switch (Alignment) {
                case MAlignment.Left:
                    // Pad on the right.
                    return str + new string(' ', diff);
                case MAlignment.Center:
                    // Pad on the center (biased to the left)
                    int div = diff - (diff / 2) * 2;
                    return new string(' ', diff / 2) + str +
                        new string(' ', diff / 2 + div);
                case MAlignment.Right:
                    // Pad on the left.
                    return new string(' ', diff) + str;
                default:
                    // Wow.
                    return "";
            }
        }
        private int menuWidth() {
            // If it's vertical, it's the size of the ItemWidth.
            if (Type == MType.Vertical) return ItemWidth;

            // Count the amount of items.
            int itemAddition = Items.Count * ItemWidth;
            // Count the total amount of padding (minus last item.)
            int padding = (Items.Count - 1) * XPadding;
            // Return total.
            return itemAddition + padding;
        }
        private int getLeftBuffer() {
            int value = MenuAlignment == MAlignment.Right ?
                       Console.BufferWidth - menuWidth() :
                        MenuAlignment == MAlignment.Center ?
                       (Console.BufferWidth - menuWidth()) / 2 : 0;
            if (value < 0) return 0;
            return value;
        }
    }
}
