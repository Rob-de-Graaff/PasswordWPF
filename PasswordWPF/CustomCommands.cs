using System.Windows.Input;

namespace PasswordWPF
{
    public static class CustomCommands
    {
        public static readonly RoutedUICommand Create = new RoutedUICommand
               (
                       "Create",
                       "Create",
                       typeof(CustomCommands),
                       new InputGestureCollection()
                       {
                            new KeyGesture(Key.Insert, ModifierKeys.None)
                       }
               );

        public static readonly RoutedUICommand Update = new RoutedUICommand
                 (
                       "Update",
                       "Update",
                       typeof(CustomCommands),
                       new InputGestureCollection()
                       {
                            new KeyGesture(Key.Enter, ModifierKeys.None)
                       }
               );

        public static readonly RoutedUICommand Delete = new RoutedUICommand
               (
                       "Delete",
                       "Delete",
                       typeof(CustomCommands),
                       new InputGestureCollection()
                       {
                            new KeyGesture(Key.Delete, ModifierKeys.None)
                       }
               );
    }
}