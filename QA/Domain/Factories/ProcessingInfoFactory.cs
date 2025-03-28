using QA.Application.Ports.Outbound;
using QA.Domain.Enums;
using QA.Domain.ValueObjects;

namespace QA.Domain.Factories
{
    public static class ProcessingInfoFactory
    {
        public static MediaProcessingInfo Build(MediaType mediaType, CroppingProvider croppingProvider, ViewConfig configuration)
        {
            return mediaType switch {
                MediaType.Image => new ImageProcessingInfo(
                        configuration.Crop,
                        configuration.DesaturateYellow,
                        configuration.Enhance,
                        configuration.AddLogo,
                        configuration.AddLpc,
                        configuration.AddStamp,
                        croppingProvider,
                        configuration.AddShadow,
                        configuration.CorrectRotation,
                        configuration.Center,
                        configuration.ZoomPercentage ?? 0,
                        configuration.HorizontalLineHeight ?? null,
                        configuration.Margins
                    ),
                _ => throw new ArgumentException($"Unsupported processing info type: {mediaType}.")
            };
           
        }
    }
}
