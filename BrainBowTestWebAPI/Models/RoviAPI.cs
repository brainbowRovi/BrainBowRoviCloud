using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Helpers;
using Newtonsoft.Json.Linq;

namespace BrainBowTestWebAPI.Models
{
    public class RoviAPI
    {
        private const string RoviAppId = "7fk63kqh6es89qh75gvn6b9a";
        private const string RoviAppSecretKey = "tPbdSg4AWG";

        public JObject GetMovieByKeyword(string Keyword)
        {
            string OAuthSessionToken = GetAccessTokenFromCode( RoviAppId, RoviAppSecretKey );
             return GetRovi_Movie_Metadata( OAuthSessionToken, Keyword);
        }

        private static JObject GetRovi_Movie_Metadata(string oAuthSessionToken, string keyword)
        {
            string offset = RandNumber(1,300).ToString();

            string roviSearchMovieByKeywordURL =
                string.Format(
                    "http://api.rovicorp.com/search/v2.1/amgvideo/search?entitytype=movie&query=%2A&rep=1&filter=genreid%3Ad+++0647%2Creleaseyear%3A2008&include=cast&size=20&offset={3}&language=en&country=US&format=json&apikey={1}&sig={2}",
                    keyword, RoviAppId, oAuthSessionToken, offset);
            //"http://sr-prod.rovicorp.com:8080/rovi-snr-ws-2/rest/phoenix_global/search?size=20&offset=0&modifiers=restriction%3bcount&types=CosmoMovie&fields=name&query=hanks";

            string results = string.Empty;

            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create( roviSearchMovieByKeywordURL );
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse( );

                StreamReader sr = new StreamReader( resp.GetResponseStream( ) );
                results = sr.ReadToEnd( );

                sr.Close( );
            }
            catch (Exception e)
            {
                if (e.Message.Contains( "400" ))
                {
                    //invalid reponse
                }
            }

            JObject jo = JObject.Parse( results );
            return jo;
//            string response = requestRoviData( userLikeUrl );
//            Console.WriteLine( response );
            //ParseResponse( response );
        }

        public static int RandNumber(int Low, int High)
        {
            Random rndNum = new Random(int.Parse(Guid.NewGuid().ToString().Substring(0, 8), System.Globalization.NumberStyles.HexNumber));

            int rnd = rndNum.Next(Low, High);

            return rnd;
        }

        public JObject GetCelebByName(string Keyword)
        {
            string OAuthSessionToken = GetAccessTokenFromCode(RoviAppId, RoviAppSecretKey);
            return GetRovi_Celeb_Metadata(OAuthSessionToken, Keyword);
        }

        private static JObject GetRovi_Celeb_Metadata(string oAuthSessionToken, string keyword)
        {
            string roviSearchCelebByNameURL =
                string.Format(
                    "http://api.rovicorp.com/search/v2.1/amgvideo/search?entitytype=movie&query=%2A&rep=1&filter=genreid%3A{0}&size=20&offset=0&language=en&country=US&format=json&apikey={1}&sig={2}",
                    keyword, RoviAppId, oAuthSessionToken);
            //"http://sr-prod.rovicorp.com:8080/rovi-snr-ws-2/rest/phoenix_global/search?size=20&offset=0&modifiers=restriction%3bcount&types=CosmoMovie&fields=name&query=hanks";

            string results = string.Empty;

            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(roviSearchCelebByNameURL);
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

                StreamReader sr = new StreamReader(resp.GetResponseStream());
                results = sr.ReadToEnd();

                sr.Close();
            }
            catch (Exception e)
            {
                if (e.Message.Contains("400"))
                {
                    //invalid reponse
                }
            }

            JObject jo = JObject.Parse(results);
            return jo;
            //            string response = requestRoviData( userLikeUrl );
            //            Console.WriteLine( response );
            //ParseResponse( response );
        }


#region GetAccessToken
        public static string GetAccessTokenFromCode(string AppId, string SecretKey)
        {
            //get the timestamp value
            string timestamp = (DateTime.UtcNow - new DateTime( 1970, 1, 1, 0, 0, 0 )).TotalSeconds.ToString( );

            //grab just the integer portion
            timestamp = timestamp.Substring( 0, timestamp.IndexOf( "." ) );

            //set the API key (note that this is not a valid key!
            string apikey = RoviAppId;

            //set the shared secret key
            string secret = RoviAppSecretKey;

            //call the function to create the hash
            string sig = CreateMD5Hash( apikey + secret + timestamp );

            return sig;
        }

        //note, requires "using System.Security.Cryptography;"
        public static string CreateMD5Hash(string input)
        {
            // Use input string to calculate MD5 hash
            MD5 md5 = MD5.Create( );
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes( input );
            byte[] hashBytes = md5.ComputeHash( inputBytes );

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder( );
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append( hashBytes[i].ToString( "x2" ) );  //this will use lowercase letters, use "X2" instead of "x2" to get uppercase
            }
            return sb.ToString( );
        }
#endregion 

    }
}