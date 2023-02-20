using System.Linq;
using Drastic.Tools;

namespace ThoughtBot.DotNetMaui;

public partial class MainPage : ContentPage
{
    public MainPage(IServiceProvider provider)
    {
        this.InitializeComponent();
        this.BindingContext = this.ViewModel = provider.GetService<AIChatViewModel>()!;
        this.ViewModel.Messages.CollectionChanged += Messages_CollectionChanged;
    }

    private void Messages_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
        {
            if (this.ViewModel.Messages.Count > 1)
            {
                // ScrollTo for CollectionView works on Android. Not WinUI, need to test iOS/Catalyst...
                this.MessageCollectionView.ScrollTo(e.NewStartingIndex, -1, ScrollToPosition.MakeVisible, true);
                // this.MessageListView.ScrollTo(e.NewItems[0], ScrollToPosition.Start, true);
            }
        }
    }

    internal AIChatViewModel ViewModel { get; }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        this.ViewModel.OnLoad().FireAndForgetSafeAsync();
    }

    void InputBox_Completed(System.Object sender, System.EventArgs e)
    {
        if (this.ViewModel.SendMessageCommand.CanExecute())
        {
            this.ViewModel.SendMessageCommand.ExecuteAsync().FireAndForgetSafeAsync();
            this.InputBox.Focus();
        }
    }
}

