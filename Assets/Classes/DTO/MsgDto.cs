using System;

namespace Classes.DTO
{
    [Serializable]
    public class MsgDto
    {
        public string content { set; get; }
        public string sender { set; get; }
        public long timestamp { set; get; }
        
        public MsgDto()
        {
            
        }
    }
}