using QA.Domain.Entities;
using QA.Domain.Enums;
using QA.Domain.ValueObjects;
using QA.Infrastructure.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QA.Application.Mappers.Persistence
{
    public static class RemarketingProcessingInfoMapper
    {
        public static RemarketingProcessingInfoModel? ToDbModel(RemarketingProcessingInfo? info)
        {
            if (info == null){
                return null;
            }

            return new RemarketingProcessingInfoModel
            {
                BackgroundType = info.BackgroundType,
                ExteriorCroppingProvider = info.ExteriorCroppingProvider,
                InteriorCroppingProvider = info.InteriorCroppingProvider,
                BackgroundUrl = info.BackgroundUrl,
                LogoUrl = info.LogoUrl,
                StamUrl = info.StampUrl,
                LpcUrl = info.LpcUrl,
                FloorUrl = info.FloorUrl,
                BlurringVBalue = (int?)info.BlurringValue,
                OutputDimensions = info.OutputDimensions != null
                ? new OutputDimensionsModel
                {
                    OutputWidth = info.OutputDimensions.OutputWidth,
                    OutputHeight = info.OutputDimensions.OutputHeight
                }
                : null
            };
        }

        public static RemarketingProcessingInfo FromDbModel(RemarketingProcessingInfoModel model)
        {
            return new RemarketingProcessingInfo
            {
                BackgroundType = model.BackgroundType,
                ExteriorCroppingProvider = model.ExteriorCroppingProvider,
                InteriorCroppingProvider = model.InteriorCroppingProvider,
                BackgroundUrl = model.BackgroundUrl ?? string.Empty,
                LogoUrl = model.LogoUrl ?? string.Empty,
                StampUrl = model.StamUrl ?? string.Empty,
                LpcUrl = model.LpcUrl ?? string.Empty,
                FloorUrl = model.FloorUrl ?? string.Empty,
                BlurringValue = model.BlurringVBalue ?? 0,
                OutputDimensions = new OutputDimensions(model?.OutputDimensions?.OutputHeight, model?.OutputDimensions?.OutputWidth)
            };
        }


    }
}
