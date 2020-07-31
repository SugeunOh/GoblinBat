﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;

using RestSharp;

using ShareInvest.Catalog;
using ShareInvest.Interface;

namespace ShareInvest.Client
{
    public sealed class GoblinBatClient
    {
        public static GoblinBatClient GetInstance() => Client;
        public static GoblinBatClient GetInstance(dynamic key)
        {
            if (Client == null && new Security(key).GrantAccess)
            {
                Client = new GoblinBatClient(key);
                Coin = 0x2710;
            }
            return Client;
        }
        public static int Coin
        {
            get; private set;
        }
        public async Task<int> EmergencyContext<T>(Codes param) => (int)(await client.ExecuteAsync<T>(new RestRequest(string.Concat(security.CoreAPI, param.GetType().Name, "/", param.Code), Method.PUT).AddJsonBody(param, security.ContentType), source.Token)).StatusCode;
        public async Task<Retention> EmergencyContext<T>(Queue<T> param) where T : struct => JsonConvert.DeserializeObject<Retention>((await client.ExecuteAsync(new RestRequest(string.Concat(security.CoreAPI, param.GetType().GetGenericArguments()[0].Name), Method.POST).AddHeader(security.ContentType, security.Json).AddParameter(security.Json, JsonConvert.SerializeObject(param), ParameterType.RequestBody), source.Token)).Content);
        public async Task<IEnumerable<Charts>> GetContext(Catalog.Request.Charts chart)
        {
            var response = await client.ExecuteAsync(new RestRequest(security.RequestCharts(chart), Method.GET), source.Token);

            try
            {
                Coin--;

                return JsonConvert.DeserializeObject<IEnumerable<Charts>>(response.Content);
            }
            catch (Exception ex)
            {
                SendMessage(ex.StackTrace);
                SendMessage((int)response.StatusCode);
            }
            return null;
        }
        public async Task<Codes> GetContext(Codes codes)
        {
            var response = await client.ExecuteAsync(new RestRequest(string.Concat(security.CoreAPI, codes.GetType().Name, "/", codes.Code), Method.GET), source.Token);

            try
            {
                Coin--;

                return JsonConvert.DeserializeObject<Codes>(response.Content);
            }
            catch (Exception ex)
            {
                SendMessage(ex.StackTrace);
                SendMessage((int)response.StatusCode);
            }
            return codes;
        }
        public async Task<object> GetContext(Codes param, int length)
        {
            var response = await client.ExecuteAsync(new RestRequest(string.Concat(security.CoreAPI, param.GetType().Name, "/", length), Method.GET), source.Token);

            try
            {
                Coin--;

                return JsonConvert.DeserializeObject<List<Codes>>(response.Content);
            }
            catch (Exception ex)
            {
                SendMessage(ex.StackTrace);
                SendMessage((int)response.StatusCode);
            }
            return null;
        }
        public async Task<object> GetContext<T>(IParameters param)
        {
            var response = await client.ExecuteAsync<T>(new RestRequest(string.Concat(security.CoreAPI, param.GetType().Name, "/", param.Security), Method.GET), source.Token);

            try
            {
                Coin--;

                switch (param)
                {
                    case Privacies _:
                        return JsonConvert.DeserializeObject<Privacies>(response.Content);
                }
            }
            catch (Exception ex)
            {
                SendMessage(ex.StackTrace);
                SendMessage((int)response.StatusCode);
            }
            return null;
        }
        public async Task<Retention> GetContext<T>(T param) where T : struct, ICharts
        {
            var response = await client.ExecuteAsync(new RestRequest(string.Concat(security.CoreAPI, param.GetType().Name), Method.GET), source.Token);

            try
            {
                Coin--;

                return JsonConvert.DeserializeObject<Retention>(response.Content);
            }
            catch (Exception ex)
            {
                SendMessage(ex.StackTrace);
                SendMessage((int)response.StatusCode);
            }
            return new Retention
            {
                Code = string.Empty,
                LastDate = string.Empty
            };
        }
        public async Task<Retention> PostContext<T>(Queue<T> param) where T : struct, ICharts
        {
            var response = await client.ExecuteAsync(new RestRequest(string.Concat(security.CoreAPI, param.GetType().GetGenericArguments()[0].Name), Method.POST).AddHeader(security.ContentType, security.Json).AddParameter(security.Json, JsonConvert.SerializeObject(param), ParameterType.RequestBody), source.Token);

            try
            {
                Coin--;

                return JsonConvert.DeserializeObject<Retention>(response.Content);
            }
            catch (Exception ex)
            {
                SendMessage(ex.StackTrace);
                SendMessage((int)response.StatusCode);
            }
            return new Retention
            {
                Code = string.Empty,
                LastDate = string.Empty
            };
        }
        public async Task<object> PostContext<T>(IParameters param)
        {
            var response = await client.ExecuteAsync(new RestRequest(string.Concat(security.CoreAPI, param.GetType().Name), Method.POST).AddJsonBody(param, security.ContentType), source.Token);

            try
            {
                Coin--;

                switch (param)
                {
                    case Privacies _:
                        return (int)response.StatusCode;
                }
            }
            catch (Exception ex)
            {
                SendMessage(ex.StackTrace);
                SendMessage((int)response.StatusCode);
            }
            return null;
        }
        public async Task<object> PostContext<T>(IEnumerable<T> param)
        {
            var response = await client.ExecuteAsync(new RestRequest(string.Concat(security.CoreAPI, param.GetType().GetGenericArguments()[0].Name), Method.POST).AddHeader(security.ContentType, security.Json).AddParameter(security.Json, JsonConvert.SerializeObject(param), ParameterType.RequestBody), source.Token);

            try
            {
                Coin--;

                switch (param)
                {
                    case IEnumerable<Codes> _:
                        return response.IsSuccessful;
                }
            }
            catch (Exception ex)
            {
                SendMessage(ex.StackTrace);
                SendMessage((int)response.StatusCode);
            }
            return null;
        }
        public async Task<int> PutContext<T>(Codes param)
        {
            var response = await client.ExecuteAsync<T>(new RestRequest(string.Concat(security.CoreAPI, param.GetType().Name, "/", param.Code), Method.PUT).AddJsonBody(param, security.ContentType), source.Token);

            try
            {
                Coin--;

                return (int)response.StatusCode;
            }
            catch (Exception ex)
            {
                SendMessage(ex.StackTrace);
                SendMessage((int)response.StatusCode);
            }
            return int.MinValue;
        }
        public async Task<int> PutContext<T>(IParameters param)
        {
            var response = await client.ExecuteAsync(new RestRequest(string.Concat(security.CoreAPI, param.GetType().Name, "/", param.Security), Method.PUT, DataFormat.Json).AddJsonBody(param, security.ContentType), source.Token);

            try
            {
                Coin--;

                return (int)response.StatusCode;
            }
            catch (Exception ex)
            {
                SendMessage(ex.StackTrace);
                SendMessage((int)response.StatusCode);
            }
            return int.MinValue;
        }
        public async Task<int> DeleteContext<T>(IParameters param)
        {
            var response = await client.ExecuteAsync<T>(new RestRequest(string.Concat(security.CoreAPI, param.GetType().Name, "/", param.Security), Method.DELETE), source.Token);

            try
            {
                Coin--;

                return (int)response.StatusCode;
            }
            catch (Exception ex)
            {
                SendMessage(ex.StackTrace);
                SendMessage((int)response.StatusCode);
            }
            return int.MinValue;
        }
        static GoblinBatClient Client
        {
            get; set;
        }
        [Conditional("DEBUG")]
        void SendMessage(object code)
        {
            if (code is int response && response > 200)
                Console.WriteLine(response);

            else if (code is string str)
                Console.WriteLine(str);
        }
        GoblinBatClient(dynamic key)
        {
            security = new Security(key);
            client = new RestClient(security.Url)
            {
                Timeout = -1
            };
            source = new CancellationTokenSource();
        }
        readonly CancellationTokenSource source;
        readonly Security security;
        readonly IRestClient client;
    }
}