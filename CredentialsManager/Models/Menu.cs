using System;
using System.Collections.Generic;
using System.Text;

namespace CredentialsManager.Models
{
  public class Menu
  {
    public string Header { get; }
    public IList<MenuItem> MenuItems { get; }
    public MenuItem ExitItem { get; }

    public Menu(string header, IList<MenuItem> menuItems, MenuItem exitItem)
    {
      Header = header;
      MenuItems = menuItems;
      ExitItem = exitItem;
    }

    public MenuItem? this[ConsoleKey consoleKey]
    {
      get
      {
        foreach (var item in MenuItems)
        {
          if (item.ConsoleKeys.Contains(consoleKey))
          {
            return item;
          }
        }

        return ExitItem.ConsoleKeys.Contains(consoleKey) ? ExitItem : null;
      }
    }

    public override string ToString()
    {
      var sb = new StringBuilder();

      foreach (var item in MenuItems)
      {
        sb.Append(item);
        sb.AppendLine();
      }

      sb.Append(ExitItem);

      return sb.ToString();
    }
  }
}