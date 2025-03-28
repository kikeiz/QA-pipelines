using QA.Domain.Entities;
using QA.Domain.Enums;
using QA.Domain.ValueObjects;
using QA.Infrastructure.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QA.Application.Mappers.Persistence
{
    public static class MediaMapper
    {
        public static MediaModel ToDbModel(Media media)
        {
            return new MediaModel
            {
                ID = media.Id.ToString(),
                Url = media.OriginalUrl,
                OptimizedUrl = media.FinalUrl,
                RepushedUrl = media.RepushedUrl,
                Position = media.Position,
                Type = media.MediaType,
                ManuallyProcessed = media.ManuallyProcessed,
                VehicleView = media.View,
                MediaClassification = media switch {
                    Image image => image.Type,
                    _ => ImageTypes.Video
                },
                ProcessingInfo = MediaProcessingInfoMapper.ToDbModel(media.ProcessingInfo)
            };
        }

        public static Media FromDbModel(MediaModel model, Guid shootingId)
        {
            return MediaProcessingInfoMapper.FromDbModel(model.Type, model.ProcessingInfo) switch {
                ImageProcessingInfo info => new Image(
                    Guid.Parse(model.ID), 
                    model.Url, 
                    model.OptimizedUrl,
                    shootingId, 
                    model.Position, 
                    model.VehicleView, 
                    [], 
                    model.MediaClassification, 
                    info,
                    model.ManuallyProcessed
                ),
                _ => throw new InvalidCastException($"Unexpected type: {model.Type}")
            };
        }
    }

};
