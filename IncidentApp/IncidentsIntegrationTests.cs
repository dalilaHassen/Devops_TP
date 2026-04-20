using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using WebApplication2.model;

namespace AppTests
{
    public class IncidentsIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        public IncidentsIntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }
        [Fact]
        public async Task GetIncidents_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/IncidentsDb");
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task PostIncident_CreatesIncident()
        {
            var incident = new
            {
                Title = "Test Incident",
                Description = "Test Description",
                Severity = "HIGH"
            };
            
            var response = await _client.PostAsJsonAsync("/api/IncidentsDb", incident);
            response.EnsureSuccessStatusCode();
            var createdIncident = await response.Content.ReadFromJsonAsync<Incident>();
            Assert.NotNull(createdIncident);
            Assert.Equal("Test Incident", createdIncident.title);
            Assert.Equal("HIGH", createdIncident.severity);
        }
    
    [Fact]
        public async Task PostThenGet_ReturnsInsertedIncident()
        {
            var incident = new
            {
                title = "Integration Test",
                description = "Test Description",
                severity = "MEDIUM"
            };
            await _client.PostAsJsonAsync("/api/IncidentsDb", incident);
            var response = await _client.GetAsync("/api/IncidentsDb");
            var data = await response.Content.ReadFromJsonAsync<List<Incident>>();
            Assert.Contains(data, i => i.title == "Integration Test");
        }
    }

}
