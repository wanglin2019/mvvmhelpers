﻿using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using JulMar.Core;
using JulMar.Interfaces;
using JulMar.UI;

[assembly:ExportService(typeof(IMessageVisualizer), typeof(MessageVisualizer))]

namespace JulMar.UI
{
    /// <summary>
    /// This class implements the IMessageVisualizer for WPF
    /// </summary>
    sealed class MessageVisualizer : IMessageVisualizer
    {
        public Task<IUICommand> ShowAsync(string message, string title = "")
        {
            return this.ShowAsync(title, message, null);
        }

        public async Task<IUICommand> ShowAsync(string message, string title, MessageVisualizerOptions visualizerOptions)
        {
            if (visualizerOptions == null)
            {
                visualizerOptions = new MessageVisualizerOptions(UICommand.Ok);
            }

            var popup = new Window
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                SizeToContent = SizeToContent.WidthAndHeight,
                Background = SystemColors.ControlLightBrush,
                Foreground = SystemColors.ControlTextBrush,
                Title = title,
                MinWidth = 200,
                MinHeight = 100,
            };

            StackPanel rootPanel = new StackPanel();
            TextBlock messageText = new TextBlock
            {
                Text = message,
                Margin = new Thickness(20),
                TextWrapping = TextWrapping.Wrap,
                MaxWidth = SystemParameters.PrimaryScreenWidth / 2,
                MaxHeight = SystemParameters.FullPrimaryScreenWidth / 2,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            rootPanel.Children.Add(messageText);

            var commands = visualizerOptions.Commands;
            if (commands.Count == 0)
                commands = new[] { UICommand.Ok };

            IUICommand finalCommand = null;
            WrapPanel buttonPanel = new WrapPanel() { Margin = new Thickness(10), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
            for (int index = 0; index < commands.Count; index++)
            {
                var oneCommand = commands[index];
                IUICommand command = oneCommand;
                Button commandButton = new Button
                {
                    Content = command.Label,
                    Tag = command,
                    MinWidth = 75,
                    Margin = new Thickness(5),
                    Padding = new Thickness(10, 5, 10, 5),
                    IsDefault = index == visualizerOptions.DefaultCommandIndex,
                    IsCancel = index == visualizerOptions.CancelCommandIndex,
                };
                commandButton.Click += (s, e) =>
                {
                    if (command.Invoked != null)
                        command.Invoked();
                    finalCommand = ((Button)s).Tag as IUICommand;
                    popup.DialogResult = !((Button)s).IsCancel;
                };
                buttonPanel.Children.Add(commandButton);
            }

            rootPanel.Children.Add(buttonPanel);
            popup.Content = rootPanel;

            await Task.Run(
                () =>
                    {
                        bool? rc = null;
                        Application.Current.Dispatcher.Invoke(
                            () =>
                                {
                                    rc = popup.ShowDialog();
                                });
                    });

            return finalCommand;
        }
    }
}
