using System;
using QA.Domain.Entities;
using QA.Domain.Enums;
using QA.Domain.ValueObjects;

namespace QA.Domain.Factories
{
    public static class MediaFactory
    {
        public static Media Build(Guid id, string url, string optimizedUrl, int position, Guid shootingId, VehicleViews view, ImageTypes imageType, MediaProcessingInfo processingInfo)
        {
            return processingInfo switch
            {
                ImageProcessingInfo imageInfo => new Image(
                    id,
                    url,
                    optimizedUrl,
                    shootingId,
                    position,
                    view,
                    [],
                    imageType,
                    imageInfo
                ),
                _ => throw new ArgumentException($"Unsupported processing info type: {processingInfo.GetType().Name}.", nameof(processingInfo))
            };
        }
    }
}
