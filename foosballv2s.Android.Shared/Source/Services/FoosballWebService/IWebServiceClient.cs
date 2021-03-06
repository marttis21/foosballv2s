﻿using System.Threading.Tasks;

namespace foosballv2s.Droid.Shared.Source.Services.FoosballWebService
{
    public interface IWebServiceClient
    {
        Task<string> GetAsync(string endPointUri);
        Task<string> PostAsync(string endPointUri, string json);
        Task<string> PutAsync(string endPointUri, string json);
        Task<string> DeleteAsync(string endPointUri);
        void AddAuthorizationHeader();
    }
}