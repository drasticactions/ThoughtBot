// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using CommunityToolkit.Mvvm.DependencyInjection;
using Drastic.Tools;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ThoughtBot.WinUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.DataContext = this.ViewModel = Ioc.Default.GetService<AIChatViewModel>();
            this.ViewModel.OnSendingMessage += ViewModel_OnSendingMessage;
            this.ViewModel.OnLoad().FireAndForgetSafeAsync();
            this.ViewModel.Messages.CollectionChanged += Messages_CollectionChanged;
        }

        private void Messages_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
        }

        private void ViewModel_OnSendingMessage(object sender, EventArgs e)
        {
            this.InputBox.Text = string.Empty;
        }

        public AIChatViewModel ViewModel { get; }

        private void InputBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                if (this.ViewModel.SendMessageWithStringCommand.CanExecute(this.InputBox.Text))
                {
                    this.ViewModel.SendMessageWithStringCommand.ExecuteAsync(this.InputBox.Text).FireAndForgetSafeAsync();
                }
            }
        }
    }
}
