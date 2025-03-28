using QA.Infrastructure.Persistence.Models;
using QA.Domain.Enums;

namespace IntegrationTests.Fixtures.Mocks
{
    public static class QaProcessMockGenerator
    {
        public static QAProcessModel GenerateMockProcess(string processId)
        {
            return new QAProcessModel
            {
                ProcessId = Guid.Parse(processId),
                RemarketingId = Guid.NewGuid(),
                ShootingId = Guid.NewGuid(),
                CorporationId = Guid.NewGuid(),
                Status = QAProcessStatus.Pending,
                SelfQA = true,
                Priority = ProcessPriority.High,
                UserId = Guid.NewGuid(),
                BillingPlan = BillingPlan.Advanced,
                Type = QAProcessType.Original,
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                PriorityTimestamp = DateTimeOffset.UtcNow.AddDays(1).ToUnixTimeSeconds(),

                ProcessingInfo = new RemarketingProcessingInfoModel
                {
                    BackgroundType = BackgroundType.Full,
                    BackgroundUrl = MockGenerator.GenerateMock<string>(),
                    LogoUrl = MockGenerator.GenerateMock<string>(),
                    StamUrl = MockGenerator.GenerateMock<string>(),
                    LpcUrl = MockGenerator.GenerateMock<string>(),
                    FloorUrl = MockGenerator.GenerateMock<string>(),
                    BlurringVBalue = MockGenerator.GenerateMock<int>(),
                    ExteriorCroppingProvider = CroppingProvider.REMOVE_BG,
                    InteriorCroppingProvider = CroppingProvider.NEXTLANE_AI,
                    OutputDimensions = new OutputDimensionsModel
                    {
                        OutputWidth = 1920,
                        OutputHeight = 1080
                    }
                },

                Media = [
                    new MediaModel
                    {
                        ID = Guid.NewGuid().ToString(),
                        Url = MockGenerator.GenerateMock<string>(),
                        OptimizedUrl = MockGenerator.GenerateMock<string>(),
                        RepushedUrl = MockGenerator.GenerateMock<string>(),
                        Position = 1,
                        ManuallyProcessed = false,
                        Type = MediaType.Image,
                        VehicleView = VehicleViews.Exterior,
                        MediaClassification = ImageTypes.NorthEast,
                        ProcessingInfo = new MediaProcessingInfoModel
                        {
                            Crop = true,
                            DesaturateYellow = false,
                            Enhance = true,
                            AddLogo = true,
                            AddLpc = false,
                            AddStamp = true,
                            AddShadow = true,
                            CorrectRotation = true,
                            Center = false,
                            ZoomPercentage = 10.0,
                            HorizontalLineHeight = 50.0,
                            CroppingProvider = CroppingProvider.REMOVE_BG,
                            Margins = new MarginsModel
                            {
                                Top = 5,
                                Bottom = 5,
                                Left = 10,
                                Right = 10
                            }
                        }
                    }
                ]
            };
        }
    }
}
