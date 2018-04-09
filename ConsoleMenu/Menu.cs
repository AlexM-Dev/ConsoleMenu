using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMenu {
    class Menu {
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

                var key = Console.ReadKey().Key;

                // Handle key press.
                if (key == plus && selectedIndex < Items.Count - 1) selectedIndex++;
                else if (key == minus && selectedIndex > 0) selectedIndex--;
                else if (key == ConsoleKey.Enter) selected = true;
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
                colorScheme(i == selectedIndex);

                // Write the item to the console.
                Console.Write(alignString(item) +
                    (Type == MType.Vertical ? "\n" : ""));

                // Unhighlight the text.
                colorScheme(false);

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
            // Statically written colours because I honestly can't 
            // be fucked.
            if (highlighted) {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.Gray;
            } else {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.BackgroundColor = ConsoleColor.Black;
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
            return MenuAlignment == MAlignment.Right ?
                       Console.BufferWidth - menuWidth() :
                   MenuAlignment == MAlignment.Center ?
                       (Console.BufferWidth - menuWidth()) / 2 : 0;
        }
    }
}
