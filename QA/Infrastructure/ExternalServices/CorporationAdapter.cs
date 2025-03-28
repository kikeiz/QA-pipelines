using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using QA.Application.Ports.Outbound;
using QA.Domain.Enums;
using Microsoft.Extensions.Logging;
using QA.Application.DTOs;
using System.Text.Json.Serialization;

namespace QA.Infrastructure.ExternalServices
{

    public class CorporationInfoAdapter(HttpClient httpClient, ILogger<CorporationInfoAdapter> logger) : ICorporationInfoAdapter
    {
        public async Task<CorporationInfoResponse> GetInfo(Guid shootingId)
        {
            logger.LogInformation("Fetching usage plan for ShootingId: {ShootingId}", shootingId);

            try
            {
                // Construct the request URL (relative to the base URL configured in Program.cs)
                string requestUrl = $"/corporation/{shootingId}";
                
                // Make the HTTP GET request
                var response = await httpClient.GetAsync(requestUrl);

                // Ensure the request was successful
                response.EnsureSuccessStatusCode();

                // Deserialize the response content
                var responseContent = await response.Content.ReadFromJsonAsync<CorporationApiResponse>()! 
                    ?? throw new InvalidOperationException("Response content is null.");
                    
                Console.Write(responseContent.ToString());
                
                // Map the response to the custom DTO
                return new CorporationInfoResponse
                {
                    BillingPlan = Enum.Parse<BillingPlan>(responseContent.BillingPlan, ignoreCase: true),
                    CorporationId = responseContent.CorporationId,
                    SelfQA = responseContent.SelfQA,
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to fetch corporation info for ShootingId: {ShootingId}", shootingId);
                throw;
            }
        }
    }
}
