using System.Text.Json.Serialization;

namespace QA.Application.DTOs
    {
        public class Stamp
        {
            [JsonPropertyName("backgroundUrl")]
            public required string BackgroundUrl { get; set; }

            [JsonPropertyName("stampUrl")]
            public required string StampUrl { get; set; }

            [JsonPropertyName("floorUrl")]
            public required string FloorUrl { get; set; }

            
            [JsonPropertyName("lpcUrl")]
            public required string LPCUrl { get; set; }

            [JsonPropertyName("logoUrl")]
            public required string LogoUrl { get; set; }

            [JsonPropertyName("backgroundType")]
            public required string BackgroundType { get; set; }

            [JsonPropertyName("interiorBackgroundUrl")]
            public required string InteriorBackgroundUrl { get; set; }

            [JsonPropertyName("exteriorCroppingProvider")]
            public required string ExteriorCroppingProvider { get; set; }

            [JsonPropertyName("interiorCroppingProvider")]
            public required string InteriorCroppingProvider { get; set; }


            [JsonPropertyName("outputWidth")]
            public required int OutputWidth { get; set; }


            [JsonPropertyName("outputHeight")]
            public required int OutputHeight { get; set; }
        }
        public class MaskModel
        {
            [JsonPropertyName("position")]
            public int Position { get; set; }

            [JsonPropertyName("cropped")]
            public bool Crop { get; set; }

            [JsonPropertyName("shadow")]
            public bool AddShadow { get; set; }
            [JsonPropertyName("yellow_color_treatment")]
            public bool DesaturateYellow { get; set; }
            [JsonPropertyName("enhance")]
            public bool Enhance { get; set; }
            [JsonPropertyName("license_plate_cover")]
            public bool AddLpc { get; set; }
            [JsonPropertyName("background_logo")]
            public bool AddLogo { get; set; }
            [JsonPropertyName("stamp")]
            public bool AddStamp { get; set; }
            [JsonPropertyName("correct_rotation")]
            public bool CorrectRotation { get; set; }
            [JsonPropertyName("center")]
            public bool Center { get; set; }
            [JsonPropertyName("zoom_percentage")]
            public double ZoomPercentage { get; set; }
            [JsonPropertyName("horizontal_line_height")]
            public double HorizontalLineHeight { get; set; }
            [JsonPropertyName("margin_right")]
            public int MarginRight { get; set; }
            [JsonPropertyName("margin_left")]
            public int MarginLeft { get; set; }
            [JsonPropertyName("margin_top")]
            public int MarginTop { get; set; }
            [JsonPropertyName("margin_bottom")]
            public int MarginBottom { get; set; }
            [JsonPropertyName("mask_category")]
            public required string View { get; set; }

            [JsonPropertyName("mask_type")]
            public required string ImageType { get; set; }
        }
        public class MediaApiResponse
        {
            [JsonPropertyName("stamp")]
            public required Stamp Stamp { get; set; }

            [JsonPropertyName("views")]
            public required List<MaskModel> Views { get; set; }

        }
    }
    