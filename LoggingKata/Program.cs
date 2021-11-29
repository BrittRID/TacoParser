using System;
using System.Linq;
using System.IO;
using GeoCoordinatePortable;

namespace LoggingKata
{
   public class Program
    {
        static readonly ILog logger = new TacoLogger();
        const string csvPath = "TacoBell-US-AL.csv";

        static void Main(string[] args)
        {
           
            //Find taco bell farthest location 
            logger.LogInfo("Log initialized");

            //Get information from csv file and detected error/warmings
            string[] lines = File.ReadAllLines(csvPath);

            if(lines.Length == 0)
            {
                logger.LogError( "file has no input");
            }

            if(lines.Length == 1)
            {
                logger.LogWarning("file only has one line of input");
            }

            logger.LogInfo($"Lines: {lines[0]}");


            //  new instance of TacoParser class
            TacoParser parser = new TacoParser();

            //  IEnumerable of locations using the Select command: var locations = lines.Select(parser.Parse);
            var locations = lines.Select(parser.Parse).ToArray();

          //Use to store two taco bells locations
            ITrackable tacoBell1 = null;
            ITrackable tacoBell2 = null;
            double distance = 0;
            
            //For loop to grab each location & Geolocation toolbox
            for (int i = 0; i < locations.Length; i++)
            {
                
                // New corA Coodinate with latitude and longitude
                var LocA = locations[i];
              
                var corA = new GeoCoordinate();
                corA.Latitude = LocA.Location.Latitude;
                corA.Longitude = LocA.Location.Longitude;

                //Grab destination location 
                for (int j = 0; j < locations.Length; j++)
                {
                    var LocB = locations[j];

                    var corB = new GeoCoordinate();
                    corB.Latitude = LocB.Location.Latitude;
                    corB.Longitude = LocB.Location.Longitude;

                   
                    //Get distance from 2 taco bell location
                    if (corA.GetDistanceTo(corB) > distance)
                    {
                        distance = corA.GetDistanceTo(corB);
                        tacoBell1 = LocA;
                        tacoBell2 = LocB;

                    }

                }

            }
            //  found the two Taco Bells farthest away from each other.
             logger.LogInfo($"{tacoBell1.Name} and {tacoBell2.Name} are the farthest apart");
            
        }

    }
}
