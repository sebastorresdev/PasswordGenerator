using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using System;
using System.Text;
using Windows.ApplicationModel.DataTransfer;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PasswordGenerator;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window
{
    const string lowerCase = "abcdefghijklmnopqrstuvwxyz";
    const string numbers = "0123456789";
    const string symbol = "@?¡¿*+-{}[]:)(&%$#!";
    public MainWindow()
    {
        InitializeComponent();
        ExtendsContentIntoTitleBar = true;
        SetTitleBar(AppTitleBar);
        IntPtr hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
        WindowId WndID = Win32Interop.GetWindowIdFromWindow(hwnd);
        AppWindow appW = AppWindow.GetFromWindowId(WndID);
        appW.Resize(new Windows.Graphics.SizeInt32 { Width = 550, Height = 650 });
        OverlappedPresenter presenter = appW.Presenter as OverlappedPresenter;
        presenter.IsAlwaysOnTop = false;
        presenter.IsMaximizable = false;
        presenter.IsMinimizable = false;
        presenter.IsResizable = false;
        presenter.IsModal = false;
    }

    private void LengthSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
    {
        Slider slider = sender as Slider;
        if (slider is not null && LengthText is not null)
        {
            LengthText.Text = LengthSlider?.Value.ToString();
        }
    }

    private void PasswordGeneratorBtn_Click(object sender, RoutedEventArgs e)
    {
        int length = Convert.ToInt32(LengthText.Text);
        Random random = new();
        string text = "";
        text += IsLowercase.IsOn ? lowerCase : string.Empty;
        text += IsUppercase.IsOn ? lowerCase.ToUpper() : string.Empty;
        text += IsSymbol.IsOn ?    symbol   : string.Empty;
        text += IsNumber.IsOn ?    numbers  : string.Empty;

        StringBuilder sb = new();

        while (text.Length > 1 && sb.Length < length)
        {
            sb.Append(text[random.Next(text.Length)]);
        }
        PasswordOutput.Text = sb.ToString();
    }
    private void CopyText_Click(object sender, RoutedEventArgs e)
    {
        var package = new DataPackage();
        package.SetText(PasswordOutput.Text);
        Clipboard.SetContent(package);
    }
}
