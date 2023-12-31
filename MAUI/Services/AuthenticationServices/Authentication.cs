using MAUI.Dtos;
using MAUI.Services.TokenService;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MAUI.Services.AuthenticationServices
{
    public class Authentication : IAuthentication
    {
        RestClientOptions RestClientOptions;
        RestClient client;

        public Authentication()
        {
            string token = Token.GetToken();
            if (string.IsNullOrEmpty(token)) token = "kopsdopk";
            RestClientOptions = new RestClientOptions("http://5.239.47.28:82/api")
            {
                Authenticator = new JwtAuthenticator(token)
            };

            client = new RestClient(RestClientOptions);
        }
        public async Task<bool> IsAuthenticated()
        {
            var request = new RestRequest("Authentication");
            var response = await client.ExecuteGetAsync<ResponseIsAuth>(request);
            return response.Data.IsAuthenticated;

        }

        public async Task<ResultDto<string>> login(string username, string password)
        {
            var request = new RestRequest("Authentication");
            request.AddBody(new
            {
                username = username,
                password = password
            });
            var response = await client.ExecutePostAsync(request);
            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                dynamic res = JObject.Parse(response.Content);
                return new ResultDto<string>
                {
                    Data = res.token,
                    IsSuccess = true,
                    Message = res.message
                };
            }
            else
            {
                dynamic res = JObject.Parse(response.Content);
                return new ResultDto<string>
                {
                    IsSuccess = false,
                    Message = res.message
                };
            }
        }

        public async Task<ResultDto> SignUp(string name, string lastName, string number, string userName, string password)
        {
            var request = new RestRequest("Authentication/SignUp");
            request.AddBody(new
            {
                name = name,
                lastName = lastName,
                number = number,
                userName = userName,
                password = password,
                imgSrc = ""
            });
            var response = await client.ExecutePostAsync(request);
            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                dynamic res = JObject.Parse(response.Content);
                return new ResultDto
                {
                    IsSuccess = true,
                    Message = res.message
                };
            }
            else
            {
                dynamic res = JObject.Parse(response.Content);
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = res.message
                };
            }
        }
    }

    public class ResponseIsAuth
    {
        public bool IsAuthenticated { get; set; }
    }
}
