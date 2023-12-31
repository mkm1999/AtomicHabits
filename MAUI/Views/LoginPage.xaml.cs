using MAUI.Services.AuthenticationServices;
using MAUI.Services.TokenService;
using System.Windows.Input;

namespace MAUI.Views;

public partial class LoginPage : ContentPage
{
    public ICommand TapCommand => new Command(async() => await Shell.Current.GoToAsync(nameof(SignUpPage)));

    private readonly IAuthentication authentication;

    public LoginPage(IAuthentication authentication)
    {
        InitializeComponent();
        BindingContext = this;
        this.authentication = authentication;
    }

    private async void btn_login_Clicked(object sender, EventArgs e)
    {
        var result = await authentication.login(entryUserName.Text,entryPassword.Text);
        if(result.IsSuccess == false)
        {
            await DisplayAlert("خطا", result.Message, "OK");
        }
        else
        {
            Token.SaveToken(result.Data);
            await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
        }
    }
}