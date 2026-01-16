using B4.Api;
using B4.Shared;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;
using Xunit;

namespace B4.Tests
{
    public class EpigrafeIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public EpigrafeIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAll_Unauthenticated_Returns401()
        {
            var response = await _client.GetAsync("/api/Epigrafe");
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GetAll_Authenticated_Returns200()
        {
            // 1️ Generar token JWT (simulando login)
            var jwtService = new JwtService(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build());
            var token = jwtService.GenerateToken(userId: 1, role: "Admin");

            // 2️ Poner token en Authorization header
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // 3️ Llamar endpoint protegido
            var response = await _client.GetAsync("/api/Epigrafe");

            // 4️ Verificar
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNullOrEmpty();
        }
    }
}
