﻿using System.Threading.Tasks;
using foosballv2s.Droid.Shared.Source.Entities;
using foosballv2s.Droid.Shared.Source.Services.CredentialStorage;
using foosballv2s.Droid.Shared.Source.Services.FoosballWebService.Helpers;
using foosballv2s.Droid.Shared.Source.Services.FoosballWebService.Repository;
using Newtonsoft.Json;
using Xamarin.Forms;

[assembly: Dependency(typeof(TeamRepository))]
namespace foosballv2s.Droid.Shared.Source.Services.FoosballWebService.Repository
{
    /// <summary>
    /// A class for forming and fetching requests to a web service about teams
    /// </summary>
    public class TeamRepository
    {
        private readonly string endpointUrl = "/team";
        private IWebServiceClient client;
        private ICredentialStorage storage;

        public TeamRepository()
        {
            client = DependencyService.Get<IWebServiceClient>();
            storage = DependencyService.Get<ICredentialStorage>();
        }
       
        /// <summary>
        /// Gets all teams
        /// </summary>
        /// <returns></returns>
        public async Task<Team[]> GetAll(string urlParams = null)
        {
            var response = await client.GetAsync(endpointUrl + "/?" + urlParams);
            Team[] teams = FoosballJsonConvert.DeserializeObject<Team[]>(response);
            if (teams == null)
            {
                teams = new Team[]{};
            }
            return teams;
        }
        
        /// <summary>
        /// Gets one team by a given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Team> GetById(int id)
        {
            var response = await client.GetAsync(endpointUrl + "/" + id);
            Team team = FoosballJsonConvert.DeserializeObject<Team>(response);
            return team;
        }
        
        /// <summary>
        /// Creates a team
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        public async Task<Team> Create(Team team)
        {
            team.User = storage.GetCurrentUser();
            
            var teamJsonString = JsonConvert.SerializeObject(team);
            var response = await client.PostAsync(endpointUrl, teamJsonString);
            Team createdTeam = FoosballJsonConvert.DeserializeObject<Team>(response);
            return createdTeam;
        }
        
        /// <summary>
        /// Updates a team by a given id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="team"></param>
        /// <returns></returns>
        public async Task<Team> Update(int id, Team team)
        {
            team.User = storage.GetCurrentUser();
            
            var teamJsonString = JsonConvert.SerializeObject(team);
            var response = await client.PutAsync(endpointUrl + "/" + id , teamJsonString);
            Team updatedTeam = FoosballJsonConvert.DeserializeObject<Team>(response);
            return updatedTeam;
        }
        
        /// <summary>
        /// Deletes a team by a given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Team> Delete(int id)
        {
            var response = await client.DeleteAsync(endpointUrl + "/" + id);
            Team updatedTeam = FoosballJsonConvert.DeserializeObject<Team>(response);
            return updatedTeam;
        }
    }
}