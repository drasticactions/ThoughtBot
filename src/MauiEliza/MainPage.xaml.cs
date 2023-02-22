using System.Collections.ObjectModel;
using System.Reflection;
using Microsoft.Maui.Controls.Shapes;
using ELIZA.NET;
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;

namespace MauiEliza;

public partial class MainPage : ContentPage
{
    ObservableCollection<Line> _dialog = new ObservableCollection<Line>();
    Session _session;

    Random _random;

    public MainPage()
	{
		InitializeComponent();
        var eliza = new ELIZA.NET.ELIZALib(GetResourceFileContentAsString("DOCTOR.json"));
        _session = eliza.Session;

        Conversation.ItemsSource = _dialog;

        _dialog.Add(new Line(Speaker.Eliza, _session.GetGreeting()));

        _random = new Random(DateTime.Now.Second);

        App.Current.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        this.AddItem(UserEntry.Text);
    }

    void UserEntry_Completed(System.Object sender, System.EventArgs e)
    {
        this.AddItem(UserEntry.Text);
    }

    private async void AddItem(string userText)
    {
        if (string.IsNullOrEmpty(userText))
        {
            return;
        }

        UserEntry.Text = string.Empty;

        _dialog.Add(new Line(Speaker.User, userText));

        var delay = (int)(1000 * _random.Next(0, 3) + (_random.NextDouble() * 1000));

        await Task.Delay(delay);

        _dialog.Add(new Line(Speaker.Eliza, _session.GetResponse(userText)));

        UserEntry.Focus();
    }

    public static string GetResourceFileContentAsString(string fileName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        if (assembly is null)
        {
            return string.Empty;
        }

        var resourceName = "MauiEliza." + fileName;

        string? resource = null;
        using (Stream? stream = assembly.GetManifestResourceStream(resourceName))
        {
            if (stream is null)
            {
                return string.Empty;
            }

            using StreamReader reader = new StreamReader(stream);
            resource = reader.ReadToEnd();
        }

        return resource ?? string.Empty;
    }
}

public class SpeakerSelector : DataTemplateSelector
{
    public DataTemplate ElizaTemplate { get; set; }
    public DataTemplate UserTemplate { get; set; }

    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {
        var line = (Line)item;

        return line.Speaker == Speaker.User
            ? UserTemplate
            : ElizaTemplate;
    }
}

public enum Speaker
{
    Eliza,
    User
}

public class Line
{
    public Line(Speaker speaker, string text)
    {
        Speaker = speaker;
        Text = text;
    }

    public Speaker Speaker { get; set; }
    public string Text { get; set; }
}
