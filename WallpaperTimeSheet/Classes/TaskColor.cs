using WallpaperTimeSheet.Utills;

namespace WallpaperTimeSheet.Classes
{
    public class TaskColor
    {
        public string HexColor { get; set; }

        public Color SystemDrawingColor() { 
            return ColorsUtilis.ToColor(HexColor);
        }

        public TaskColor(string color)
        {
            HexColor = color;
        }
    }

    public static class TaskColors
    {
        public static readonly TaskColor GialloOro = new TaskColor("#ffb900");
        public static readonly TaskColor Oro = new TaskColor("#ff8c00");
        public static readonly TaskColor ArancioBrillante = new TaskColor("#f7630c");
        public static readonly TaskColor ArancioneScuro = new TaskColor("#ca5010");
        public static readonly TaskColor Ruggine = new TaskColor("#da3b01");
        public static readonly TaskColor RugginePallido = new TaskColor("#ef6950");
        public static readonly TaskColor RossoMattone = new TaskColor("#d13438");
        public static readonly TaskColor RossoModerno = new TaskColor("#ff4343");
        public static readonly TaskColor RossoPallido = new TaskColor("#e74856");
        public static readonly TaskColor Rosso = new TaskColor("#e81123");
        public static readonly TaskColor RosaBrillante = new TaskColor("#ea005e");
        public static readonly TaskColor Rosa = new TaskColor("#c30052");
        public static readonly TaskColor PrugnaChiaro = new TaskColor("#e3008c");
        public static readonly TaskColor Prugna = new TaskColor("#bf0077");
        public static readonly TaskColor OrchideaChiaro = new TaskColor("#c239b3");
        public static readonly TaskColor Orchidea = new TaskColor("#9a0089");
        public static readonly TaskColor Blu = new TaskColor("#0078d4");
        public static readonly TaskColor BluScuro = new TaskColor("#0063b1");
        public static readonly TaskColor Viola = new TaskColor("#8e8cd8");
        public static readonly TaskColor ViolaScuro = new TaskColor("#6b69d6");
        public static readonly TaskColor IrisPastello = new TaskColor("#8764b8");
        public static readonly TaskColor IrisPrimaverile = new TaskColor("#744da9");
        public static readonly TaskColor RossoViolaChiaro = new TaskColor("#b146c2");
        public static readonly TaskColor RossoViola = new TaskColor("#881798");
        public static readonly TaskColor BluFreddoBrillante = new TaskColor("#0099bc");
        public static readonly TaskColor BluFreddo = new TaskColor("#2d7d9a");
        public static readonly TaskColor SpumaMarina = new TaskColor("#00b7c3");
        public static readonly TaskColor VerdeAcqua = new TaskColor("#038387");
        public static readonly TaskColor VerdeMentaChiaro = new TaskColor("#00b294");
        public static readonly TaskColor VerdeMentaScuro = new TaskColor("#18574e");
        public static readonly TaskColor VerdeErba = new TaskColor("#00cc6a");
        public static readonly TaskColor VerdeSport = new TaskColor("#10893e");
        public static readonly TaskColor Grigio = new TaskColor("#7a7574");
        public static readonly TaskColor MarroneGrigio = new TaskColor("#5d5a58");
        public static readonly TaskColor BluAcciaio = new TaskColor("#68768a");
        public static readonly TaskColor BluMetallico = new TaskColor("#515c6b");
        public static readonly TaskColor VerdeMuschioPallido = new TaskColor("#567c73");
        public static readonly TaskColor VerdeMuschioScuro = new TaskColor("#486860");
        public static readonly TaskColor VerdePrato = new TaskColor("#498205");
        public static readonly TaskColor Verde = new TaskColor("#107c10");
        public static readonly TaskColor GrigioPlumbeo = new TaskColor("#767676");
        public static readonly TaskColor GrigioTemporale = new TaskColor("#4c4a48");
        public static readonly TaskColor GrigioBlu = new TaskColor("#69797e");
        public static readonly TaskColor GrigioScuro = new TaskColor("#4a5459");
        public static readonly TaskColor VerdeNinfea = new TaskColor("#647c64");
        public static readonly TaskColor Salvia = new TaskColor("#525e54");
        public static readonly TaskColor MimeticoDeserto = new TaskColor("#847545");
        public static readonly TaskColor Mimetico = new TaskColor("#7e735f");

        internal static TaskColor? GetColorByHex(string color)
        {
            var properties = typeof(TaskColors).GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);

            foreach (var property in properties)
            {
                if (property.GetValue(null) is TaskColor taskColor && taskColor.HexColor.Equals(color, StringComparison.OrdinalIgnoreCase))
                {
                    return taskColor;
                }
            }

            return null;
        }

    }
}
