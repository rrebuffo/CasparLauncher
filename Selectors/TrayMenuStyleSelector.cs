namespace CasparLauncher;

public class TrayMenuStyleSelector : StyleSelector
{
    public Style? SeparatorStyle { get; set; }
    public Style? MenuItemStyle { get; set; }
    public Style? ExecutableItemStyle { get; set; }

    public override Style? SelectStyle(object item, DependencyObject container)
    {
        if (item is Separator) return SeparatorStyle;
        if (item is Executable) return ExecutableItemStyle;
        return MenuItemStyle;
    }
}
