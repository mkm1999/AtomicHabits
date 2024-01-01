using MAUI.Services.AuthenticationServices;
using RestSharp.Authenticators;
using RestSharp;
using MAUI.Services.ToDoSevices;
using System.Globalization;
using MAUI.Dtos;
using CommunityToolkit.Maui.Views;

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
            await Refresh();
        }
    }

    public async Task Refresh()
    {
        var result = await _ToDoService.GetToDosByDate(DateOnly.FromDateTime(DateTime.Now));
        listViewToDos.ItemsSource = result.Data;
    }

    private async void btnAdd_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AddToDoPage));
    }

    private async void btnDone_Clicked(object sender, EventArgs e)
    {
        dynamic senderdy = sender;
        int id = senderdy.CommandParameter;
        var result = await _ToDoService.UpdateToDo(new RequestEditTodoDto
        {
            Id = id,


            Status = "Finished"
        });
        if(result.IsSuccess)
        {
            await DisplayAlert("موفق", "تبریک کار شما با موفقیت انجام شد", "OK");
            await Refresh();
        }
        else
        {
            await DisplayAlert("خطا!", result.Message, "OK");
        }
    }

    private void btnTransfer_Clicked(object sender, EventArgs e)
    {
        dynamic senderdy = sender;
        int id = senderdy.CommandParameter;
        this.ShowPopup(new TransferToDoPopup(id,_ToDoService,this));
    }

    private async void btnDelete_Clicked(object sender, EventArgs e)
    {
        dynamic senderdy = sender;
        int id = senderdy.CommandParameter;
        var result = await _ToDoService.DeleteToDo(id);
        if (result.IsSuccess)
        {
            await DisplayAlert("موفق", result.Message, "OK");
            await Refresh();
        }
        else
        {
            await DisplayAlert("خطا!", result.Message, "OK");

        }
    }
}