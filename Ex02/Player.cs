namespace PlayerNameSpace
{
    public class Player
    {
        private readonly string r_Name;
        private int m_Score = 0;
        private bool m_IsComputer = false;
        public Player(string i_name, bool i_isComputer)
        {
            r_Name= i_name;
            m_IsComputer = i_isComputer;
        }
        
        public string Name
        {
            get
            {
                return r_Name;
            }
        }

        public int Score
        {
            get
            {
                return m_Score;
            }
            set
            {
                m_Score = value;
            }
        }

        public bool IsComputer
        {
            get
            {
                return m_IsComputer;
            }
            set
            {
                m_IsComputer = value;
            }
        }
    }
}
