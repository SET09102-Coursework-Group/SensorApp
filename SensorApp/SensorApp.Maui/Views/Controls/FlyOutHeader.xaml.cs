namespace SensorApp.Maui.Views.Controls;

public partial class FlyOutHeader : StackLayout
{
    public FlyOutHeader()
    {
        InitializeComponent();

        if (App.UserInfo != null)
        {
            BindingContext = App.UserInfo;
        }
    }
}