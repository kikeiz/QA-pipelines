using Bogus;
using QA.Application.DTOs;
using QA.Domain.Enums;

namespace IntegrationTests.Fixtures.Custom
{
    public class FakeDataManager
    {
        public static List<MaskModel> GenerateViewData()
        {
            int position = 1;
            return new Faker<MaskModel>()
                .RuleFor(x => x.Position, (f, t) => position++) 
                .RuleFor(x => x.Crop, (f, t) => f.Random.Bool())
                .RuleFor(x => x.AddShadow, (f, t) => f.Random.Bool())
                .RuleFor(x => x.DesaturateYellow, (f, t) => f.Random.Bool())
                .RuleFor(x => x.Enhance, (f, t) => f.Random.Bool())
                .RuleFor(x => x.AddLpc, (f, t) => f.Random.Bool())
                .RuleFor(x => x.AddLogo, (f, t) => f.Random.Bool())
                .RuleFor(x => x.AddStamp, (f, t) => f.Random.Bool())
                .RuleFor(x => x.CorrectRotation, (f, t) => f.Random.Bool())
                .RuleFor(x => x.Center, (f, t) => f.Random.Bool())
                .RuleFor(x => x.ZoomPercentage, (f, t) => f.Random.Double(0.5, 2.0))
                .RuleFor(x => x.HorizontalLineHeight, (f, t) => f.Random.Double(0.1, 5.0))
                .RuleFor(x => x.MarginRight, (f, t) => f.Random.Int(0, 50))
                .RuleFor(x => x.MarginLeft, (f, t) => f.Random.Int(0, 50))
                .RuleFor(x => x.MarginTop, (f, t) => f.Random.Int(0, 50))
                .RuleFor(x => x.MarginBottom, (f, t) => f.Random.Int(0, 50))
                .RuleFor(x => x.View, _ => VehicleViews.Exterior.ToString())
                .RuleFor(x => x.ImageType, _ => ImageTypes.East.ToString())
                .Generate(2);
        }

        public static Stamp GenerateStampData()
        {
            return new Faker<Stamp>()
                .RuleFor(x => x.BackgroundUrl, (f, t) => f.Internet.Url())
                .RuleFor(x => x.StampUrl, (f, t) => f.Internet.Url())
                .RuleFor(x => x.FloorUrl, (f, t) => f.Internet.Url())
                .RuleFor(x => x.LPCUrl, (f, t) => f.Internet.Url())
                .RuleFor(x => x.LogoUrl, (f, t) => f.Internet.Url())
                .RuleFor(x => x.BackgroundType, _ => BackgroundType.FadedCropping.ToString())
                .RuleFor(x => x.InteriorBackgroundUrl, (f, t) => f.Internet.Url())
                .RuleFor(x => x.ExteriorCroppingProvider, _ => CroppingProvider.NEXTLANE_AI.ToString())
                .RuleFor(x => x.InteriorCroppingProvider, _ => CroppingProvider.NEXTLANE_AI.ToString())
                .RuleFor(x => x.OutputWidth, (f, t) => f.Random.Int(500, 5000))
                .RuleFor(x => x.OutputHeight, (f, t) => f.Random.Int(500, 5000));
        }

        public static CorporationApiResponse GenerateCorporationData()
        {
            return new Faker<CorporationApiResponse>()
                .RuleFor(x => x.CorporationId, (f, t) => Guid.NewGuid())
                .RuleFor(x => x.SelfQA, (f, t) => f.Random.Bool())
                .RuleFor(x => x.BillingPlan, _ => BillingPlan.Platinum.ToString());
        }
    }
}
