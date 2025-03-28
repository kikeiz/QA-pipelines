using QA.Domain.Entities;
using QA.Domain.Enums;
using QA.Domain.ValueObjects;
using QA.Infrastructure.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QA.Application.Mappers.Persistence
{
    public static class MediaProcessingInfoMapper
    {
        public static MediaProcessingInfoModel ToDbModel(MediaProcessingInfo info)
        {
           return info switch
                {
                    ImageProcessingInfo imageInfo => new MediaProcessingInfoModel
                    {
                        Crop = imageInfo.Crop,
                        DesaturateYellow = imageInfo.DesaturateYellow,
                        Enhance = imageInfo.Enhance,
                        AddLogo = imageInfo.AddLogo,
                        AddLpc = imageInfo.AddLpc,
                        AddStamp = imageInfo.AddStamp,
                        AddShadow = imageInfo.AddShadow,
                        CorrectRotation = imageInfo.CorrectRotation,
                        Center = imageInfo.Center,
                        ZoomPercentage = imageInfo.ZoomPercentage,
                        HorizontalLineHeight = imageInfo.HorizontalLineHeight ?? 0,
                        CroppingProvider = imageInfo.CroppingProvider,
                        Margins = imageInfo?.Margins != null 
                            ? new MarginsModel
                            {
                                Top = imageInfo.Margins.Top,
                                Left = imageInfo.Margins.Left,
                                Bottom = imageInfo.Margins.Bottom,
                                Right = imageInfo.Margins.Right
                            }
                            : null
                    },
                    _ => throw new InvalidCastException($"Unexpected type: {info.GetType().Name}")
                };
        }

        public static MediaProcessingInfo FromDbModel(MediaType mediaType, MediaProcessingInfoModel model)
        {
            return mediaType switch
            {
                MediaType.Image => new ImageProcessingInfo
                (
                    model.Crop,
                    model.DesaturateYellow,
                    model.Enhance,
                    model.AddLogo,
                    model.AddLpc,
                    model.AddStamp,
                    model.CroppingProvider,
                    model.AddShadow,
                    model.CorrectRotation,
                    model.Center,
                    model.ZoomPercentage,
                    model.HorizontalLineHeight,
                    model.Margins != null
                        ? new Margins(model.Margins.Top, model.Margins.Left, model.Margins.Bottom, model.Margins.Right)
                        : null
                ),
                _ => throw new InvalidCastException($"Unexpected type: {mediaType}")
            };
           
        }
    }
}
