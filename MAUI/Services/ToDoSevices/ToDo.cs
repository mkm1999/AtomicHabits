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
            RestClientOptions = new RestClientOptions("http://5.239.47.28:82/api")
            {
                Authenticator = new JwtAuthenticator(token)
            };

            client = new RestClient(RestClientOptions);
        }
        public async Task<ResultDto<List<ToDoDto>>> GetToDosByDate(DateOnly date)
        {
            var request = new RestRequest($"ToDo?date={date}");
            var response = await client.GetAsync<ResultDto<List<ToDoDto>>>(request);
            return response;
        }
    }
}
