using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using QA.Application.Ports.Outbound;
using QA.Domain.Enums;
using Microsoft.Extensions.Logging;
using QA.Application.DTOs;
using System.Text.Json.Serialization;
using QA.Domain.ValueObjects;

namespace QA.Infrastructure.ExternalServices
{

    public class ProcessingInfoAdapter(HttpClient httpClient, ILogger<ProcessingInfoAdapter> logger) : IProcessingInfoAdater
    {
        public async Task<ProcessingInfoResponse> GetInfo(Guid shootingId)
        {
            logger.LogInformation("Fetching ProcessingInfo for ShootingId: {ShootingId}", shootingId);

            try
            {
                // Construct the request URL (relative to the base URL configured in Program.cs)
                string requestUrl = $"/configuration/{shootingId}";
                
                // Make the HTTP GET request
                var response = await httpClient.GetAsync(requestUrl);

                // Ensure the request was successful
                response.EnsureSuccessStatusCode();

                // Deserialize the response content
                var responseContent = await response.Content.ReadFromJsonAsync<MediaApiResponse>()! 
                    ?? throw new InvalidOperationException("Response content is null.");
                    
                Console.Write(responseContent.ToString());
                
                // Map GeneralConfig from the Stamp object
                var generalConfig = new RemarketingProcessingInfo
                {
                    BackgroundUrl = responseContent.Stamp.BackgroundUrl,
                    StampUrl = responseContent.Stamp.StampUrl,
                    LogoUrl = responseContent.Stamp.LogoUrl,
                    LpcUrl = responseContent.Stamp.LPCUrl,
                    FloorUrl = responseContent.Stamp.FloorUrl,
                    ExteriorCroppingProvider = Enum.Parse<CroppingProvider>(responseContent.Stamp.ExteriorCroppingProvider, ignoreCase: true),
                    InteriorCroppingProvider = Enum.Parse<CroppingProvider>(responseContent.Stamp.InteriorCroppingProvider, ignoreCase: true),
                    BackgroundType = Enum.Parse<BackgroundType>(responseContent.Stamp.BackgroundType, ignoreCase: true),
                    OutputDimensions = new OutputDimensions(responseContent.Stamp.OutputHeight, responseContent.Stamp.OutputWidth)
                };

                // Map ViewConfig from the Views collection
                var viewConfigs = responseContent.Views.ConvertAll(view => new ViewConfig
                {
                    Position = view.Position,
                    Crop = view.Crop,
                    AddShadow = view.AddShadow,
                    DesaturateYellow = view.DesaturateYellow,
                    Enhance = view.Enhance,
                    AddLpc = view.AddLpc,
                    AddLogo = view.AddLogo,
                    AddStamp = view.AddStamp,
                    CorrectRotation = view.CorrectRotation,
                    Center = view.Center,
                    ZoomPercentage = view.ZoomPercentage,
                    HorizontalLineHeight = view.HorizontalLineHeight,
                    Margins = new Margins(view.MarginTop, view.MarginLeft, view.MarginBottom, view.MarginRight),
                    View = Enum.Parse<VehicleViews>(view.View, ignoreCase: true),
                    ImageType = Enum.Parse<ImageTypes>(view.ImageType, ignoreCase: true),
                });

                // Create and return the ProcessingInfoResponse
                return new ProcessingInfoResponse
                {
                    GeneralConfiguration = generalConfig,
                    ViewConfiguration = viewConfigs
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to fetch processing info for ShootingId: {ShootingId}", shootingId);
                throw;
            }
        }
    }
}
