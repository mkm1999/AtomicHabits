using MAUI.Services.AuthenticationServices;
using RestSharp.Authenticators;
using RestSharp;
using MAUI.Services.ToDoSevices;
using System.Globalization;
using MAUI.Dtos;

namespace MAUI.Views;

public partial class HomePage : ContentPage
{
    private  IAuthentication _authentication;
    private IToDo _ToDoService;
    public HomePage(IAuthentication authentication, IToDo toDoService)
    {
        InitializeComponent();
        //_ToDoService = toDoService;
        //_authentication = authentication;
    }
    protected override async void OnAppearing()
    {
        PersianCalendar pc = new PersianCalendar();
        lblDate.Text = pc.GetYear(DateTime.Now) + "/" + pc.GetMonth(DateTime.Now) + "/" + pc.GetDayOfMonth(DateTime.Now);

        _authentication = new Authentication();
        base.OnAppearing();
        bool ex = await _authentication.IsAuthenticated();
        if (ex == false)
        {
            await Shell.Current.GoToAsync(nameof(LoginPage));
        }
        else
        {
            _ToDoService = new ToDo();
            var result = await _ToDoService.GetToDosByDate(DateOnly.FromDateTime(DateTime.Now));
            listViewToDos.ItemsSource = result.Data;
        }
    }
    private async void btnAdd_Clicked(object sender, EventArgs e)
    {
    }

    private void btnDone_Clicked(object sender, EventArgs e)
    {

    }

    private void btnTransfer_Clicked(object sender, EventArgs e)
    {

    }

    private void btnDelete_Clicked(object sender, EventArgs e)
    {

    }
}