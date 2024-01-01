using MAUI.Dtos;
using MAUI.Services.ToDoSevices;
using System.Globalization;

namespace MAUI.Views;

public partial class AddToDoPage : ContentPage
{
	private readonly IToDo _toDo;
    public AddToDoPage(IToDo toDo)
    {
        InitializeComponent();
        _toDo = toDo;
    }

    private async void btn_Submit_Clicked(object sender, EventArgs e)
    {
		int SelectedIndex = picker.SelectedIndex;
		if (SelectedIndex == -1)
		{
			await DisplayAlert("خطا", "لطفا نوع فعالیت را انتخاب کنید", "Ok");
		}
		else
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
                await DisplayAlert("خطا", "لطفا تاریخ و ساعت را به درستی وارد کنید", "Ok");
                return;
            }

            string type = ((ToDoType)SelectedIndex).ToString();

            var result = await _toDo.AddToDo(new RequestAddTodoDto
            {
                Status = "InProgress",
                Title = entryTitle.Text,
                DateTime = gregorianDateTime,
                Type = type,
            });
            if(result.IsSuccess)
            {
                await DisplayAlert("موفق", $"جدید با موفقیت ثبت شد {picker.SelectedItem}", "Ok");
                await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
            }
            else
            {
                await DisplayAlert("خطا",result.Message, "Ok");

            }
        }

    }
}