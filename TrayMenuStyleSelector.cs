using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CasparLauncher
{
    public class TrayMenuStyleSelector : StyleSelector
    {
        public Style SeparatorStyle { get; set; }
        public Style MenuItemStyle { get; set; }
        public Style ExecutableItemStyle { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is Separator) return null;
            if (item is Executable) return ExecutableItemStyle;
            return MenuItemStyle;
        }
    }
}
