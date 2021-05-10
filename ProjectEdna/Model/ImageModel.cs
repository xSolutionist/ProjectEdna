using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectEdna.Model
{
    public class ImageModel
    {
        public int ID { get; set; }

        public string Project { get; set; }

        public int ProjectId { get; set; }


        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Display(Name = "Sample ID")]
        public string TestID { get; set; }

        [Display(Name = "GPS Longitude")]
        public double? Longitude { get; set; } // WGS 84 Long

        [Display(Name = "GPS Latitude")]
        public double? Latitude { get; set; } // WGS 84 LAT

        [Display(Name = "GPS Altitude")]
        public double? Altitude { get; set; }

        [Display(Name = "Material")]
        public string Material { get; set; } // snow mud etc

        [Display(Name = "Amount of tracks collected")]
        public int AmountOfTracksCollected { get; set; }

        [Display(Name = "Temperature °C")]
        public double Temperature { get; set; }

        [Display(Name = "What kind of snow")]
        public string Snow { get; set; } // "Bild, fin snö" Vettefan vad man menar

        [Display(Name = "What Animal")]
        public string AnimalKind { get; set; } // Lynx, Snow leopard etc

        [Display(Name = "Gender")]
        public string Gender { get; set; } // M / F

        [Display(Name = "Track Age")]
        public string TrackAge { get; set; } // ålder spår
        
        [Display(Name = "Collected by")]
        public string CollectedBy { get; set; } // Insamlare 

        [Display(Name = "Comment")]
        public string Comment { get; set; } // Kommetar från insamlare i guess

        [Display(Name = "User ID")]
        public string UserId { get; set; }

        [Display(Name = "Image")]
        public string Image { get; set; }

        [Display(Name = "QR Code")]
        public string QRCode { get; set; }

        // En prototyp
    }
}
