using System.Collections;
using System.Collections.Generic;

namespace SevenSeas
{
    public class HighScoreModel
    {
        public string username;
        public int score;

        public HighScoreModel(string u, int s)
        {
            username = u;
            score = s;
        }

        public static HighScoreModel FromPipeDreamlo(string strData)
        {
            string[] cols = strData.Split('|');
            return new HighScoreModel(cols[0], int.Parse(cols[1]));
        }
    }

}
