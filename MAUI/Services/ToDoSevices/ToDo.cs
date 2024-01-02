using MAUI.Dtos;
using RestSharp.Authenticators;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MAUI.Services.TokenService;

namespace MAUI.Services.ToDoSevices
{
    public class ToDo : IToDo
    {

        RestClientOptions RestClientOptions;
        RestClient client;

        public ToDo()
        {
            string token = Token.GetToken();
            if (string.IsNullOrEmpty(token)) token = "kopsdopk";
            RestClientOptions = new RestClientOptions("http://todoapp.snapcalc.ir/api")
            {
                Authenticator = new JwtAuthenticator(token)
            };

            client = new RestClient(RestClientOptions);
        }

        public async Task<ResultDto> AddToDo(RequestAddTodoDto request)
        {
            var httpRequest = new RestRequest("ToDo");
            httpRequest.AddBody(request);
            var response = await client.PostAsync<ResultDto>(httpRequest);
            return response;
        }

        public async Task<ResultDto> DeleteToDo(int id)
        {
            var request = new RestRequest($"ToDo/{id}");
            var response = await client.DeleteAsync<ResultDto>(request);
            return response;
        }

        public async Task<ResultDto<List<ToDoDto>>> GetToDosByDate(DateOnly date)
        {
            var request = new RestRequest($"ToDo?date={date}");
            var response = await client.GetAsync<ResultDto<List<ToDoDto>>>(request);
            return response;
        }

        public async Task<ResultDto> UpdateToDo(RequestEditTodoDto request)
        {
            var httpRequest = new RestRequest("ToDo");
            httpRequest.AddBody(request);
            var response = await client.PutAsync<ResultDto>(httpRequest);
            return response;
        }
        public async Task<ResultDto<ToDoDto>> GetToDo(int id)
        {
            var request = new RestRequest($"ToDo/GetToDo/{id}");
            var response = await client.GetAsync<ResultDto<ToDoDto>>(request);
            return response;
        }
    }
}
