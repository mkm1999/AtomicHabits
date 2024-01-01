using CommunityToolkit.Maui.Views;
using MAUI.Dtos;
using MAUI.Services.ToDoSevices;
using System.Globalization;

namespace MAUI.Views;

public partial class TransferToDoPopup : Popup
{
    private readonly int id;
    private readonly IToDo _ToDo;
    private readonly HomePage _HomePage;
    public TransferToDoPopup(int id, IToDo toDo, HomePage homePage)
    {
        InitializeComponent();
        this.id = id;
        _ToDo = toDo;
        _HomePage = homePage;
    }

    private async void btn_Submit_Clicked(object sender, EventArgs e)
    {
        var result = await _ToDo.UpdateToDo(new RequestEditTodoDto
        {
            Id = id,
            Status = "Transferred",
        });
        if (result.IsSuccess)
        {
            var todoToChange = await _ToDo.GetToDo(id);
            if (todoToChange.IsSuccess)
            {
                CultureInfo persianCulture = new CultureInfo("fa-IR");
                DateTime persianDateTime = default;
                DateTime gregorianDateTime = default;

                string DateTimeEntered = entryYear.Text + "/" + entryMonth.Text + "/" + entryDay.Text + " " + entryHour.Text + ":" + entryMinute.Text;
                try
                {
                    persianDateTime = DateTime.ParseExact(DateTimeEntered, "yyyy/MM/dd HH:mm", persianCulture);

                    PersianCalendar pc = new PersianCalendar();
                    // convert the Persian calendar date to Gregorian
                    gregorianDateTime = pc.ToDateTime(persianDateTime.Year, persianDateTime.Month, persianDateTime.Day, persianDateTime.Hour, persianDateTime.Minute, persianDateTime.Second, persianDateTime.Millisecond);
                }
                catch (Exception)
                {
                    lblError.Text = "لطفا تاریخ و ساعت را به درستی وارد کنید";
                    lblError.IsVisible = true;
                    return;
                }
                var result2 = await _ToDo.AddToDo(new RequestAddTodoDto
                {
                    DateTime = gregorianDateTime,
                    Status = "InProgress",
                    Title = todoToChange.Data.Title,
                    Type = todoToChange.Data.Type,
                });
                if (result2.IsSuccess)
                {
                    //lblError.Text = "با موفقیت موکول شد";
                    //lblError.TextColor = new Color(Color.Green);
                    //lblError.IsVisible = true;
                    //Thread.Sleep(1000);
                    await _HomePage.DisplayAlert("موفق", "با موفقیت موکول شد", "OK");
                    await _HomePage.Refresh();
                    Close();
                }
                else
                {
                    lblError.Text = result2.Message;
                    lblError.IsVisible = true;
                }
            }
            else
            {
                lblError.Text = todoToChange.Message;
                lblError.IsVisible = true;
            }
        }
        else
        {
            lblError.Text = result.Message;
            lblError.IsVisible = true;
        }
    }

    private void btn_Close_Clicked(object sender, EventArgs e)
    {
        Close();
    }
}