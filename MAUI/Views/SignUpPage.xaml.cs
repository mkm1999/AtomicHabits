using MAUI.Services.AuthenticationServices;
using MAUI.Services.TokenService;

namespace MAUI.Views;

public partial class SignUpPage : ContentPage
{
    private readonly IAuthentication authentication;
    public SignUpPage(IAuthentication authentication)
    {
        InitializeComponent();
        this.authentication = authentication;
    }

    private async void btn_SignUp_Clicked(object sender, EventArgs e)
    {
		var result = await authentication.SignUp(entryName.Text,entryLastName.Text,entryNumber.Text,entryUserName.Text,entryPassword.Text);
        if(result.IsSuccess == false)
        {
            await DisplayAlert("Error", result.Message, "OK");
        }
        else
        {
            var signInresult = await authentication.login(entryUserName.Text, entryPassword.Text);
            if (signInresult.IsSuccess == false)
            {
                await Shell.Current.GoToAsync(nameof(LoginPage));
            }
            else
            {
                Token.SaveToken(signInresult.Data);
                await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
            }
        }
    }
    protected override bool OnBackButtonPressed()
    {
        Shell.Current.GoToAsync(nameof(LoginPage));
        return true;
    }
}