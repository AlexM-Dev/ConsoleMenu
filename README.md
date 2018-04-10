# ConsoleMenu
A console helper class to display a menu.

## Usage
A menu is instantiated and given properties for when it is displayed. You can either choose to "Draw" the menu (interactive or not), or simply Display the Menu -- interactive always.

The `DisplayMenu()` method returns an int -- the nth item in the list of items. 

An example is shown below:

```
                List<string> items = new List<string>(new[] {
                    "  LOCAL: " + e.IpAddress + "  ",
                    "  ADDRESS: " + e.WebAddress + "  ",
                    "  COMMENT: " + e.Comment + "  ",
                    "  TOGGLE ENABLED (" + e.Enabled +")  ",
                    "    ",
                    "  [SAVE]  ",
                    "  [CANCEL]  "
                })
```

The above prepares a list for the Menu, the proceeding code creates the menu:

```
            Menu menu = new Menu() {
                ItemWidth = MaxLength(items),
                MenuAlignment = MAlignment.Center,
                Alignment = MAlignment.Left,
                Type = MType.Vertical,
                Items = items
            };
            return menu;
```

and to display the interactive menu:

```
int result = menu.DisplayMenu();
```

If the user selected the first option, it would return `0`, etc.
You can easily convert this to a string appropriate to the list by using the result as an index.
