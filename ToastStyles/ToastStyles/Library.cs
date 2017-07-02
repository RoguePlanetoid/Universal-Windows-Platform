using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

public class Library
{
    public IEnumerable<string> GetTemplates()
    {
        return Enum.GetValues(typeof(ToastTemplateType))
            .Cast<ToastTemplateType>().Select(s => s.ToString());
    }

    private void UpdateToast(string style, string value)
    {
        ToastTemplateType template = (ToastTemplateType)Enum.Parse(typeof(ToastTemplateType), style);
        XmlDocument toast = ToastNotificationManager.GetTemplateContent(template);
        XmlNodeList text = toast.GetElementsByTagName("text");
        if (text.Length > 0)
        {
            text[0].AppendChild(toast.CreateTextNode(value));
        }
        XmlNodeList image = toast.GetElementsByTagName("image");
        if (image.Length > 0)
        {
            image[0].Attributes.GetNamedItem("src").NodeValue =
            "Assets/Square44x44Logo.scale-200.png";
        }
        ToastNotification notification = new ToastNotification(toast);
        ToastNotificationManager.CreateToastNotifier().Show(notification);
    }

    private async Task<Tuple<string, string>> Dialog()
    {
        ComboBox template = new ComboBox()
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Margin = new Thickness(5),
            ItemsSource = GetTemplates()
        };
        template.SelectedIndex = 0;
        TextBox text = new TextBox()
        {
            PlaceholderText = "Text",
            Margin = new Thickness(5)
        };
        StackPanel panel = new StackPanel()
        {
            Orientation = Orientation.Vertical
        };
        panel.Children.Add(template);
        panel.Children.Add(text);
        ContentDialog dialog = new ContentDialog()
        {
            Title = "Toast Styles",
            PrimaryButtonText = "Update",
            CloseButtonText = "Cancel",
            Content = panel
        };
        ContentDialogResult result = await dialog.ShowAsync();
        if (result == ContentDialogResult.Primary)
        {
            return new Tuple<string, string>((string)template.SelectedItem, text.Text);
        }
        return null;
    }

    public async void Toast()
    {
        Tuple<string, string> result = await Dialog();
        if (result != null)
        {
            UpdateToast(result.Item1, result.Item2);
        }
    }
}